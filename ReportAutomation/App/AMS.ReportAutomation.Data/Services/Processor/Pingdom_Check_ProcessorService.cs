using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.ReportData.Pingdom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class Pingdom_Check_ProcessorService : IPingdom_Check_ProcessorService
    {
        private readonly IReport_ReportsRepository _report_ReportsRepository;
        private readonly IPingdom_CheckRepository _pingdom_CheckRepository;
        private readonly IPingdom_CheckSummaryRepository _pingdom_CheckSummaryRepository;
        private readonly IReportData_PingdomRepository _reportData_PingdomRepository;
        private readonly IAMSLogger _logger;

        public Pingdom_Check_ProcessorService(IReport_ReportsRepository report_ReportsRepository, IPingdom_CheckRepository pingdom_CheckRepository, IPingdom_CheckSummaryRepository pingdom_CheckSummaryRepository, IReportData_PingdomRepository reportData_PingdomRepository, IAMSLogger logger)
            : base()
        {
            _report_ReportsRepository = report_ReportsRepository;
            _pingdom_CheckRepository = pingdom_CheckRepository;
            _pingdom_CheckSummaryRepository = pingdom_CheckSummaryRepository;
            _reportData_PingdomRepository = reportData_PingdomRepository;
            _logger = logger;
        }        

        public void Process()
        {
            //Get the list of active clientSites to report from the database
            var listActiveReportsFromDB = _report_ReportsRepository.FindBy(x => x.IsActive == true);

            //Get the list of pingdomCheck from the database
            var listPingdomCheckIdsFromDB = _pingdom_CheckRepository.GetAll().Select(x => x.Id).ToList();
            foreach (var report in listActiveReportsFromDB)
            {
                dynamic reportConfig = JsonConvert.DeserializeObject(report.ToolIds);
                if (reportConfig.PingdomCheckId != null && listPingdomCheckIdsFromDB.Contains((long)reportConfig.PingdomCheckId))
                {
                    long? reportTime;
                    var reportData = ParseData((long)reportConfig.PingdomCheckId, out reportTime);
                    if (reportData == null || reportTime == null)
                    {
                        //TODO: Should we log error string.Format("Failed to create report data for Pingdom Check Id {0}", client.PingdomCheckId) here or outside ?
                        throw new Exception(string.Format("Failed to create report data for Pingdom Check Id {0}", reportConfig.PingdomCheckId));                        
                    }

                    var reportData_Pingdom = _reportData_PingdomRepository.FindByCheckIdAndReportTime((long)reportConfig.PingdomCheckId, reportTime.Value);
                    if (reportData_Pingdom == null)
                    {

                        var reportDataToInsert = new ReportData_Pingdom()
                        {
                            Id = Guid.NewGuid(),
                            CheckId = reportConfig.PingdomCheckId,
                            ReportTimeUtc = reportTime,
                            Json = JsonConvert.SerializeObject(reportData),
                            GeneratedTimestamp = DateTime.Now
                        };
                        _reportData_PingdomRepository.Add(reportDataToInsert);
                    }
                    else
                    {
                        reportData_Pingdom.Json = JsonConvert.SerializeObject(reportData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                        reportData_Pingdom.GeneratedTimestamp = DateTime.Now;
                        _reportData_PingdomRepository.Edit(reportData_Pingdom);
                    }
                }
            }
            _reportData_PingdomRepository.Save();
        }

        protected UptimeAndResponse ParseData(long pingdomCheckId, out long? reportTime)
        {
            reportTime = null;

            var todayUTC = DateTime.UtcNow;
            var ago7Days = todayUTC.Subtract(TimeSpan.FromDays(7));
            //Starting from 0:00
            ago7Days = new DateTime(ago7Days.Year, ago7Days.Month, ago7Days.Day);
            var ago30Days = todayUTC.Subtract(TimeSpan.FromDays(30));
            //Starting from 0:00
            ago30Days = new DateTime(ago30Days.Year, ago30Days.Month, ago30Days.Day);

            //Get all Uptime (SummaryPerformance) data of pingdomCheckId in the last 30 days with Resolution = Hours from the database
            var rawDataFromDB = _pingdom_CheckSummaryRepository
                .FindWhere(x => x.CheckId == pingdomCheckId && x.Resolution.HasValue && x.Resolution.Value == (int)ResolutionType.Hour)
                .OrderByDescending(x => x.DataTimeUtc)
                .FirstOrDefault();
            if (rawDataFromDB != null)
            {
                reportTime = rawDataFromDB.DataTimeUtc;

                //Reference: https://docs.pingdom.com/api/#tag/Summary.performance/paths/~1summary.performance~1{checkid}/get
                var rawPerfDataFromDB = rawDataFromDB.Json == null ? null : JsonConvert.DeserializeObject<IEnumerable<dynamic>>(rawDataFromDB.Json);
                if (rawPerfDataFromDB != null)
                {
                    var returnedData = new UptimeAndResponse();

                    var list7DaysAgo = rawPerfDataFromDB.Where(x => ((long)x.StartTime).UnixTimeStampToDateTimeUtc() >= ago7Days).ToList();                    
                    if (list7DaysAgo != null && list7DaysAgo.Any())
                        returnedData.Summary.Add(new KeyValuePair<string, SummaryUptime>("Last 7 days", new SummaryUptime()
                        {
                            UptimePercentage = 100 * (decimal)list7DaysAgo.Sum(x => (long)x.UpTime) / list7DaysAgo.Sum(x => (long)x.UpTime + (long)x.DownTime + (long)x.Unmonitored),
                            Outages = list7DaysAgo.Where(x => (long)x.DownTime > 0).Count(),
                            DownTimeSec = list7DaysAgo.Sum(x => (long)x.DownTime),
                            AvgResponseMillisec = (long)list7DaysAgo.Average(x => (long)x.AvgResponse)
                        }));

                    var list30DaysAgo = rawPerfDataFromDB.Where(x => ((long)x.StartTime).UnixTimeStampToDateTimeUtc() >= ago30Days).ToList();
                    if (list30DaysAgo != null && list30DaysAgo.Any())
                        returnedData.Summary.Add(new KeyValuePair<string, SummaryUptime>("Last 30 days", new SummaryUptime()
                        {
                            UptimePercentage = 100 * (decimal)list30DaysAgo.Sum(x => (long)x.UpTime) / list30DaysAgo.Sum(x => (long)x.UpTime + (long)x.DownTime + (long)x.Unmonitored),
                            Outages = list30DaysAgo.Where(x => (long)x.DownTime > 0).Count(),
                            DownTimeSec = list30DaysAgo.Sum(x => (long)x.DownTime),
                            AvgResponseMillisec = (long)list30DaysAgo.Average(x => (long)x.AvgResponse)
                        }));

                    returnedData.Performance = rawPerfDataFromDB.Select(x => new PerformanceUptime()
                    {
                        StartTimeUtc = ((long)x.StartTime).UnixTimeStampToDateTimeUtc(),
                        AvgResponseMillisec = (long)x.AvgResponse,
                        UpTimeSec = (long)x.UpTime,
                        DownTimeSec = (long)x.DownTime,
                        UnmonitoredSec = (long)x.Unmonitored
                    }).ToList();

                    return returnedData;
                }
            }

            return null;
        }

        public UptimeAndResponse GetPingdomReportDataByCheckId(long checkId, long reportToDateTime)
        {
            var reportData = _reportData_PingdomRepository
                .FindWhere(x => x.CheckId == checkId && x.ReportTimeUtc <= reportToDateTime)
                .OrderByDescending(x => x.ReportTimeUtc)
                .ThenBy(x => x.GeneratedTimestamp)
                .FirstOrDefault();
            if (reportData != null && reportData.Json != null)
            {
                var retData = JsonConvert.DeserializeObject<UptimeAndResponse>(reportData.Json);
                if (retData != null && reportData.ReportTimeUtc != null) retData.ConsolidatedTimeUtc = reportData.ReportTimeUtc.Value.UnixTimeStampToDateTimeUtc();
                return retData;
            }

            return null;
        }

        public Dictionary<long, UptimeAndResponse> GetPingdomReportDataByCheckIds(List<long> checkIds, long reportToDateTime)
        {
            var retDic = new Dictionary<long, UptimeAndResponse>();

            //Note: We want to avoid using foreach but make a single SQL query to DB to select all data records (the latest one for each checkId), so not to:
            //foreach (var checkId in checkIds)
            //{
            //    retDic.Add(checkId, GetPingdomReportDataByCheckId(checkId, reportToDateTime));
            //}
            //, but do:
            var reportData = _reportData_PingdomRepository
                .FindWhere(x => x.CheckId != null && checkIds.Contains((long)x.CheckId) && x.ReportTimeUtc <= reportToDateTime)                
                .GroupBy(x => x.CheckId, (key, g) => g.OrderByDescending(x => x.ReportTimeUtc).ThenBy(x => x.GeneratedTimestamp).FirstOrDefault())
                .ToList();
            if (reportData != null)
            {
                foreach(var data in reportData)
                {
                    if (data.Json != null)
                    {
                        var retData = JsonConvert.DeserializeObject<UptimeAndResponse>(data.Json);
                        if (retData != null && data.ReportTimeUtc != null) retData.ConsolidatedTimeUtc = data.ReportTimeUtc.Value.UnixTimeStampToDateTimeUtc();
                        retDic.Add((long)data.CheckId, retData);
                    }                    
                }
            }
            foreach(var checkId in checkIds)
            {
                if (!retDic.ContainsKey(checkId)) retDic.Add(checkId, null);
            }

            return retDic;            
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _reportData_PingdomRepository.BatchDelete(x => x.ReportTimeUtc < retentionTimeCalc);
        }
    }
}
