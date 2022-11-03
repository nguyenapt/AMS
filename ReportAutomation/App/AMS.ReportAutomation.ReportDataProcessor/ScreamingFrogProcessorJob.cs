using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Common.Mail;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.ReportDataProcessor
{
    public class ScreamingFrogProcessorJob : IJob
    {
        private IAMSLogger _logger;
        private EmailService _emailService;

        private IScreamingFrog_ProcessorService _screamingFrog_ProcessorService;

        public ScreamingFrogProcessorJob(IAMSLogger logger, IScreamingFrog_ProcessorService screamingFrog_ProcessorService)
        {
            _logger = logger;
            _screamingFrog_ProcessorService = screamingFrog_ProcessorService;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                _logger.Information("Start to execute ScreamingFrog Processor job.");
                try
                {
                    //Process data and write to DB
                    _screamingFrog_ProcessorService.SpiderOutputFolder = ConfigurationManager.AppSettings["screamingFrog:SpiderOutputFolder"];
                    _screamingFrog_ProcessorService.Process();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["screamingfrog:ReportDataRetentionInHours"]);
                    _screamingFrog_ProcessorService.DeleteOldData(retentionTime);
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
                _logger.Information("ScreamingFrog Processor job completed.");
            }, context.CancellationToken);
        }
    }
}