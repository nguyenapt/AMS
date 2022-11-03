using System.Collections.Generic;
using System.Linq;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.Data.Repository
{
    public class Report_ReportsRepository : GenericRepository<Report_Reports>, IReport_ReportsRepository
    {
        private Entities _dbContext;
        public Report_ReportsRepository(Entities context, IAMSLogger logger) : base(context,logger)
        {
            _dbContext = context;
        }

        public IList<ClientSite_ReportViewModel> GetClientSiteReportViewModels()
        {
            var result = (from x in _dbContext.Report_Reports
                join cs in _dbContext.Report_ClientSites on x.ClientSiteId equals cs.Id
                select new ClientSite_ReportViewModel()
                {
                    Id = x.Id,
                    ClientSiteId = x.ClientSiteId,
                    ReportName = cs.Name + " - " + x.ReportName,
                    IsActive = x.IsActive,
                    ToolIdString = x.ToolIds
                }).ToList();

            return result;
        }
    }
}
