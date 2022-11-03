using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.ReportDataProcessor
{
    public class GoogleSearchConsoleProcessorJob : IJob
    {
        private IAMSLogger _logger;
        private EmailService _emailService;

        private IGoogleSearchConsole_ProcessorService _googleSearchConsole_ProcessorService;

        public GoogleSearchConsoleProcessorJob(IAMSLogger logger, IGoogleSearchConsole_ProcessorService googleSearchConsole_ProcessorService)
        {
            _logger = logger;
            _emailService = new EmailService();
            _googleSearchConsole_ProcessorService = googleSearchConsole_ProcessorService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                _logger.Information("Start to execute Google Search Processor job.");
                try
                {
                    //Process data and write to DB
                    _googleSearchConsole_ProcessorService.Process();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["googlesearch:ReportDataRetentionInHours"]);
                    _googleSearchConsole_ProcessorService.DeleteOldData(retentionTime);
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
                _logger.Information("Google Search Processor job completed.");
            }, context.CancellationToken);
        }
    }
}