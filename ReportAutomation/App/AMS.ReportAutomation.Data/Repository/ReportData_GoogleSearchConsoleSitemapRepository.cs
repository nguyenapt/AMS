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
    public class ReportData_GoogleSearchConsoleSitemapRepository : GenericRepository<ReportData_GoogleSearch_Sitemap>, IReportData_GoogleSearchConsoleSitemapRepository
    {
        public ReportData_GoogleSearchConsoleSitemapRepository(Entities context,IAMSLogger logger): base(context,logger)
        {

        }
    }
}
