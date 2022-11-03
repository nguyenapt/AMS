using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Quartz;
using System.Linq;

namespace AMS.ReportAutomation.ReportDataProcessor
{
    public class ProcessorService : IProcessorService
    {
        private IAutomationReport_SchedulerService _automationReport_SchedulerService;

        private readonly IScheduler _scheduler;

        private IJobService _jobService;

        public ProcessorService(IAutomationReport_SchedulerService automationReport_SchedulerService, IScheduler scheduler, IJobService jobService)
        {
            _automationReport_SchedulerService = automationReport_SchedulerService;
            _scheduler = scheduler;
            _jobService = jobService;
        }

        public void Start()
        {
#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            var configurations = _automationReport_SchedulerService.GetProcessorConfigurations();

            if (configurations != null && configurations.Any())
            {
                var types = _jobService.GetAllJob<IJob>();

                foreach (var tp in types)
                {
                    if (tp.Name != "IJob")
                    {
                        var job = _jobService.CreateJob(tp);

                        AutomationReport_Scheduler configuration;
                        switch (tp.Name)
                        {
                            case "PingdomProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "Pingdom");
                                break;
                            case "PageSpeedInsightsProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "PageSpeed");
                                break;
                            case "JiraServiceDeskProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "Jira");
                                break;
                            case "DareboostProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "Dareboost");
                                break;
                            case "DetectifyProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "Detectify");
                                break;
                            case "GoogleAnalyticsProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "GoogleAnalytics");
                                break;
                            case "GoogleSearchConsoleProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "GoogleSearchConsole");
                                break;
                            case "OnpremiseLightHouseProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "OnpremiseLightHouse");
                                break;
                            case "ScreamingFrogProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "ScreamingFrog");
                                break;
                            case "SpeedCurveProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "SpeedCurve");
                                break;
                            case "DynomapperProcessorJob":
                                configuration = configurations.FirstOrDefault(x => x.ExecutionTool == "DynoMapper");
                                break;
                            default:
                                configuration = null;
                                break;

                        }

                        if (configuration != null && !string.IsNullOrWhiteSpace(configuration.ExecutionTool))
                        {
                            var trigger = _jobService.CreateTrigger(configuration.ExecutionTool, configuration.ExecutionExpression);
                            _scheduler.ScheduleJob(job, trigger);
                        }
                        else
                        {
                            //TODO: log to tell that schedule config should be done?
                        }
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
