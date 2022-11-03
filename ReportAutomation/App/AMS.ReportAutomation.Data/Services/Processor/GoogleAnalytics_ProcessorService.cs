using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.ReportData.GoogleAnalytic;
using AMS.GoogleAnalytic;
using Google.Apis.AnalyticsReporting.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class GoogleAnalytics_ProcessorService : IGoogleAnalytics_ProcessorService
    {
        private readonly IAMSLogger _logger;

        private readonly IReport_ReportsRepository _report_ReportsRepository;

        private readonly IGoogleAnalytics_Repository _googleAnalyticsRepository;

        private readonly IReportData_GoogleAnalyticRepository _reportData_GoogleAnalyticRepository;

        private readonly IReport_ClientSitesRepository _report_ClientSitesRepository;

        public GoogleAnalytics_ProcessorService(IAMSLogger logger, IReport_ReportsRepository report_ReportsRepository, IGoogleAnalytics_Repository googleAnalyticsRepository, IReportData_GoogleAnalyticRepository reportData_GoogleAnalyticRepository, IReport_ClientSitesRepository report_ClientSitesRepository) : base()
        {
            _logger = logger;
            _report_ReportsRepository = report_ReportsRepository;
            _googleAnalyticsRepository = googleAnalyticsRepository;
            _reportData_GoogleAnalyticRepository = reportData_GoogleAnalyticRepository;
            _report_ClientSitesRepository = report_ClientSitesRepository;
        }

        public void Process()
        {
            //Get the list of active clientSites to report from the database
            var listActiveReportsFromDB = _report_ReportsRepository.FindBy(x => x.IsActive == true);

            foreach (var report in listActiveReportsFromDB)
            {
                dynamic reportConfig = JsonConvert.DeserializeObject(report.ToolIds);

                if ((reportConfig.GoogleAnalyticEnabled != null && ((string)reportConfig.GoogleAnalyticEnabled).ToLower() != "false" && ((string)reportConfig.GoogleAnalyticEnabled).ToLower() != "0"))
                {
                    ProcessSaveReportData(report.ClientSiteId.Value);
                }
            }
        }

        protected void ProcessSaveReportData(Guid clientSiteId)
        {
            var utcNow = DateTime.UtcNow;
            var endOfYesterdayUtc = (new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0)).AddSeconds(-1);
            //We don't need to process all records, particularly that old data will still be correct as the analytics was already done for the past days, and also the database will grow very big so the number of crawled records will be big
            //Even if the processor does not run often (let say it is scheduled to run weekly) => it is good enough to process the data for the last 7 days, particularly each data record contained data of multiple days in 1 month.
            const int maxDaysToProcess = 7;
            var maxOldUnixDayToProcess = endOfYesterdayUtc.AddDays(-maxDaysToProcess).ToUnixTimestamp();

            var rawDataRecords = _googleAnalyticsRepository.FindBy(x => x.ClientSiteId == clientSiteId && x.DataTimeUtc != null && x.DataTimeUtc >= maxOldUnixDayToProcess);
            if (rawDataRecords != null)
            {
                //Group data records by DataTimeUtc
                var rawDataList = rawDataRecords.GroupBy(x => x.DataTimeUtc).ToList();

                //Process data for each DataTimeUtc
                foreach (var dataList in rawDataList)
                {
                    if (!dataList.Any()) continue;

                    var dataTimeUtc = dataList.Key;
                    var returnData = new GoogleAnalyticData();

                    foreach (var rawData in dataList)
                    {
                        var reportDataRows = JsonConvert.DeserializeObject<List<ReportRow>>(rawData.Json);

                        switch (rawData.DatasetType)
                        {
                            case DatasetType.AudienceOverview:
                                foreach (var row in reportDataRows)
                                {
                                    var metric = row.Metrics[0];
                                    var rptDate = DateTime.ParseExact(row.Dimensions[0], "yyyyMMdd", CultureInfo.InvariantCulture);
                                    var key = rptDate.ToString("yyyy-MM-dd");
                                    if (!returnData.AudienceOverviews.ContainsKey(key))
                                    {
                                        returnData.AudienceOverviews.Add(key, new List<AudienceOverview>());
                                    }
                                    returnData.AudienceOverviews[key].Add(new AudienceOverview
                                    {
                                        Date = rptDate,
                                        Users = double.Parse(metric.Values[0]),
                                        NewUsers = double.Parse(metric.Values[1]),
                                        Sessions = double.Parse(metric.Values[2]),
                                        SessionsPerUser = double.Parse(metric.Values[3]),
                                        PageViews = double.Parse(metric.Values[4]),
                                        PageSession = double.Parse(metric.Values[5]),
                                        AvgSessionDuration = double.Parse(metric.Values[6]),
                                        BounceRate = double.Parse(metric.Values[7]),
                                    });
                                }
                                break;

                            //case DatasetType.Users1Day:
                            //case DatasetType.Users1Week:
                            //case DatasetType.Users2Weeks:
                            //case DatasetType.Users4Weeks:
                            //    var listActiveUser = new List<DateUserPair>();
                            //    foreach (var row in reportDataRows)
                            //    {
                            //        var metric = row.Metrics[0];
                            //        var rptDate = DateTime.ParseExact(row.Dimensions[0], "yyyyMMdd", CultureInfo.InvariantCulture);
                            //        listActiveUser.Add(new DateUserPair
                            //        {
                            //            Date = rptDate,
                            //            User = int.Parse(metric.Values[0])
                            //        });
                            //    }
                            //    if (!returnData.ActiveUsers.ContainsKey(rawData.DatasetType))
                            //        returnData.ActiveUsers.Add(rawData.DatasetType, listActiveUser);
                            //    else
                            //        returnData.ActiveUsers[rawData.DatasetType] = listActiveUser;
                            //    break;

                            case DatasetType.TrafficByDevice:
                            case DatasetType.TrafficByBrowser:
                            case DatasetType.TrafficByOS:
                            case DatasetType.ChannelGrouping:
                                var dicTraffic = new Dictionary<string, List<TrafficItem>>();
                                foreach (var row in reportDataRows)
                                {
                                    var metric = row.Metrics[0];
                                    var key = row.Dimensions[0];
                                    var rptDate = DateTime.ParseExact(row.Dimensions[1], "yyyyMMdd", CultureInfo.InvariantCulture);

                                    if (!dicTraffic.ContainsKey(key))
                                    {
                                        dicTraffic.Add(key, new List<TrafficItem>());
                                    }
                                    dicTraffic[key].Add(new TrafficItem
                                    {
                                        Date = rptDate,
                                        Users = int.Parse(metric.Values[0]),
                                        SessionsPerUser = rawData.DatasetType == DatasetType.TrafficByDevice || rawData.DatasetType == DatasetType.TrafficByBrowser || rawData.DatasetType == DatasetType.TrafficByOS ? double.Parse(metric.Values[1]) : (double?)null,
                                        BounceRate = rawData.DatasetType == DatasetType.ChannelGrouping ? double.Parse(metric.Values[1]) : (double?)null
                                    });
                                }
                                if (rawData.DatasetType == DatasetType.TrafficByDevice) returnData.TrafficByDevice = dicTraffic;
                                if (rawData.DatasetType == DatasetType.TrafficByBrowser) returnData.TrafficByBrowser = dicTraffic;
                                if (rawData.DatasetType == DatasetType.TrafficByOS) returnData.TrafficByOS = dicTraffic;
                                if (rawData.DatasetType == DatasetType.ChannelGrouping) returnData.ChannelGrouping = dicTraffic;
                                break;
                        }
                    }
                    
                    //Save processed data into DB
                    var reportData_GoogleAnalytic = new ReportData_GoogleAnalytic
                    {
                        Id = Guid.NewGuid(),
                        ClientSiteId = clientSiteId,
                        Json = JsonConvert.SerializeObject(returnData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        ReportTimeUtc = dataTimeUtc,
                        GeneratedTimestamp = DateTime.Now
                    };
                    var existedData = _reportData_GoogleAnalyticRepository
                        .FindFirstOrDefault(x => x.ClientSiteId == reportData_GoogleAnalytic.ClientSiteId && x.ReportTimeUtc == reportData_GoogleAnalytic.ReportTimeUtc);
                    if (existedData != null)
                    {
                        existedData.Json = reportData_GoogleAnalytic.Json;
                        existedData.GeneratedTimestamp = reportData_GoogleAnalytic.GeneratedTimestamp;
                        _reportData_GoogleAnalyticRepository.Edit(existedData);
                    }
                    else
                    {
                        _reportData_GoogleAnalyticRepository.Add(reportData_GoogleAnalytic);
                    }
                    _reportData_GoogleAnalyticRepository.Save();
                }
            }
        }

        public GoogleAnalyticData GetGoogleAnalyticReportDataByClientId(Guid clientId, long reportToDateTime)
        {
            var reportData = _reportData_GoogleAnalyticRepository
                .FindWhere(x => x.ClientSiteId == clientId && x.ReportTimeUtc <= reportToDateTime)
                .OrderByDescending(x => x.ReportTimeUtc)
                .ThenBy(x => x.GeneratedTimestamp)
                .FirstOrDefault();
            if (reportData != null && reportData.Json != null)
            {
                var retData = JsonConvert.DeserializeObject<GoogleAnalyticData>(reportData.Json);
                if (retData != null && reportData.ReportTimeUtc != null) retData.ConsolidatedTimeUtc = reportData.ReportTimeUtc.Value.UnixTimeStampToDateTimeUtc();

                return retData;
            }
            return null;
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _reportData_GoogleAnalyticRepository.BatchDelete(x => x.ReportTimeUtc < retentionTimeCalc);
        }
    }
}