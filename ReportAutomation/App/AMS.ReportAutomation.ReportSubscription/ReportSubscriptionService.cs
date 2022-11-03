using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Quartz;
using System;
using System.Linq;

namespace AMS.ReportAutomation.ReportSubscription
{
    public class ReportSubscriptionService : IReportSubscriptionService
    {
        private IReportSubscription_Service _reportSubscription_Service;

        private readonly IScheduler _scheduler;

        private IJobService _jobService;

        public ReportSubscriptionService(IReportSubscription_Service reportSubscription_Service, IScheduler scheduler, IJobService jobService)
        {
            _reportSubscription_Service = reportSubscription_Service;
            _scheduler = scheduler;
            _jobService = jobService;
        }

        public void Start()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            var reportSubscriptions = _reportSubscription_Service.GetReportSubscriptionConfigurations();

            if (reportSubscriptions != null && reportSubscriptions.Any())
            {
                foreach (var reportSubscription in reportSubscriptions)
                {
                    if (reportSubscription != null)
                    {
                        var job = _jobService.CreateJob(typeof(ReportSubscriptionJob),reportSubscription.Id.ToString());
                        job.JobDataMap["ReportId"] = reportSubscription.ReportId.ToString();
                        job.JobDataMap["ReportName"] = reportSubscription.ReportName;
                        job.JobDataMap["ReceiverEmails"] = reportSubscription.ReceiverEmails;
                        //Calculate the report date & report range
                        string reportTo = null;
                        int reportDays = 30;
                        if (reportSubscription.IsMonthly == true)
                        {
                            //To the last day of last month
                            reportTo = DateTime.UtcNow.AddDays(1).AddMonths(-1).ToString("yyyy-MM-dd");
                        }
                        else if (reportSubscription.IsWeekly == true)
                        {
                            //To the last day of last week
                            reportTo = DateTime.UtcNow.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Sunday).ToString("yyyy-MM-dd");
                        }
                        job.JobDataMap["ReportTo"] = reportTo;
                        job.JobDataMap["ReportDays"] = reportDays;

                        var trigger = _jobService.CreateTrigger(reportSubscription.Id.ToString(), reportSubscription.Frequency);
                        _scheduler.ScheduleJob(job, trigger);
                    }
                }
                    
                _scheduler.Start();
            }
        }

        public void Stop()
        {
            _scheduler.Shutdown(true);
        }
    }
}
