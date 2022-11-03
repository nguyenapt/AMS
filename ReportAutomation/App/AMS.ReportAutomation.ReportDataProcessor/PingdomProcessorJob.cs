using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.ReportDataProcessor
{
    public class PingdomProcessorJob : IJob
    {
        private IPingdom_Check_ProcessorService _pingdom_Check_ProcessorService;
        private IAMSLogger _logger;
        private EmailService _emailService;
        public PingdomProcessorJob(IPingdom_Check_ProcessorService pingdom_Check_ProcessorService, IAMSLogger logger)
        {
            _pingdom_Check_ProcessorService = pingdom_Check_ProcessorService;
            _logger = logger;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => {
                _logger.Information("Start to execute Pingdom Processor job.");
                try
                {
                    //Process data and write to DB
                    _pingdom_Check_ProcessorService.Process();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["pingdom:ReportDataRetentionInHours"]);
                    _pingdom_Check_ProcessorService.DeleteOldData(retentionTime);
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
                _logger.Information("Pingdom Processor job completed.");
            }, context.CancellationToken);
        }
    }
}
