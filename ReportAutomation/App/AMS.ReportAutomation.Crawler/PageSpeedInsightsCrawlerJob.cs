using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Common.Mail;
using AMS.ReportAutomation.Data.Services.Crawler;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Data.ViewModel;
using System.Threading;
using System.Linq;
using AMS.ReportAutomation.Data.DataContext;

namespace AMS.ReportAutomation.Crawler
{
    public class PageSpeedInsightsCrawlerJob : IJob
    {
        private IPageSpeedInsights_Service _pageSpeedInsights_Service;
        private IAMSLogger _logger;
        private EmailService _emailService;

        public PageSpeedInsightsCrawlerJob(IPageSpeedInsights_Service pageSpeedInsights_Service, IAMSLogger logger)
        {
            _pageSpeedInsights_Service = pageSpeedInsights_Service;
            _logger = logger;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            const uint const_NumberOfThreads = 5;
            return Task.Run(() =>
            {
                _logger.Information("Start to execute Google PageSpeed Insights crawler job.");
                try
                {
                    var clientSites = _pageSpeedInsights_Service.GetClientSites();
                    if (clientSites != null && clientSites.Count > 0)
                    {
                        //<thread Id, list of sites>
                        var sitesForThreads = new Dictionary<int, List<PSICheckSite>>();
                        //<thread Id, true/false>
                        var didThreadsCompleted = new Dictionary<int, bool>();
                        for (var i = 1; i <= const_NumberOfThreads; i++)
                        {
                            sitesForThreads.Add(i, new List<PSICheckSite>());
                            didThreadsCompleted.Add(i, false);
                        }
                        int count = 0;
                        foreach (var clientSite in clientSites)
                        {
                            count++;
                            if (count > const_NumberOfThreads) count = 1;
                            sitesForThreads[count].Add(clientSite);
                        }

                        //Start crawling                        
                        foreach (var sitesForThread in sitesForThreads)
                        {
                            new Thread((sites) =>
                            {
                                Thread.CurrentThread.IsBackground = false;
                                var sitesToProcess = (KeyValuePair<int, List<PSICheckSite>>)sites;

                                _logger.Information($"[Thread #{sitesToProcess.Key}] {sitesToProcess.Value.Count()} site(s) to process. Thread.CurrentThread.IsBackground = {Thread.CurrentThread.IsBackground}");
                                var listParams = PageSpeedInsights_Service.CrawlParamList();
                                foreach (var site in sitesToProcess.Value)
                                {
                                    try
                                    {
                                        _logger.Information($"[Thread #{sitesToProcess.Key}] Getting analysis for the site {site.CrawlerConfig.PsiUrl}");
                                        var batchDateTimeUtc = DateTime.UtcNow.ToUnixTimestamp();
                                        foreach (var param in listParams)
                                        {
                                            _logger.Information($"[Thread #{sitesToProcess.Key}] - {param.Category}, {param.Strategy}, Locale={param.Locale}");
                                            Data_PageSpeedInsights crawledData;
                                            try
                                            {
                                                crawledData = _pageSpeedInsights_Service.GetPageSpeedData(site, param, batchDateTimeUtc);
                                            }
                                            catch (Exception ex)
                                            {
                                                string errMessage = $"{ex.Message} {(ex.InnerException != null ? "InnerException: " + ex.InnerException.Message : string.Empty)} StackTrace: {ex.StackTrace}";
                                                _logger.Error(errMessage);

                                                //Continue to the next site/check
                                                continue;
                                            }
                                            
                                            _pageSpeedInsights_Service.SavePageSpeedData(crawledData);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string errMessage = $"[Thread #{sitesToProcess.Key}] {ex.Message} {(ex.InnerException != null ? "InnerException: " + ex.InnerException.Message : string.Empty)} StackTrace: {ex.StackTrace}";
                                        _logger.Error(errMessage);
                                        try
                                        {
                                            _emailService.SendNotification(errMessage);
                                        }
                                        catch (Exception e)
                                        {
                                            _logger.Error($"[Thread #{sitesToProcess.Key}] Could not send email to notify exception occurred. Error: {e.Message}");
                                        }

                                        //Not to continue
                                        break;
                                    }
                                }

                                //This thread completed
                                didThreadsCompleted[sitesToProcess.Key] = true;
                            }).Start(sitesForThread);
                        }

                        //Wait until all thread completed their work, to log the "completed" message accurately, and to make sure all injected services (i.e. _pageSpeedInsights_Service) do not dispose and are still usable by the threads (not sure if they would be dispose but it is better to be safe ^^)
                        while (didThreadsCompleted.Values.Contains(false))
                        {
                            //Sleep for 5 seconds before checking again
                            Thread.Sleep(5000);
                        }
                    }

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["pagespeed:DataRetentionInHours"]);
                    _pageSpeedInsights_Service.DeleteOldData(retentionTime);
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
                _logger.Information("Google PageSpeed Insights crawler job completed.");
            }, context.CancellationToken);
        }

    }
}
