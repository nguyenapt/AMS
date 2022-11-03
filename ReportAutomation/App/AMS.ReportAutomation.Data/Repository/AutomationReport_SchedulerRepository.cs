using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Repository
{
    public class AutomationReport_SchedulerRepository : GenericRepository<AutomationReport_Scheduler>, IAutomationReport_SchedulerRepository
    {
        public AutomationReport_SchedulerRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {

        }
    }
}
