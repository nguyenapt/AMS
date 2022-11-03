using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.ReportDataProcessor
{
    public class JiraServiceDeskProcessorJob : IJob
    {
        private IAMSLogger _logger;
        private EmailService _emailService;

        private IJira_ServiceDesk_ProcessorService _jira_ServiceDesk_ProcessorService;

        public JiraServiceDeskProcessorJob(IAMSLogger logger, IJira_ServiceDesk_ProcessorService jira_ServiceDesk_ProcessorService)
        {
            _logger = logger;
            _jira_ServiceDesk_ProcessorService = jira_ServiceDesk_ProcessorService;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                _logger.Information("Start to execute JiraServiceDesk Processor job.");
                try
                {
                    //Process data and write to DB
                    _jira_ServiceDesk_ProcessorService.Process();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["jiraservicedesk:ReportDataRetentionInHours"]);
                    _jira_ServiceDesk_ProcessorService.DeleteOldData(retentionTime);
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
                _logger.Information("JiraServiceDesk Processor job completed.");
            }, context.CancellationToken);
        }
    }
}