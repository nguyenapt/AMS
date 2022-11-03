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
    public class GoogleSearchConsole_DetailRepository : GenericRepository<Data_GoogleSearch_Detail>, IGoogleSearchConsole_DetailRepository
    {
        public GoogleSearchConsole_DetailRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {

        }
    }
}
