using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AMS.ReportAutomation.Data.Exceptions;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Data.ViewModel;
using Google.Apis.Webmasters.v3;
using Google.Apis.Webmasters.v3.Data;

namespace AMS.ReportAutomation.Data.Services.Crawler
{
    public class GoogleSearchConsole_Service : EntityService<Data_GoogleSearch>, IGoogleSearchConsole_Service
    {
        private IGoogleSearchConsole_Repository _googleSearchConsole_Repository;
        private IGoogleSearchConsole_DetailRepository _googleSearchConsole_DetailRepository;
        private IGoogleSearchConsole_SitemapRepository _googleSearchConsole_SitemapRepository;
        private IReport_ClientSitesRepository _report_ClientSitesRepository;
        private GoogleSearch.GoogleSearch _googleSearch = new GoogleSearch.GoogleSearch(new string[] { WebmastersService.Scope.Webmasters });
        private WebmastersService webmastersService;
        public string GoogleCredentialsFolder { get; set; }
        public GoogleSearchConsole_Service(IGoogleSearchConsole_Repository googleSearchConsole_Repository, IGoogleSearchConsole_DetailRepository googleSearchConsole_DetailRepository, IReport_ClientSitesRepository report_ClientSitesRepository, IGoogleSearchConsole_SitemapRepository googleSearchConsole_SitemapRepository, IAMSLogger logger) : base(googleSearchConsole_Repository, logger)
        {
            _googleSearchConsole_Repository = googleSearchConsole_Repository;
            _googleSearchConsole_DetailRepository = googleSearchConsole_DetailRepository;
            _googleSearchConsole_SitemapRepository = googleSearchConsole_SitemapRepository;
            _report_ClientSitesRepository = report_ClientSitesRepository;
        }

        public void GetReportDataAndSaveToDB(GoogleSearchSite clientSite)
        {
            //Unfortunately, Google Seach Console does not have ServiceAccount as an authenticated way to get data
            //TODO: Investigate if a normal API Key can be used for convenience instead of OAuth authenticate
            //if (!string.IsNullOrEmpty(clientSite.CrawlerConfig.GoogleServiceAccSecretJsonFile))
            //{
            //    webmastersService = _googleSearch.AuthenticateServiceAccount(System.IO.Path.Combine(GoogleCredentialsFolder, clientSite.CrawlerConfig.GoogleServiceAccSecretJsonFile));
            //}
            //else
            if (!string.IsNullOrEmpty(clientSite.CrawlerConfig.GoogleOauthAccSecretJsonFile))
            {
                webmastersService = _googleSearch.AuthenticateOauth(System.IO.Path.Combine(GoogleCredentialsFolder, clientSite.CrawlerConfig.GoogleOauthAccSecretJsonFile), clientSite.CrawlerConfig.GoogleOauthAccEmail);
            }

            if (webmastersService != null)
            {
                var utcNow = DateTime.UtcNow;
                var endOfYesterdayUtc = (new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0)).AddSeconds(-1);

                var startDate = endOfYesterdayUtc.Subtract(TimeSpan.FromDays(31)).ToString("yyyy-MM-dd");
                var endDate = endOfYesterdayUtc.ToString("yyyy-MM-dd");

                //Get the top keywords (with highest number of clicks)
                var keywords = _googleSearch.GetSearchAnalyticData(webmastersService, clientSite.CrawlerConfig.GCSearchUrl, new List<string> { "query" }, startDate, endDate, rowLimit: 20);

                SaveGoogleSearchReportData(clientSite, keywords, startDate, endDate, endOfYesterdayUtc.ToUnixTimestamp());

                var siteMaps = webmastersService.Sitemaps.List(clientSite.CrawlerConfig.GCSearchUrl).Execute().Sitemap;

                SaveGoogleSearchSiteMapReport(siteMaps,clientSite.ClientSiteId, endOfYesterdayUtc.ToUnixTimestamp());
            }
        }

        protected void SaveGoogleSearchReportData(GoogleSearchSite config, SearchAnalyticsQueryResponse keywords, string startDate, string endDate, long unixDataTimeUtc)
        {
            if (keywords != null)
            {
                var googleSearchConsole = new Data_GoogleSearch
                {
                    Id = Guid.NewGuid(),
                    ClientSiteId = config.ClientSiteId,
                    Json = JsonConvert.SerializeObject(keywords.Rows, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                    DataTimeUtc = unixDataTimeUtc,
                    CrawledTimestamp = DateTime.Now
                };
                var existedData = _googleSearchConsole_Repository
                    .FindFirstOrDefault(x => x.ClientSiteId == config.ClientSiteId && x.DataTimeUtc == unixDataTimeUtc);
                if (existedData == null)
                {
                    _googleSearchConsole_Repository.Add(googleSearchConsole);
                }
                else
                {
                    existedData.Json = JsonConvert.SerializeObject(keywords.Rows);
                    existedData.CrawledTimestamp = DateTime.Now;
                    _googleSearchConsole_Repository.Edit(existedData);
                }
                _googleSearchConsole_Repository.Save();

                //Get detailed stats of each keyword
                foreach (var keywordsRow in keywords.Rows)
                {
                    var keyword = keywordsRow.Keys[0];
                    var report = _googleSearch.GetSearchAnalyticData(webmastersService, config.CrawlerConfig.GCSearchUrl, config.CrawlerConfig.GCDimensions.Split(',').ToList(), startDate, endDate, keyword: keyword);

                    SaveGoogleSearchDetailReport(existedData?.Id ?? googleSearchConsole.Id, keyword, report, unixDataTimeUtc);
                }

            }
        }

        protected void SaveGoogleSearchDetailReport(Guid googleSearchId, string keyword, SearchAnalyticsQueryResponse reports, long unixDataTimeUtc)
        {
            if (reports != null)
            {
                var googleSearchDetail = new Data_GoogleSearch_Detail
                {
                    Id = Guid.NewGuid(),
                    GoogleSearchId = googleSearchId,
                    Keyword = keyword,
                    Json = JsonConvert.SerializeObject(reports.Rows, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                    DataTimeUtc = unixDataTimeUtc,
                    CrawledTimestamp = DateTime.Now
                };
                var existedData = _googleSearchConsole_DetailRepository
                    .FindFirstOrDefault(x => x.GoogleSearchId == googleSearchId && x.DataTimeUtc == unixDataTimeUtc && x.Keyword == keyword);
                if (existedData == null)
                {
                    _googleSearchConsole_DetailRepository.Add(googleSearchDetail);
                }
                else
                {
                    existedData.Json = JsonConvert.SerializeObject(reports.Rows);
                    existedData.CrawledTimestamp = DateTime.Now;
                    _googleSearchConsole_DetailRepository.Edit(existedData);
                }
                _googleSearchConsole_DetailRepository.Save();
            }
        }

        protected void SaveGoogleSearchSiteMapReport(IList<WmxSitemap> sitemapContents, Guid clientSiteId, long unixDataTimeUtc)
        {
            if (sitemapContents != null)
            {
                var googleSearchSitemap = new Data_GoogleSearch_Sitemap
                {
                    Id = Guid.NewGuid(),
                    ClientSiteId = clientSiteId,
                    Json = JsonConvert.SerializeObject(sitemapContents, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                    DataTimeUtc = unixDataTimeUtc,
                    CrawledTimestamp = DateTime.Now
                };
                var existedData = _googleSearchConsole_SitemapRepository
                    .FindFirstOrDefault(x => x.ClientSiteId == clientSiteId && x.DataTimeUtc == unixDataTimeUtc);
                if (existedData == null)
                {
                    _googleSearchConsole_SitemapRepository.Add(googleSearchSitemap);
                }
                else
                {
                    existedData.Json = JsonConvert.SerializeObject(sitemapContents);
                    existedData.CrawledTimestamp = DateTime.Now;
                    _googleSearchConsole_SitemapRepository.Edit(existedData);
                }
                _googleSearchConsole_SitemapRepository.Save();
            }
        }

        public List<GoogleSearchSite> GetClientSites()
        {
            var data = _report_ClientSitesRepository.FindBy(x => x.CrawlerConfig != null && x.CrawlerConfig != string.Empty);

            var list = new List<GoogleSearchSite>();

            if (data != null)
            {
                foreach (var site in data)
                {
                    dynamic crawlerconfig = JsonConvert.DeserializeObject(site.CrawlerConfig);
                    if (crawlerconfig.GoogleSearch != null)
                    {
                        var client = new GoogleSearchSite
                        {
                            ClientSiteId = site.Id,
                            SiteUrl = site.SiteUrl,
                            CrawlerConfig = JsonConvert.DeserializeObject<GoogleSearchCrawlerConfig>(crawlerconfig.GoogleSearch.ToString())
                        };

                        if (!string.IsNullOrWhiteSpace(client.CrawlerConfig.GCDimensions)) list.Add(client);
                    }
                }
            }

            return list;
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _googleSearchConsole_Repository.BatchDelete(x => x.DataTimeUtc < retentionTimeCalc);
        }
    }
}