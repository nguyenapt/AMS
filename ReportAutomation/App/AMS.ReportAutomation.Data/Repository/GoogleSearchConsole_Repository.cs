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
    public class GoogleSearchConsole_Repository : GenericRepository<Data_GoogleSearch>, IGoogleSearchConsole_Repository
    {
        public GoogleSearchConsole_Repository(Entities context, IAMSLogger logger) : base(context, logger)
        {

        }
    }
}
