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
    public class Report_ClientsRepository : GenericRepository<Report_Clients>, IReport_ClientsRepository
    {
        public Report_ClientsRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {

        }
    }
}
