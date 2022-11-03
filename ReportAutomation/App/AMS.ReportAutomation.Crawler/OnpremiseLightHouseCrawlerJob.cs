using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.Crawler
{
    public class OnpremiseLightHouseCrawlerJob : IJob
    {
        private IOnpremiseLightHouse_Service _onpremiseLightHouse_Service;
        private IAMSLogger _logger;
        private EmailService _emailService;

        public OnpremiseLightHouseCrawlerJob(IOnpremiseLightHouse_Service onpremiseLightHouse_Service, IAMSLogger logger)
        {
            _onpremiseLightHouse_Service = onpremiseLightHouse_Service;
            _logger = logger;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => {
                _logger.Information("Start to execute OnpremiseLightHouse crawler job.");
                try
                {
                    //TODO: Craw data

                    //TODO: Delete old data
                    //var retentionTime = int.Parse(ConfigurationManager.AppSettings["onpremiselighthouse:DataRetentionInHours"]);
                    //_onpremiseLightHouse_Service.DeleteOldData(retentionTime);
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
                _logger.Information("OnpremiseLightHouse crawler job completed.");
            }, context.CancellationToken);
        }
    }
}
