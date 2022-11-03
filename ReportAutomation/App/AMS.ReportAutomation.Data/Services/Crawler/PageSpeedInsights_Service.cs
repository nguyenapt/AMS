using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using System;
using AMS.PageSpeed;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;
using AMS.ReportAutomation.Data.ViewModel;
using AMS.PageSpeed.Contracts;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;

namespace AMS.ReportAutomation.Data.Services.Crawler
{
    public class PSIRequestParams
    {
        public string Category = "performance";
        public string Locale = "en";
        public string Strategy = "DESKTOP";
    }

    public class PageSpeedInsights_Service : EntityService<Data_PageSpeedInsights>, IPageSpeedInsights_Service
    {
        private IPageSpeedInsights_Repository _pageSpeedInsights_Repository;
        private IReport_ClientSitesRepository _report_ClientSitesRepository;
        //Multi-threading
        private readonly object dbOperationThreadLock = new object();

        public PageSpeedInsights_Service(IPageSpeedInsights_Repository pageSpeedInsights_Repository, IReport_ClientSitesRepository report_ClientSitesRepository, IAMSLogger logger)
            : base(pageSpeedInsights_Repository, logger)
        {
            _pageSpeedInsights_Repository = pageSpeedInsights_Repository;
            _report_ClientSitesRepository = report_ClientSitesRepository;
        }


        //TODO: Finalize this list for what we need to crawl (will we add utm_campaign and utm_source here?)
        //Ref: https://developers.google.com/speed/docs/insights/v5/reference/pagespeedapi/runpagespeed (Parametters)
        public static List<PSIRequestParams> CrawlParamList()
        {
            var list = new List<PSIRequestParams>();

            list.Add(new PSIRequestParams
            {
                Category = "performance",
                Locale = "en",
                Strategy = "DESKTOP"
            });


            list.Add(new PSIRequestParams
            {
                Category = "performance",
                Locale = "en",
                Strategy = "MOBILE"
            });


            list.Add(new PSIRequestParams
            {
                Category = "accessibility",
                Locale = "en",
                Strategy = "DESKTOP"
            });


            list.Add(new PSIRequestParams
            {
                Category = "accessibility",
                Locale = "en",
                Strategy = "MOBILE"
            });


            list.Add(new PSIRequestParams
            {
                Category = "best-practices",
                Locale = "en",
                Strategy = "DESKTOP"
            });


            list.Add(new PSIRequestParams
            {
                Category = "best-practices",
                Locale = "en",
                Strategy = "MOBILE"
            });


            list.Add(new PSIRequestParams
            {
                Category = "seo",
                Locale = "en",
                Strategy = "DESKTOP"
            });


            list.Add(new PSIRequestParams
            {
                Category = "seo",
                Locale = "en",
                Strategy = "MOBILE"
            });


            list.Add(new PSIRequestParams
            {
                Category = "pwa",
                Locale = "en",
                Strategy = "DESKTOP"
            });


            list.Add(new PSIRequestParams
            {
                Category = "pwa",
                Locale = "en",
                Strategy = "MOBILE"
            });

            return list;
        }

        private List<string> GetPropertyKeysForDynamic(dynamic dynamicToGetPropertiesFor)
        {
            Newtonsoft.Json.Linq.JObject attributesAsJObject = dynamicToGetPropertiesFor;
            Dictionary<string, object> values = attributesAsJObject.ToObject<Dictionary<string, object>>();
            List<string> toReturn = new List<string>();
            foreach (string key in values.Keys)
            {
                toReturn.Add(key);
            }
            return toReturn;
        }
        
        public List<PSICheckSite> GetClientSites()
        {
            //Use the url from CrawlerConfig value
            var data = _report_ClientSitesRepository.FindBy(x => x.CrawlerConfig != null && x.CrawlerConfig != string.Empty);

            var list = new List<PSICheckSite>();

            if (data != null)
            {
                foreach (var site in data)
                {
                    var client = new PSICheckSite
                    {
                        ClientSiteId = site.Id,
                        CrawlerConfig = JsonConvert.DeserializeObject<PSICrawlerConfig>(site.CrawlerConfig)
                    };

                    if (!string.IsNullOrWhiteSpace(client.CrawlerConfig.PsiUrl)) list.Add(client);
                }
            }

            return list;
        }

        public Data_PageSpeedInsights GetPageSpeedData(PSICheckSite siteInfo, PSIRequestParams param, long batchUnixTimestamp)
        {
            if (param == null) throw new ArgumentNullException();

            var configuration = new PageSpeedConfiguration();
            if (string.IsNullOrWhiteSpace(configuration.ApiName)) throw new Exception("You must configure Google PageSpeed Insights authentication!");
            var pageSpeed = new PageSpeedService(configuration);

            var url = siteInfo.CrawlerConfig.PsiUrl;
            if (url != null && !url.Contains("https://") && !url.Contains("http://"))
            {
                url = "https://" + url;
            }
            var method = System.Net.Http.HttpMethod.Get;
            var apiName = configuration.ApiName;

            var listParam = new Dictionary<string, string>
            {
                { "category", param.Category },
                { "locale", param.Locale },
                { "strategy", param.Strategy },
                { "url", url }
            };

            var data = pageSpeed.PageSpeedRequest(method, apiName, listParam).Result;
            //_logger.Debug(data);
            var responseObj = JsonConvert.DeserializeObject<PageSpeedInsightsJson>(data);
            //List<string> auditProperties = GetPropertyKeysForDynamic(responseObj.LighthouseResult.Audits);
            //_logger.Information("--Audit properties:" + string.Join(",", auditProperties));

            //Clean-up unused data to save storage space and performance (data processing speed when it is used)
            if (responseObj != null && responseObj.LighthouseResult != null && responseObj.LighthouseResult.Audits != null)
            {
                //_logger.Debug("final-screenshot: " + responseObj.LighthouseResult.Audits["final-screenshot"]);
                responseObj.LighthouseResult.Audits["final-screenshot"] = null;
            }

            //Sleep to help CPU work better, and not to DDOS Pingdom System
            Thread.Sleep(TimeSpan.FromSeconds(2));

            if (responseObj != null)
            {
                return new Data_PageSpeedInsights
                {
                    Id = Guid.NewGuid(),
                    ClientSiteId = siteInfo.ClientSiteId,
                    Url = responseObj.Id ?? siteInfo.CrawlerConfig.PsiUrl,
                    Param = JsonConvert.SerializeObject(param),
                    Json = JsonConvert.SerializeObject(responseObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                    //DataTimeUtc = DateTimeOffset.Parse(responseObj.AnalysisUTCTimestamp).UtcDateTime.ToUnixTimestamp(),
                    //Override DataTimeUtc value by the crawled timestamp of the whole batch (as all fetch time values of API calls are very close) so the report looks nicer
                    DataTimeUtc = batchUnixTimestamp,
                    CrawledTimestamp = DateTime.Now
                };
            }

            return null;
        }

        /// <summary>
        /// Save to database.
        /// </summary>
        /// <param name="data"></param>
        public void SavePageSpeedData(Data_PageSpeedInsights data)
        {
            if (data != null)
            {
                lock (dbOperationThreadLock)
                {
                    _pageSpeedInsights_Repository.Add(data);
                    this.SafeExecute(() => _pageSpeedInsights_Repository.Save());
                }
            }
        }

        /// <summary>
        /// Delete old data in the database.
        /// </summary>
        /// <param name="retentionTime"></param>
        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _pageSpeedInsights_Repository.BatchDelete(x => x.DataTimeUtc < retentionTimeCalc);

            //Note: To avoid timeout, you may do smt like this
            //LOOP
            //Delete from Data_PageSpeedInsights where Id in (
            // select top 100 Id from Data_PageSpeedInsights where DataTimeUtc < retentionTimeCalc
            //)
        }
    }
}