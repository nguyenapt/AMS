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
    public class GoogleAnalytics_Repository : GenericRepository<Data_GoogleAnalytic>, IGoogleAnalytics_Repository
    {
        public GoogleAnalytics_Repository(Entities context,IAMSLogger logger): base(context,logger)
        {

        }
    }
}
