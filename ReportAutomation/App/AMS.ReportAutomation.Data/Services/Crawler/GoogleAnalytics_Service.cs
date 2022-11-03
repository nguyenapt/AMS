using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Data.ViewModel;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;

namespace AMS.ReportAutomation.Data.Services.Crawler
{
    public class GoogleAnalytics_Service : EntityService<Data_GoogleAnalytic>, IGoogleAnalytics_Service
    {
        private IGoogleAnalytics_Repository _googleAnalyticsRepository;
        private IReport_ClientSitesRepository _report_ClientSitesRepository;
        public string GoogleCredentialsFolder { get; set; }

        public GoogleAnalytics_Service(IGoogleAnalytics_Repository googleAnalyticsRepository, IReport_ClientSitesRepository report_ClientSitesRepository, IAMSLogger logger) : base(googleAnalyticsRepository, logger)
        {
            _googleAnalyticsRepository = googleAnalyticsRepository;
            _report_ClientSitesRepository = report_ClientSitesRepository;
        }

        public void GetReportDataAndSaveToDB(GoogleAnalyticsSite clientSite)
        {
            var googleAnalytic = new GoogleAnalytic.GoogleAnalytic(new string[] { AnalyticsReportingService.Scope.AnalyticsReadonly }, clientSite.CrawlerConfig.GAViewId);
            AnalyticsReportingService analyticReport = null;
            if (!string.IsNullOrEmpty(clientSite.CrawlerConfig.GoogleServiceAccSecretJsonFile))
            {
                analyticReport = googleAnalytic.AuthenticateServiceAccount(System.IO.Path.Combine(GoogleCredentialsFolder, clientSite.CrawlerConfig.GoogleServiceAccSecretJsonFile));
            }
            else if (!string.IsNullOrEmpty(clientSite.CrawlerConfig.GoogleOauthAccSecretJsonFile))
            {
                analyticReport = googleAnalytic.AuthenticateOauth(System.IO.Path.Combine(GoogleCredentialsFolder, clientSite.CrawlerConfig.GoogleOauthAccSecretJsonFile), clientSite.CrawlerConfig.GoogleOauthAccEmail);
            }

            if (analyticReport != null)
            {
                var utcNow = DateTime.UtcNow;
                var endOfYesterdayUtc = (new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0)).AddSeconds(-1);

                DateRange dateRange = new DateRange
                {
                    StartDate = endOfYesterdayUtc.Subtract(TimeSpan.FromDays(61)).ToString("yyyy-MM-dd"),
                    EndDate = endOfYesterdayUtc.ToString("yyyy-MM-dd")
                };

                var reports = googleAnalytic.GetAnalyticsData(analyticReport, new List<DateRange> { dateRange });
                SaveReport(clientSite, reports, endOfYesterdayUtc.ToUnixTimestamp());
            }
        }

        protected void SaveReport(GoogleAnalyticsSite config, Dictionary<string, List<Report>> reports, long unixDataTimeUtc)
        {
            if (reports != null && reports.Any())
            {
                foreach (var datasetType in reports)
                {
                    foreach (var report in datasetType.Value)
                    {
                        ColumnHeader header = report.ColumnHeader;
                        List<string> dimensionHeaders = (List<string>)header.Dimensions;

                        List<MetricHeaderEntry> metricHeaders =
                            (List<MetricHeaderEntry>)header.MetricHeader.MetricHeaderEntries;
                        List<ReportRow> rows = (List<ReportRow>)report.Data.Rows;

                        var googleAnalytic = new Data_GoogleAnalytic
                        {
                            Id = Guid.NewGuid(),
                            DatasetType = datasetType.Key,
                            ClientSiteId = config.ClientSiteId,
                            Json = JsonConvert.SerializeObject(rows, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                            DataTimeUtc = unixDataTimeUtc,
                            CrawledTimestamp = DateTime.Now
                        };
                        var existedData = _googleAnalyticsRepository
                            .FindFirstOrDefault(x => x.DatasetType == datasetType.Key && x.ClientSiteId == config.ClientSiteId && x.DataTimeUtc == unixDataTimeUtc);
                        if (existedData == null)
                        {
                            _googleAnalyticsRepository.Add(googleAnalytic);
                        }
                        else
                        {
                            existedData.Json = JsonConvert.SerializeObject(rows);
                            existedData.CrawledTimestamp = DateTime.Now;
                            _googleAnalyticsRepository.Edit(existedData);
                        }
                        _googleAnalyticsRepository.Save();
                    }
                }   
            }
        }

        public List<GoogleAnalyticsSite> GetClientSites()
        {
            var data = _report_ClientSitesRepository.FindBy(x => x.CrawlerConfig != null && x.CrawlerConfig != string.Empty);

            var list = new List<GoogleAnalyticsSite>();

            if (data != null)
            {
                foreach (var site in data)
                {
                    dynamic crawlerconfig = JsonConvert.DeserializeObject(site.CrawlerConfig);
                    if (crawlerconfig.GoogleAnalytics != null)
                    {
                        var client = new GoogleAnalyticsSite
                        {
                            ClientSiteId = site.Id,
                            SiteUrl = site.SiteUrl,
                            CrawlerConfig = JsonConvert.DeserializeObject<GoogleAnalyticsCrawlerConfig>(crawlerconfig.GoogleAnalytics.ToString())
                        };

                        if (!string.IsNullOrWhiteSpace(client.CrawlerConfig.GAViewId)) list.Add(client);
                    }
                }
            }

            return list;
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _googleAnalyticsRepository.BatchDelete(x => x.DataTimeUtc < retentionTimeCalc);
        }
    }
}