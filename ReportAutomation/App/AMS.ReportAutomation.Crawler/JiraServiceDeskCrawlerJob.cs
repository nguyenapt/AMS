using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.Crawler
{
    public class JiraServiceDeskCrawlerJob : IJob
    {        
        private IAMSLogger _logger;
        private EmailService _emailService;
        private IJira_ServiceDeskService _jira_ServiceDeskService;        

        public JiraServiceDeskCrawlerJob(IJira_ServiceDeskService jira_ServiceDeskService, IAMSLogger logger)
        {
            _logger = logger;
            _jira_ServiceDeskService = jira_ServiceDeskService;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                _logger.Information("Start to execute JiraServiceDesk crawler job.");
                try
                {
                    _jira_ServiceDeskService.GetIssuesByJQLAndSaveToDB();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["jiraservicedesk:DataRetentionInHours"]);
                    _jira_ServiceDeskService.DeleteOldData(retentionTime);
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
                _logger.Information("JiraServiceDesk crawler job completed.");
            }, context.CancellationToken);
        }
    }
}