using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Common.Mail;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.ReportData.Pingdom;
using AMS.ReportAutomation.ReportData.ScreamingFrog;
using AMS.ReportAutomation.ReportData.PageSpeed;
using System.Text;

namespace AMS.ReportAutomation.ReportSubscription
{
    public class ReportSubscriptionJob : IJob
    {
        private IAMSLogger _logger;
        private EmailService _emailService;

        private readonly IReport_ReportsRepository _report_ReportsRepository;
        private readonly IPingdom_Check_ProcessorService _pingdom_Check_ProcessorService;
        private readonly IJira_ServiceDesk_ProcessorService _jiraServiceDesk_ProcessorService;
        private readonly IReportData_PageSpeedRepository _report_PageSpeedRepository;
        private readonly IReportData_ScreamingFrogRepository _report_ScreamingFrogRepository;

        public ReportSubscriptionJob(IAMSLogger logger,
            IReport_ReportsRepository report_ReportsRepository,
            IPingdom_Check_ProcessorService pingdom_Check_ProcessorService,
            IJira_ServiceDesk_ProcessorService jiraServiceDesk_ProcessorService,
            IReportData_PageSpeedRepository report_PageSpeedRepository,
            IReportData_ScreamingFrogRepository report_ScreamingFrogRepository)
        {
            _logger = logger;
            _emailService = new EmailService();

            _report_ReportsRepository = report_ReportsRepository;
            _pingdom_Check_ProcessorService = pingdom_Check_ProcessorService;
            _jiraServiceDesk_ProcessorService = jiraServiceDesk_ProcessorService;
            _report_PageSpeedRepository = report_PageSpeedRepository;
            _report_ScreamingFrogRepository = report_ScreamingFrogRepository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                _logger.Information("Start to send subscription.");

                JobDataMap dataMap = context.JobDetail.JobDataMap;

                var reportId = dataMap.GetString("ReportId");

                var receiverEmails = dataMap.GetString("ReceiverEmails").Split(',').ToList();

                string subject = MailSettings.MailSubscriptionSubject + " " + dataMap.GetString("ReportName");
                string subscriptionMessage = MailSettings.MailSubscriptionBody + "<br/>" + MailSettings.MailSubscriptionReportUrl + reportId;
                //Pass into the number of days and ToDate; based on the current date/time and the current subscribed schedule.
                if (dataMap["ReportTo"] != null)
                {
                    subscriptionMessage += $"&reportTo={dataMap.GetString("ReportTo")}&reportDays={dataMap.GetInt("ReportDays")}";
                }
                try
                {

                    string subscriptionContentTemplate = @"
{0}
{1}
{2}
";
                    //TODO: Pass into the number of days and ToDate; based on the current date/time and the current subscribed schedule.
                    var reportSummary = GetReportSummary(new Guid(reportId));
                    if (reportSummary != null)
                    {
                        string strScores = string.Empty;
                        var sbScores = new StringBuilder();
                        if (reportSummary.Overview != null && reportSummary.Overview.Stats_Pingdom != null && reportSummary.Overview.Stats_Pingdom.Score > 0)
                            sbScores.AppendFormat("<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><b>Uptime: </b>{0}</div>", Math.Round(reportSummary.Overview.Stats_Pingdom.Score * 100, 2));
                        if (reportSummary.Overview != null && reportSummary.Overview.Stats_Performance != null && reportSummary.Overview.Stats_Performance.Score > 0)
                            sbScores.AppendFormat("<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><b>Performance: </b>{0}</div>", Math.Round(reportSummary.Overview.Stats_Performance.Score * 100, 2));
                        if (reportSummary.Overview != null && reportSummary.Overview.Stats_BestPractices != null && reportSummary.Overview.Stats_BestPractices.Score > 0)
                            sbScores.AppendFormat("<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><b>Best Practices: </b>{0}</div>", Math.Round(reportSummary.Overview.Stats_BestPractices.Score * 100, 2));
                        if (reportSummary.Overview != null && reportSummary.Overview.Stats_Accessibility != null && reportSummary.Overview.Stats_Accessibility.Score > 0)
                            sbScores.AppendFormat("<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><b>Accessibility: </b>{0}</div>", Math.Round(reportSummary.Overview.Stats_Accessibility.Score * 100, 2));
                        if (reportSummary.Overview != null && reportSummary.Overview.Stats_SEO != null && reportSummary.Overview.Stats_SEO.Score > 0)
                            sbScores.AppendFormat("<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><b>SEO: </b>{0}</div>", Math.Round(reportSummary.Overview.Stats_SEO.Score * 100, 2));
                        if (sbScores.Length > 0)
                        {
                            strScores = string.Format(@"
                                <h1>Score</h1>
                                <div>
                                {0}
                                </div>
                                <br />
                                ", sbScores.ToString());
                        }

                        string strUptime = string.Empty;
                        var sbUptime = new StringBuilder();
                        if (reportSummary.Pingdom != null && reportSummary.Pingdom.Summary != null)
                        {
                            foreach(var data in reportSummary.Pingdom.Summary)
                            {
                                sbUptime.Append($"<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><h3>{data.Key}</h3><b>Uptime: </b>{Math.Round(data.Value.UptimePercentage, 2)} %<br /><b>Outages: </b><font color =\"red\">{data.Value.Outages}</font></div>");
                            }
                        }
                        if (sbUptime.Length > 0 || reportSummary.CountIncidentTickets >= 0)
                        {
                            strUptime = string.Format(@"
                                <h1>Uptime performance</h1>
                                <div>
                                {0}
                                </div>
                                {1}
                                ", sbUptime.ToString(), reportSummary.CountIncidentTickets >= 0 ? $"<br /><h3>Incident tickets: {reportSummary.CountIncidentTickets}</h3>" : string.Empty);
                        }

                        string strScreamingFrog = string.Empty;
                        var sbScreamingFrog = new StringBuilder();
                        if (reportSummary.ScreamingFrog != null && reportSummary.ScreamingFrog.Indexability != null)
                            sbScreamingFrog.Append($"<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><b>Indexability</b><br /><b>Indexable:</b>{reportSummary.ScreamingFrog.Indexability.CountIndexable} ({Math.Round((double)reportSummary.ScreamingFrog.Indexability.CountIndexable / (reportSummary.ScreamingFrog.Indexability.CountIndexable + reportSummary.ScreamingFrog.Indexability.CountNonIndexable), 2)} %)<br /><b>Non-indexable:</b>{reportSummary.ScreamingFrog.Indexability.CountNonIndexable} ({Math.Round((double)reportSummary.ScreamingFrog.Indexability.CountNonIndexable / (reportSummary.ScreamingFrog.Indexability.CountIndexable + reportSummary.ScreamingFrog.Indexability.CountNonIndexable), 2)} %)</div>");
                        if (reportSummary.ScreamingFrog != null && reportSummary.ScreamingFrog.HttpStatusCodes != null)
                            sbScreamingFrog.Append($"<div style=\"border:1px solid;float:left;padding:2px;margin-right:2px;\"><b>HTTP Status Codes</b><br /><b>2xx (success):</b>{reportSummary.ScreamingFrog.HttpStatusCodes.CountSuccess}<br /><b>3xx (redirect):</b>{reportSummary.ScreamingFrog.HttpStatusCodes.CountRedirect}<br /><b>Errors:</b>{reportSummary.ScreamingFrog.HttpStatusCodes.CountNotOk}</div>");
                        if (sbScreamingFrog.Length > 0)
                        {
                            strScreamingFrog = string.Format(@"
                                <h1>Spider</h1>
                                <div>
                                {0}
                                </div>
                                <br />
                                ", sbScreamingFrog.ToString());
                        }

                        subscriptionMessage += string.Format(subscriptionContentTemplate, strScores, strUptime, strScreamingFrog);
                    }

                    _emailService.Send(subject,subscriptionMessage, receiverEmails);
                }
                catch (Exception ex)
                {
                    string errMessage = $"{ex.Message} {(ex.InnerException != null ? "InnerException: " + ex.InnerException.Message : string.Empty)} StackTrace: {ex.StackTrace}";
                    _logger.Error(errMessage);
                    try
                    {
                        _emailService.SendNotification(errMessage);
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Could not send email to notify exception occurred. Error: {e.Message}");
                    }
                }

                _logger.Information("Send email subscription job completed.");
            }, context.CancellationToken);
        }

        private ReportDataModel GetReportSummary(Guid reportId, string reportTo = null, uint reportDays = 30)
        {
            ReportDataModel reportModel = null;

            var report = _report_ReportsRepository.FindFirstOrDefault(x => x.Id == reportId);
            if (report != null)
            {
                var reportToDate = string.IsNullOrWhiteSpace(reportTo) ? new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day) : DateTime.Parse(reportTo);
                var reportFromDateTime = reportToDate.AddDays(-reportDays);
                var reportToDateTimeUnix = reportToDate.AddDays(1).AddSeconds(-1).ToUnixTimestamp();
                var reportFromDateTimeUnix = reportFromDateTime.ToUnixTimestamp();

                reportModel = new ReportDataModel();

                List<ReportScore> scoresPingdom = null;
                uint weightScorePingdom = 30;
                List<ReportScore> scoresPerformance = null;
                uint weightScorePerformance = 25;
                List<ReportScore> scoresBestPractices = null;
                uint weightScoreBestPractices = 20;
                List<ReportScore> scoresAccessibility = null;
                uint weightScoreAccessibility = 10;
                List<ReportScore> scoresSEO = null;
                uint weightScoreSEO = 15;

                dynamic reportConfig = JsonConvert.DeserializeObject(report.ToolIds);
                if (reportConfig.PingdomCheckId != null)
                {
                    var pingdomReportData = _pingdom_Check_ProcessorService.GetPingdomReportDataByCheckId((long)reportConfig.PingdomCheckId, reportToDateTimeUnix);
                    if (pingdomReportData != null)
                    {
                        pingdomReportData.Performance = pingdomReportData.Performance.Where(x => x.StartTimeUtc >= reportFromDateTime).ToList();

                        reportModel.Pingdom = pingdomReportData;

                        //Score serie
                        var groupedPingdomPerformanceByDate = pingdomReportData.Performance.GroupBy(x => x.StartDateUtc).ToList();
                        if (groupedPingdomPerformanceByDate != null && groupedPingdomPerformanceByDate.Any())
                        {
                            scoresPingdom = new List<ReportScore>();
                            foreach (var pingdomPerfByDate in groupedPingdomPerformanceByDate)
                            {
                                var score = new ReportScore()
                                {
                                    Score = (double)pingdomPerfByDate.Sum(x => x.UpTimeSec) / pingdomPerfByDate.Sum(x => x.DownTimeSec + x.UpTimeSec + x.UnmonitoredSec),
                                    UnixTimestamp = pingdomPerfByDate.Key.ToUnixTimestamp()
                                };
                                scoresPingdom.Add(score);
                            }
                        }
                    }
                }
                if (reportConfig.ClientJiraServiceDeskId != null)
                {
                    var jiraServiceDeskReportData = _jiraServiceDesk_ProcessorService.GetIncidentDataByClientJiraServiceDeskId(new Guid(reportConfig.ClientJiraServiceDeskId.ToString()), reportToDateTimeUnix);
                    reportModel.CountIncidentTickets = (jiraServiceDeskReportData != null && jiraServiceDeskReportData.Tickets != null) ? jiraServiceDeskReportData.Tickets.Count() : 0;
                }
                if ((reportConfig.PSIPerformanceEnabled != null && ((string)reportConfig.PSIPerformanceEnabled).ToLower() != "false" && ((string)reportConfig.PSIPerformanceEnabled).ToLower() != "0")
                    || (reportConfig.PSIAccessibilityEnabled != null && ((string)reportConfig.PSIAccessibilityEnabled).ToLower() != "false" && ((string)reportConfig.PSIAccessibilityEnabled).ToLower() != "0")
                    || (reportConfig.PSISeoEnabled != null && ((string)reportConfig.PSISeoEnabled).ToLower() != "false" && ((string)reportConfig.PSISeoEnabled).ToLower() != "0")
                    || (reportConfig.PSIPwaEnabled != null && ((string)reportConfig.PSIPwaEnabled).ToLower() != "false" && ((string)reportConfig.PSIPwaEnabled).ToLower() != "0")
                    )
                {
                    var reportData = _report_PageSpeedRepository.FindWhere(x => x.ClientSiteId == report.ClientSiteId
                            && x.Score != null
                            && x.ReportTimeUtc != null && x.ReportTimeUtc <= reportToDateTimeUnix && x.ReportTimeUtc > reportFromDateTimeUnix)
                        .OrderByDescending(x => x.ReportTimeUtc)
                        .ThenBy(x => x.GeneratedTimestamp)
                        .ToList();
                    if (reportData != null && reportData.Any())
                    {
                        if (reportConfig.PSIPerformanceEnabled != null && ((string)reportConfig.PSIPerformanceEnabled).ToLower() != "false" && ((string)reportConfig.PSIPerformanceEnabled).ToLower() != "0")
                        {
                            var reportDataLatestDesktop = reportData
                                .Where(x => x.Category == Constants.Category_Performance && x.Emulator == Constants.Desktop)
                                .OrderByDescending(x => x.ReportTimeUtc)
                                .FirstOrDefault();

                            if (reportDataLatestDesktop != null && reportDataLatestDesktop.Score != null && reportDataLatestDesktop.ReportTimeUtc != null)
                            {
                                //Score serie
                                scoresPerformance = new List<ReportScore>();
                                scoresPerformance.Add(
                                        //TODO: If mobile score > desktop score, take that instead?
                                        new ReportScore()
                                        {
                                            Score = reportDataLatestDesktop.Score.Value,
                                            UnixTimestamp = reportDataLatestDesktop.ReportTimeUtc.Value
                                        }
                                    );
                            }

                            reportDataLatestDesktop = reportData
                                .Where(x => x.Category == Constants.Category_BestPractices && x.Emulator == Constants.Desktop)
                                .OrderByDescending(x => x.ReportTimeUtc)
                                .FirstOrDefault();

                            if (reportDataLatestDesktop != null && reportDataLatestDesktop.Score != null && reportDataLatestDesktop.ReportTimeUtc != null)
                            {
                                //Score serie
                                scoresBestPractices = new List<ReportScore>();
                                scoresBestPractices.Add(
                                        //TODO: If mobile score > desktop score, take that instead?
                                        new ReportScore()
                                        {
                                            Score = reportDataLatestDesktop.Score.Value,
                                            UnixTimestamp = reportDataLatestDesktop.ReportTimeUtc.Value
                                        }
                                    );
                            }
                        }
                        if (reportConfig.PSIAccessibilityEnabled != null && ((string)reportConfig.PSIAccessibilityEnabled).ToLower() != "false" && ((string)reportConfig.PSIAccessibilityEnabled).ToLower() != "0")
                        {
                            var reportDataLatestDesktop = reportData
                                .Where(x => x.Category == Constants.Category_Accessibility && x.Emulator == Constants.Desktop)
                                .OrderByDescending(x => x.ReportTimeUtc)
                                .FirstOrDefault();

                            if (reportDataLatestDesktop != null && reportDataLatestDesktop.Score != null && reportDataLatestDesktop.ReportTimeUtc != null)
                            {
                                //Score serie
                                scoresAccessibility = new List<ReportScore>();
                                scoresAccessibility.Add(
                                        //TODO: If mobile score > desktop score, take that instead?
                                        new ReportScore()
                                        {
                                            Score = reportDataLatestDesktop.Score.Value,
                                            UnixTimestamp = reportDataLatestDesktop.ReportTimeUtc.Value
                                        }
                                    );
                            }
                        }
                        if (reportConfig.PSISeoEnabled != null && ((string)reportConfig.PSISeoEnabled).ToLower() != "false" && ((string)reportConfig.PSISeoEnabled).ToLower() != "0")
                        {
                            var reportDataLatestDesktop = reportData
                                .Where(x => x.Category == Constants.Category_SEO && x.Emulator == Constants.Desktop)
                                .OrderByDescending(x => x.ReportTimeUtc)
                                .FirstOrDefault();

                            if (reportDataLatestDesktop != null && reportDataLatestDesktop.Score != null && reportDataLatestDesktop.ReportTimeUtc != null)
                            {
                                //Score serie
                                scoresSEO = new List<ReportScore>();
                                scoresSEO.Add(
                                        //TODO: If mobile score > desktop score, take that instead?
                                        new ReportScore()
                                        {
                                            Score = reportDataLatestDesktop.Score.Value,
                                            UnixTimestamp = reportDataLatestDesktop.ReportTimeUtc.Value
                                        }
                                    );
                            }
                        }
                        //if (reportConfig.PSIPwaEnabled != null && ((string)reportConfig.PSIPwaEnabled).ToLower() != "false" && ((string)reportConfig.PSIPwaEnabled).ToLower() != "0")
                        //{

                        //    var reportDataLatestDesktop = reportData
                        //        .Where(x => x.Category == Constants.Category_PWA && x.Emulator == Constants.Desktop)
                        //        .OrderByDescending(x => x.ReportTimeUtc)
                        //        .FirstOrDefault();

                        //    if (reportDataLatestDesktop != null && reportDataLatestDesktop.Score != null && reportDataLatestDesktop.ReportTimeUtc != null)
                        //    {
                        //        //Score serie
                        //        scoresPWA = new List<ReportScore>();
                        //        scoresPWA.Add(
                        //                //TODO: If mobile score > desktop score, take that instead?
                        //                new ReportScore()
                        //                {
                        //                    Score = reportDataLatestDesktop.Score.Value,
                        //                    UnixTimestamp = reportDataLatestDesktop.ReportTimeUtc.Value
                        //                }
                        //            );
                        //    }
                        //}
                    }
                }

                //Report - Screaming Frog
                if (reportConfig.ScreamingFrogEnabled != null &&
                    ((string)reportConfig.ScreamingFrogEnabled).ToLower() != "false" &&
                    ((string)reportConfig.ScreamingFrogEnabled).ToLower() != "0")
                {
                    var reportData = _report_ScreamingFrogRepository.FindWhere(x => x.ClientSiteId == report.ClientSiteId
                            && x.ReportTimeUtc != null && x.ReportTimeUtc <= reportToDateTimeUnix && x.ReportTimeUtc > reportFromDateTimeUnix)
                        .OrderByDescending(x => x.ReportTimeUtc)
                        .ThenBy(x => x.GeneratedTimestamp)
                        .FirstOrDefault();
                    if (reportData != null && reportData.Json != null)
                    {
                        reportModel.ScreamingFrog = JsonConvert.DeserializeObject<ScreamingFrogData>(reportData.Json);
                        if (reportData.ReportTimeUtc != null) reportModel.ScreamingFrog.ConsolidatedTimeUtc = reportData.ReportTimeUtc.Value.UnixTimeStampToDateTimeUtc();
                    }
                }

                //
                //Latest Total Health Score
                //
                scoresPingdom = scoresPingdom?.OrderByDescending(x => x.UnixTimestamp).ToList();
                scoresPerformance = scoresPerformance?.OrderByDescending(x => x.UnixTimestamp).ToList();
                scoresBestPractices = scoresBestPractices?.OrderByDescending(x => x.UnixTimestamp).ToList();
                scoresAccessibility = scoresAccessibility?.OrderByDescending(x => x.UnixTimestamp).ToList();
                scoresSEO = scoresSEO?.OrderByDescending(x => x.UnixTimestamp).ToList();
                double totalHealthScore = 0;
                uint totalHealthScoreWeight = 0;
                long totalHealthScoreUnixTimestamp = 0;
                reportModel.Overview = new ReportOverview();
                if (scoresPingdom != null && scoresPingdom.Any())
                {
                    var scoreData = new ReportScore()
                    {
                        //Score = scoresPingdom.Average(x => x.Score),
                        Score = scoresPingdom.First().Score,
                        UnixTimestamp = scoresPingdom.Max(x => x.UnixTimestamp)
                    };
                    reportModel.Overview.Stats_Pingdom = scoreData;
                    totalHealthScore += scoreData.Score * weightScorePingdom;
                    totalHealthScoreWeight += weightScorePingdom;
                    totalHealthScoreUnixTimestamp = Math.Max(totalHealthScoreUnixTimestamp, scoreData.UnixTimestamp);
                }
                if (scoresPerformance != null && scoresPerformance.Any())
                {
                    var scoreData = scoresPerformance.First();
                    reportModel.Overview.Stats_Performance = scoreData;
                    totalHealthScore += scoreData.Score * weightScorePerformance;
                    totalHealthScoreWeight += weightScorePerformance;
                    totalHealthScoreUnixTimestamp = Math.Max(totalHealthScoreUnixTimestamp, scoreData.UnixTimestamp);
                }
                if (scoresBestPractices != null && scoresBestPractices.Any())
                {
                    var scoreData = scoresBestPractices.First();
                    reportModel.Overview.Stats_BestPractices = scoreData;
                    totalHealthScore += scoreData.Score * weightScoreBestPractices;
                    totalHealthScoreWeight += weightScoreBestPractices;
                    totalHealthScoreUnixTimestamp = Math.Max(totalHealthScoreUnixTimestamp, scoreData.UnixTimestamp);
                }
                if (scoresAccessibility != null && scoresAccessibility.Any())
                {
                    var scoreData = scoresAccessibility.First();
                    reportModel.Overview.Stats_Accessibility = scoreData;
                    totalHealthScore += scoreData.Score * weightScoreAccessibility;
                    totalHealthScoreWeight += weightScoreAccessibility;
                    totalHealthScoreUnixTimestamp = Math.Max(totalHealthScoreUnixTimestamp, scoreData.UnixTimestamp);
                }
                if (scoresSEO != null && scoresSEO.Any())
                {
                    var scoreData = scoresSEO.First();
                    reportModel.Overview.Stats_SEO = scoreData;
                    totalHealthScore += scoreData.Score * weightScoreSEO;
                    totalHealthScoreWeight += weightScoreSEO;
                    totalHealthScoreUnixTimestamp = Math.Max(totalHealthScoreUnixTimestamp, scoreData.UnixTimestamp);
                }
            }            

            return reportModel;
        }

    }

    internal class ReportDataModel
    {
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public DateTime ReportFromDateTime { get; set; }
        public DateTime ReportToDateTime { get; set; }
        public ReportOverview Overview { get; set; }
        public UptimeAndResponse Pingdom { get; set; }
        public int CountIncidentTickets = -1;
        public ScreamingFrogData ScreamingFrog { get; set; }
    }
    internal class ReportOverview
    {
        public ReportScore Stats_Pingdom { get; set; }
        public ReportScore Stats_Performance { get; set; }
        public ReportScore Stats_BestPractices { get; set; }
        public ReportScore Stats_Accessibility { get; set; }
        public ReportScore Stats_SEO { get; set; }
        //public ReportScore Stats_Security { get; set; }

    }
    internal class ReportScore
    {
        [JsonProperty("time")]
        public long UnixTimestamp { get; set; }
        public double Score { get; set; }
    }
}