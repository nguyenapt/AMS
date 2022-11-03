using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Repository
{
    public class Pingdom_CheckSummaryRepository : GenericRepository<Data_Pingdom_Check_SummaryPerformance>, IPingdom_CheckSummaryRepository
    {
        public Pingdom_CheckSummaryRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {

        }
    }
}
