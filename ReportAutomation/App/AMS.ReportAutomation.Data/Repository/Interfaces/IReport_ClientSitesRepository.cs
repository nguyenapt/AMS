using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Repository.Interfaces
{
    public interface IReport_ClientSitesRepository : IGenericRepository<Report_ClientSites>
    {
        List<Report_ClientSiteViewModel> GetReport_ClientSiteViewModels(int clientId);
    }
}
