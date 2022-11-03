using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Common.Mail;
using AMS.ReportAutomation.Data.Services.Crawler;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;

namespace AMS.ReportAutomation.Crawler
{
    public class PingdomCrawlerJob : IJob
    {
        private IPingdom_CheckService _pingdom_CheckService;
        private IAMSLogger _logger;
        private EmailService _emailService;

        public PingdomCrawlerJob(IPingdom_CheckService pingdom_CheckService, IAMSLogger logger)
        {
            _pingdom_CheckService = pingdom_CheckService;
            _logger = logger;
            _emailService = new EmailService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            const uint const_NumberOfThreads = 3;
            return Task.Run(() => {
                _logger.Information("Start to execute Pingdom crawler job.");
                try
                {
                    var ids = _pingdom_CheckService.GetListIdPingDomCheckAndSaveToDB();
                    if (ids != null && ids.Any())
                    {
                        //<thread Id, list of pingdom checkIds>
                        var checkIdsForThreads = new Dictionary<int, List<int>>();
                        //<thread Id, true/false>
                        var didThreadsCompleted = new Dictionary<int, bool>();
                        for (var i = 1; i <= const_NumberOfThreads; i++)
                        {
                            checkIdsForThreads.Add(i, new List<int>());
                            didThreadsCompleted.Add(i, false);
                        }
                        int count = 0;
                        foreach (var checkId in ids)
                        {
                            count++;
                            if (count > const_NumberOfThreads) count = 1;
                            checkIdsForThreads[count].Add(checkId);
                        }

                        //Start crawling
                        foreach (var checkIdsForThread in checkIdsForThreads)
                        {
                            new Thread((checkIds) =>
                            {
                                Thread.CurrentThread.IsBackground = false;
                                var checkIdsToProcess = (KeyValuePair<int, List<int>>)checkIds;

                                _logger.Information($"[Thread #{checkIdsToProcess.Key}] {checkIdsToProcess.Value.Count()} pingdomCheck(s) to process. Thread.CurrentThread.IsBackground = {Thread.CurrentThread.IsBackground}");
                                foreach (var checkId in checkIdsToProcess.Value)
                                {
                                    try
                                    {
                                        _logger.Information($"[Thread #{checkIdsToProcess.Key}] Executing method GetPingDomSummaryAndSaveToDB, checkId={checkId}");
                                        _pingdom_CheckService.GetPingDomSummaryAndSaveToDB(checkId);
                                    }
                                    catch (Pingdom_CheckService.PingdomClientException ex)
                                    {
                                        _logger.Error(ex.Message);
                                        if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                                        _logger.Error(ex.StackTrace);

                                        //Continue to the next pingdomCheck
                                        continue;
                                    }
                                    catch (Exception ex)
                                    {
                                        string errMessage = $"[Thread #{checkIdsToProcess.Key}] {ex.Message} {(ex.InnerException != null ? "InnerException: " + ex.InnerException.Message : string.Empty)} StackTrace: {ex.StackTrace}";
                                        _logger.Error(errMessage);
                                        try
                                        {
                                            _emailService.SendNotification(errMessage);
                                        }
                                        catch (Exception e)
                                        {
                                            _logger.Error($"[Thread #{checkIdsToProcess.Key}] Could not send email to notify exception occurred. Error: {e.Message}");
                                        }

                                        //Not to continue
                                        break;
                                    }
                                }

                                //This thread completed
                                didThreadsCompleted[checkIdsToProcess.Key] = true;
                            }).Start(checkIdsForThread);
                        }

                        //Wait until all thread completed their work, to log the "completed" message accurately, and to make sure all injected services (i.e. _pingdom_CheckService) do not dispose and are still usable by the threads (not sure if they would be dispose but it is better to be safe ^^)
                        while (didThreadsCompleted.Values.Contains(false))
                        {
                            //Sleep for 5 seconds before checking again
                            Thread.Sleep(5000);
                        }
                    }

                    //Delete old data
                    var retentionTime = int.Parse(ConfigurationManager.AppSettings["pingdom:DataRetentionInHours"]);
                    _pingdom_CheckService.DeleteOldData(retentionTime);
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
                _logger.Information("Pingdom crawler job completed.");
            }, context.CancellationToken);
        }
    }
}