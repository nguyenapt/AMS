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
    public class Report_ClientJiraServiceDesksRepository : GenericRepository<Report_ClientJiraServiceDesks>, IReport_ClientJiraServiceDesksRepository
    {
        public Report_ClientJiraServiceDesksRepository(Entities context,IAMSLogger logger): base(context,logger)
        {

        }
    }
}
