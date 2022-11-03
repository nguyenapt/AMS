using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Common.Mail;

namespace AMS.ReportAutomation.Crawler
{
    public class DynoMapperSeleniumCrawlerJob : IJob
    {
        private IDynoMapperSelenium_Service _dynoMapperSeleniumService;
        private IAMSLogger _logger;
        private EmailService _emailService;

        public DynoMapperSeleniumCrawlerJob(IDynoMapperSelenium_Service dynoMapperSeleniumService, IAMSLogger logger)
        {
            _dynoMapperSeleniumService = dynoMapperSeleniumService;
            _logger = logger;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => {
                _logger.Information("Start to execute Dynomapper crawler job.");
                try
                {
                    //TODO: Craw data
                    _dynoMapperSeleniumService.UserName = ConfigurationManager.AppSettings["dynomapper:UserName"];
                    _dynoMapperSeleniumService.Password = ConfigurationManager.AppSettings["dynomapper:Password"];
                    _dynoMapperSeleniumService.DataFolder = ConfigurationManager.AppSettings["dynomapper:DataFolder"];
                    _dynoMapperSeleniumService.OutputFolder = ConfigurationManager.AppSettings["dynomapper:OutputFolder"];
                    _dynoMapperSeleniumService.Crawl();

                    //TODO: Delete old data
                    //var retentionTime = int.Parse(ConfigurationManager.AppSettings["dynomapper:DataRetentionInHours"]);
                    //_dynoMapperSeleniumService.DeleteOldData(retentionTime);
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
                _logger.Information("Detectify crawler job completed.");
            }, context.CancellationToken);
        }
    }
}
