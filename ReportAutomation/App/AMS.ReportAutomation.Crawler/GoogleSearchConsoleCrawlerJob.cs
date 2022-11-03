using AMS.ReportAutomation.Common.Base;
using Quartz;
using System;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Common.Mail;
using System.Linq;

namespace AMS.ReportAutomation.Crawler
{
    public class GoogleSearchConsoleCrawlerJob : IJob
    {
        private IGoogleSearchConsole_Service _googleSearchConsole_Service;
        private IAMSLogger _logger;
        private EmailService _emailService;

        public GoogleSearchConsoleCrawlerJob(IGoogleSearchConsole_Service googleSearchConsole_Service, IAMSLogger logger)
        {
            _googleSearchConsole_Service = googleSearchConsole_Service;
            _googleSearchConsole_Service.GoogleCredentialsFolder = ConfigurationManager.AppSettings["googlesearch:CredentialsFolder"];
            _logger = logger;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => {
                _logger.Information("Start to execute GoogleSearchConsole crawler job.");
                try
                {
                    //Craw data
                    var clientSites = _googleSearchConsole_Service.GetClientSites();
                    if (clientSites != null && clientSites.Any())
                    {
                        foreach (var clientSite in clientSites)
                        {
                            _logger.Information($"Getting search analytics for the site {clientSite.SiteUrl}");
                            try
                            {
                                _googleSearchConsole_Service.GetReportDataAndSaveToDB(clientSite);
                            }
                            catch (Exception ex)
                            {
                                string errMessage = $"{ex.Message} {(ex.InnerException != null ? "InnerException: " + ex.InnerException.Message : string.Empty)} StackTrace: {ex.StackTrace}";
                                _logger.Error(errMessage);
                                if (errMessage.ToLower().Contains("token") || errMessage.ToLower().Contains("oauth"))
                                {
                                    try
                                    {
                                        _emailService.SendNotification(errMessage);
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.Error($"Could not send email to notify exception occurred. Error: {e.Message}");
                                    }
                                }                                

                                //Continue to the next site/check
                                continue;
                            }
                        }
                    }                    

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["googlesearch:DataRetentionInHours"]);
                    _googleSearchConsole_Service.DeleteOldData(retentionTime);
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
                _logger.Information("GoogleSearchConsole crawler job completed.");
            }, context.CancellationToken);
        }
    }
}
