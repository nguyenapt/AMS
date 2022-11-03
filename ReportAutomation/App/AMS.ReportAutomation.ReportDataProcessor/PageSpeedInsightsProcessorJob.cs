using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.ReportDataProcessor
{
    public class PageSpeedInsightsProcessorJob : IJob
    {
        private readonly IPageSpeedInsights_ProcessorService _pageSpeedInsights_ProcessorService;
        private readonly IAMSLogger _logger;
        private EmailService _emailService;
        public PageSpeedInsightsProcessorJob(IPageSpeedInsights_ProcessorService pageSpeedInsights_ProcessorService, IAMSLogger logger)
        {
            _pageSpeedInsights_ProcessorService = pageSpeedInsights_ProcessorService;
            _logger = logger;
            _emailService = new EmailService();
        }


        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => {
                _logger.Information("Start to execute Google PageSpeed Insights Processor job.");
                try
                {
                    _pageSpeedInsights_ProcessorService.Process();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["pagespeed:ReportDataRetentionInHours"]);
                    _pageSpeedInsights_ProcessorService.DeleteOldData(retentionTime);
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
                _logger.Information("Google PageSpeed Insights Processor job completed.");
            }, context.CancellationToken);
        }
    }
}
