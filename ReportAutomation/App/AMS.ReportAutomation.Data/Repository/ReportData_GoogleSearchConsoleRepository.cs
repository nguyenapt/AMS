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
    public class ReportData_GoogleSearchConsoleRepository : GenericRepository<ReportData_GoogleSearch>, IReportData_GoogleSearchConsoleRepository
    {
        public ReportData_GoogleSearchConsoleRepository(Entities context,IAMSLogger logger): base(context,logger)
        {

        }
    }
}
