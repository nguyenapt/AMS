using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.ReportDataProcessor
{
    public class GoogleAnalyticsProcessorJob : IJob
    {
        private IAMSLogger _logger;
        private EmailService _emailService;

        private IGoogleAnalytics_ProcessorService _googleAnalyticsProcessorService;

        public GoogleAnalyticsProcessorJob(IAMSLogger logger, IGoogleAnalytics_ProcessorService googleAnalyticsProcessorService)
        {
            _logger = logger;
            _emailService = new EmailService();
            _googleAnalyticsProcessorService = googleAnalyticsProcessorService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                _logger.Information("Start to execute Google Analytics Processor job.");
                try
                {
                    //Process data and write to DB
                    _googleAnalyticsProcessorService.Process();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["googleanalytics:ReportDataRetentionInHours"]);
                    _googleAnalyticsProcessorService.DeleteOldData(retentionTime);
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
                _logger.Information("Google Analytic Processor job completed.");
            }, context.CancellationToken);
        }
    }
}