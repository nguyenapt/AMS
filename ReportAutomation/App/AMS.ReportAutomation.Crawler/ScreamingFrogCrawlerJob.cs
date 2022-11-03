using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Threading.Tasks;
using AMS.ReportAutomation.Common.Mail;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using System.Configuration;

namespace AMS.ReportAutomation.Crawler
{
    public class ScreamingFrogCrawlerJob : IJob
    {
        private IScreamingFrog_Service _screamingFrogService;
        private IAMSLogger _logger;
        private EmailService _emailService;

        public ScreamingFrogCrawlerJob(IScreamingFrog_Service screamingFrog_Service, IAMSLogger logger)
        {
            _screamingFrogService = screamingFrog_Service;
            _logger = logger;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                _logger.Information("Start to execute ScreamingFrog crawler job.");
                try
                {
                    //Craw data
                    _screamingFrogService.SpiderProgramFolder = ConfigurationManager.AppSettings["screamingFrog:SpiderProgramFolder"];
                    _screamingFrogService.SpiderConfigFile = ConfigurationManager.AppSettings["screamingFrog:SpiderConfigFile"];
                    _screamingFrogService.SpiderOutputFolder = ConfigurationManager.AppSettings["screamingFrog:SpiderOutputFolder"];
                    _screamingFrogService.Crawl();

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["screamingFrog:DataRetentionInHours"]);
                    _screamingFrogService.DeleteOldData(retentionTime);
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
                _logger.Information("ScreamingFrog crawler job completed.");
            }, context.CancellationToken);
        }
    }
}