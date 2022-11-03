using System.Collections.Generic;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.Data.Repository.Interfaces
{
    public interface IReport_ReportsRepository : IGenericRepository<Report_Reports>
    {
        IList<ClientSite_ReportViewModel> GetClientSiteReportViewModels();
    }
}
