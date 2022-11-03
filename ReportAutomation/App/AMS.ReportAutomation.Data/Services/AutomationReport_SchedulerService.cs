using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Newtonsoft.Json;
using PingdomClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Services
{
    public class AutomationReport_SchedulerService : EntityService<AutomationReport_Scheduler>, IAutomationReport_SchedulerService
    {
        private readonly IAutomationReport_SchedulerRepository _automationReport_SchedulerRepository;

        public AutomationReport_SchedulerService(IAutomationReport_SchedulerRepository automationReport_SchedulerRepository, IAMSLogger logger)
            : base(automationReport_SchedulerRepository, logger)
        {
            _automationReport_SchedulerRepository = automationReport_SchedulerRepository;

        }

        public void DeleteConfiguration(AutomationReport_Scheduler configuration)
        {
            if (configuration.Id != Guid.Empty)
            {
                var existConfiguration = _automationReport_SchedulerRepository.FindFirstOrDefault(x => x.Id == configuration.Id);

                if (existConfiguration != null)
                {
                    _automationReport_SchedulerRepository.Delete(existConfiguration);
                    this.SafeExecute(() => _automationReport_SchedulerRepository.Save());
                }
            }
        }

        public AutomationReport_Scheduler GetCrawlerConfigurationByTool(string toolName)
        {
            return _automationReport_SchedulerRepository.FindFirstOrDefault(x => x.ExecutionTool == toolName && x.ExecutionType == (int)SchedulerType.CrawData);
        }

        public IList<AutomationReport_Scheduler> GetCrawlerConfigurations()
        {
            return _automationReport_SchedulerRepository.FindBy(x => x.ExecutionType == (int)SchedulerType.CrawData).ToList();
        }

        public AutomationReport_Scheduler GetProcessorConfigurationByTool(string toolName)
        {
            return _automationReport_SchedulerRepository.FindFirstOrDefault(x => x.ExecutionTool == toolName && x.ExecutionType == (int)SchedulerType.ProcessorData);
        }

        public IList<AutomationReport_Scheduler> GetProcessorConfigurations()
        {
            return _automationReport_SchedulerRepository.FindBy(x => x.ExecutionType == (int)SchedulerType.ProcessorData).ToList();
        }

        public IList<string> GetTools()
        {
            return _automationReport_SchedulerRepository.GetAll().Select(x=>x.ExecutionTool)?.Distinct().ToList();
        }

        public AutomationReport_Scheduler SaveConfiguration(AutomationReport_Scheduler configuration)
        {
            if (configuration.Id == Guid.Empty)
            {
                configuration.Id = Guid.NewGuid();
                _automationReport_SchedulerRepository.Add(configuration);
            }
            else
            {
                _automationReport_SchedulerRepository.Edit(configuration);
            }
            this.SafeExecute(() => _automationReport_SchedulerRepository.Save());

            return configuration;
        }
    }
}
