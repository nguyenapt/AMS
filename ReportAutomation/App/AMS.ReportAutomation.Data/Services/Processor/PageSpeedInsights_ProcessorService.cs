using AMS.PageSpeed.Contracts;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.ReportData.PageSpeed;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class PageSpeedInsights_ProcessorService : IPageSpeedInsights_ProcessorService
    {
        private readonly IReport_ReportsRepository _report_ReportsRepository;

        private readonly IReportData_PageSpeedRepository _reportData_PageSpeedRepository;
        private readonly IPageSpeedInsights_Repository _pageSpeedInsights_Repository;
        private readonly IAMSLogger _logger;


        public PageSpeedInsights_ProcessorService(IReport_ReportsRepository report_ReportsRepository, IReportData_PageSpeedRepository reportData_PageSpeedRepository, IPageSpeedInsights_Repository pageSpeedInsights_Repository, IAMSLogger logger)
            : base()
        {
            _report_ReportsRepository = report_ReportsRepository;
            _reportData_PageSpeedRepository = reportData_PageSpeedRepository;
            _pageSpeedInsights_Repository = pageSpeedInsights_Repository;
            _logger = logger;
        }

        public void Process()
        {
            //Get the list of active clientSites to report from the database
            var listActiveReportsFromDB = _report_ReportsRepository.FindBy(x => x.IsActive == true);

            foreach (var report in listActiveReportsFromDB)
            {
                dynamic reportConfig = JsonConvert.DeserializeObject(report.ToolIds);
                if ((reportConfig.PSIPerformanceEnabled != null && ((string)reportConfig.PSIPerformanceEnabled).ToLower() != "false" && ((string)reportConfig.PSIPerformanceEnabled).ToLower() != "0")
                    || (reportConfig.PSIAccessibilityEnabled != null && ((string)reportConfig.PSIAccessibilityEnabled).ToLower() != "false" && ((string)reportConfig.PSIAccessibilityEnabled).ToLower() != "0")
                    || (reportConfig.PSISeoEnabled != null && ((string)reportConfig.PSISeoEnabled).ToLower() != "false" && ((string)reportConfig.PSISeoEnabled).ToLower() != "0")
                    || (reportConfig.PSIPwaEnabled != null && ((string)reportConfig.PSIPwaEnabled).ToLower() != "false" && ((string)reportConfig.PSIPwaEnabled).ToLower() != "0")
                    )
                {
                    //Find the latest time the data was processed => only process the records that have not been processed (to avoid large memory consumption or timeout)
                    var lastProcessedTime = _reportData_PageSpeedRepository.FindMax(x => x.ClientSiteId == report.ClientSiteId, x => x.ReportTimeUtc);
                    _logger.Information("- PSI Process report=" + report.Id + " clientSite=" + report.ClientSiteId + "(" + report.Report_ClientSites.SiteUrl + "). Found last processed time: " + lastProcessedTime);

                    IEnumerable<Data_PageSpeedInsights> data;
                    if (lastProcessedTime == null)
                    {
                        //Never processed before, process all data found
                        //TODO: If possible, optimize this a little bit, because it can encounter timeout or outOfMemory exception if the total number of data records is big(here it is selecting ALL)
                        data = _pageSpeedInsights_Repository.FindBy(n => n.ClientSiteId == report.ClientSiteId);
                    }
                    else
                    {
                        //Process data that is 1-day old max
                        lastProcessedTime -= (long)TimeSpan.FromDays(1).TotalSeconds;
                        data = _pageSpeedInsights_Repository.FindBy(n => n.ClientSiteId == report.ClientSiteId && n.DataTimeUtc >= lastProcessedTime);
                    }
                    if (data != null)
                    {
                        _logger.Information("- PSI Process " + report.Id + ". Records to process: " + data.Count());
                        foreach (var item in data)
                        {
                            if (item.Json != null) MappingPageSpeedData(item);
                        }
                    }
                }
                //Release unmanaged resources to reduce memory consumption
                GC.Collect();
            }
        }

        private void MappingPageSpeedData(Data_PageSpeedInsights dataCheck)
        {
            if (dataCheck == null)
            {
                _logger.Error($"No PageSpeedInsights data to process for ClientSiteId={dataCheck.ClientSiteId}");
                return;
            }

            var dataObj = JsonConvert.DeserializeObject<PageSpeedInsightsJson>(dataCheck.Json);

            if (dataObj.LighthouseResult == null)
            {
                _logger.Error($"No PageSpeedInsights Lighthouse data to process for ClientSiteId={dataCheck.ClientSiteId}");
                return;
            }

            var fetchTime = DateTimeOffset.Parse(dataObj.LighthouseResult.FetchTime).UtcDateTime.ToUnixTimestamp();
            var emulator = dataObj.LighthouseResult.ConfigSettings.EmulatedFormFactor.ToLower() == Constants.Desktop ? Constants.Desktop : (dataObj.LighthouseResult.ConfigSettings.EmulatedFormFactor.ToLower() == Constants.Mobile ? Constants.Mobile : dataObj.LighthouseResult.ConfigSettings.EmulatedFormFactor);            
            CategoryInfo categoryInfo = null;
            string category = null;
            var categories = dataObj.LighthouseResult.Categories;
            if (categories.Performance != null)
            {
                category = Constants.Category_Performance;
                categoryInfo = categories.Performance;
            }
            else if (categories.Accessibility != null)
            {
                category = Constants.Category_Accessibility;
                categoryInfo = categories.Accessibility;
            }
            else if (categories.SEO != null)
            {
                category = Constants.Category_SEO;
                categoryInfo = categories.SEO;
            }
            else if (categories.PWA != null)
            {
                category = Constants.Category_PWA;
                categoryInfo = categories.PWA;
            }
            else if (categories.BestPractices != null)
            {
                category = Constants.Category_BestPractices;
                categoryInfo = categories.BestPractices;
            }
            
            if (categoryInfo == null)
            {
                throw new Exception($"Failed to process PageSpeedInsights Categories data of ClientSiteId={dataCheck.ClientSiteId}");
            }
            double score = categoryInfo.Score;
            var jsonData = new LighthouseJson()
            {
                UserAgent = dataObj.LighthouseResult.UserAgent,
                LightHouseVersion = dataObj.LighthouseResult.LightHouseVersion,
                AuditsJson = dataObj.LighthouseResult.Audits,
                AuditRefs = categoryInfo.AuditRefs,
                AuditCategoryGroups = dataObj.LighthouseResult.CategoryGroups
            };
            //Debug
            //_logger.Information("jsonData.AuditsJson:" + jsonData.AuditsJson);
            var jsonHistory = MappingHistoryData((Guid)dataCheck.ClientSiteId, category, emulator, fetchTime);

            var reportPageSpeed = _reportData_PageSpeedRepository
                .FindFirstOrDefault(x => x.ClientSiteId == (Guid)dataCheck.ClientSiteId
                    && x.ReportTimeUtc == fetchTime
                    && x.Emulator == emulator
                    && x.Category == category);

            if (reportPageSpeed == null)
            {
                var report_PageSpeedToAdd = new ReportData_PageSpeedInsights
                {
                    Id = Guid.NewGuid(),
                    ClientSiteId = dataCheck.ClientSiteId,
                    Url = dataCheck.Url,
                    Category = category,
                    Emulator = emulator,
                    Score = score,
                    //No need to store Loading Experience data if its category is not about Performance, and as the data will be duplicated
                    LoadingExperience = (categories.Performance != null) ? JsonConvert.SerializeObject(dataObj.LoadingExperience, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) : null,
                    OriginLoadingExperience = (categories.Performance != null) ? JsonConvert.SerializeObject(dataObj.OriginLoadingExperience, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) : null,
                    Json = JsonConvert.SerializeObject(jsonData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                    HistoryJson = JsonConvert.SerializeObject(jsonHistory, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                    ReportTimeUtc = fetchTime,
                    GeneratedTimestamp = DateTime.Now
                };
                _reportData_PageSpeedRepository.Add(report_PageSpeedToAdd);
            }
            else
            {
                reportPageSpeed.Score = score;
                //No need to store Loading Experience data if its category is not about Performance, and as the data will be duplicated
                reportPageSpeed.LoadingExperience = (categories.Performance != null) ? JsonConvert.SerializeObject(dataObj.LoadingExperience, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) : null;
                reportPageSpeed.OriginLoadingExperience = (categories.Performance != null) ? JsonConvert.SerializeObject(dataObj.OriginLoadingExperience, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }) : null;
                reportPageSpeed.Json = JsonConvert.SerializeObject(jsonData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                reportPageSpeed.HistoryJson = JsonConvert.SerializeObject(jsonHistory, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                reportPageSpeed.GeneratedTimestamp = DateTime.UtcNow;
                _reportData_PageSpeedRepository.Edit(reportPageSpeed);
            }
            _reportData_PageSpeedRepository.Save();
        }

        private List<LighthouseHistoryAuditsJson> MappingHistoryData(Guid clientSiteId, string category, string emulator, long? reportToUtc)
        {
            const int reportDays = 30;
            if (reportToUtc == null) reportToUtc = DateTime.UtcNow.ToUnixTimestamp();
            DateTime reportToDateTime = reportToUtc.Value.UnixTimeStampToDateTimeUtc();
            DateTime reportFromDateTime = reportToDateTime.AddDays(-reportDays);
            reportFromDateTime = new DateTime(reportFromDateTime.Year, reportFromDateTime.Month, reportFromDateTime.Day);
            long reportFromUtc = reportFromDateTime.ToUnixTimestamp();

            var retList = new List<LighthouseHistoryAuditsJson>();

            //
            //Process the data within each day (day by day), to avoid DB query timeout because the data is big
            //
            //var sbLog = new System.Text.StringBuilder();
            //sbLog.Append($"MappingHistoryData {reportFromDateTime}-{reportToDateTime}");
            bool coveredPeriod = false;
            for (int i=0; i<=reportDays; i++)
            {
                if (coveredPeriod) break;
                
                DateTime reportToDateTimeForQuery = reportToDateTime.AddDays(-i);
                DateTime reportFromDateTimeForQuery = reportToDateTimeForQuery.AddDays(-1).AddSeconds(1);
                if (reportFromDateTimeForQuery < reportFromDateTime)
                {
                    reportFromDateTimeForQuery = reportFromDateTime;
                    coveredPeriod = true;
                }
                long reportFromUtcForQuery = reportFromDateTimeForQuery.ToUnixTimestamp();
                long reportToUtcForQuery = reportToDateTimeForQuery.ToUnixTimestamp();

                var reportData = _reportData_PageSpeedRepository.FindBy(x => x.ClientSiteId == clientSiteId && x.Category == category && x.Emulator == emulator
                                 && x.ReportTimeUtc != null && x.ReportTimeUtc <= reportToUtcForQuery && x.ReportTimeUtc >= reportFromUtcForQuery
                                 && x.Score != null
                                 && x.Json != null);
                //sbLog.Append($" ->{reportToDateTimeForQuery}:{(reportData != null ? reportData.Count() + "rows" : "null")}");
                if (reportData == null || !reportData.Any()) continue;
                
                reportData = reportData
                    .OrderByDescending(x => x.ReportTimeUtc)
                    .ThenBy(x => x.GeneratedTimestamp)
                    .ToList();
                foreach (var x in reportData)
                {
                    var jsonObj = JsonConvert.DeserializeObject<LighthouseJson>(x.Json);
                    var historyObj = new LighthouseHistoryAuditsJson()
                    {
                        Utc = x.ReportTimeUtc.Value.UnixTimeStampToDateTimeUtc(),
                        Score = x.Score.Value,
                        PerformanceAudits = jsonObj.PerformanceAudits,
                        PerformanceMetrics = jsonObj.PerformanceMetrics,
                        ScoreDistribution = jsonObj.ScoreDistribution
                    };
                    //Clean-up unused history data
                    if (historyObj.PerformanceMetrics != null)
                    {
                        historyObj.PerformanceAudits.Metrics_cumulative_layout_shift.Description = null;
                        historyObj.PerformanceAudits.Metrics_first_contentful_paint.Description = null;
                        historyObj.PerformanceAudits.Metrics_interactive.Description = null;
                        historyObj.PerformanceAudits.Metrics_largest_contentful_paint.Description = null;
                        historyObj.PerformanceAudits.Metrics_speed_index.Description = null;
                        historyObj.PerformanceAudits.Metrics_total_blocking_time.Description = null;
                        historyObj.PerformanceAudits.Perf_first_cpu_idle.Description = null;
                        historyObj.PerformanceAudits.Perf_first_meaningful_paint.Description = null;
                        historyObj.PerformanceAudits.NetworkRequests.Description = null;
                        historyObj.PerformanceAudits.ScreenshotThumbnails = null;
                    }

                    retList.Add(historyObj);
                }
            }
            //_logger.Information(sbLog.ToString());

            return retList;
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _reportData_PageSpeedRepository.BatchDelete(x => x.ReportTimeUtc < retentionTimeCalc);

            //Note: To avoid timeout, you may do smt like this
            //LOOP
            //Delete from ReportData_PageSpeedInsights where Id in (
            // select top 100 Id from ReportData_PageSpeedInsights where ReportTimeUtc < retentionTimeCalc
            //)
        }

    }
}