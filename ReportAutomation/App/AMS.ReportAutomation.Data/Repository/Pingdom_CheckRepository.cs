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
    public class Pingdom_CheckRepository : GenericRepository<Data_Pingdom_Check>, IPingdom_CheckRepository
    {
        public Pingdom_CheckRepository(Entities context,IAMSLogger logger): base(context,logger)
        {

        }
    }
}
