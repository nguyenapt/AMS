using AMS.ReportAutomation.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Services.Interfaces
{
    public interface IAutomationReport_SchedulerService : IEntityService<AutomationReport_Scheduler>
    {
        IList<AutomationReport_Scheduler> GetCrawlerConfigurations();

        AutomationReport_Scheduler GetCrawlerConfigurationByTool(string toolName);

        IList<AutomationReport_Scheduler> GetProcessorConfigurations();
        AutomationReport_Scheduler GetProcessorConfigurationByTool(string toolName);

        AutomationReport_Scheduler SaveConfiguration(AutomationReport_Scheduler configuration);

        IList<string> GetTools();

        void DeleteConfiguration(AutomationReport_Scheduler configuration);
    }
}
