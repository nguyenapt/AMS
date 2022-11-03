using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.ViewModel;
using AMS.ReportAutomation.ReportData.Pingdom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Services.Interfaces
{
    public interface IClient_ManagementService
    {
        void AddClient(string clientName, string logoUrl);

        void UpdateClient(int clientId, string clientName, string logoUrl);

        void DeleteClient(int clientId);

        void AddServiceDesk(Report_ClientJiraServiceDesks serviceDesk);

        void UpdateServiceDesk(Report_ClientJiraServiceDesks serviceDesk);

        void DeleteServiceDesk(Guid serviceDeskId);

        IList<Report_ClientJiraServiceDesks> GetReport_ClientJiraServiceDesks();

        IList<Report_Clients> GetReport_Clients();

        Report_Clients GetReport_Client(int clientId);

        Report_ClientSites GetReport_ClientSite(Guid clientSiteId);

        IList<Report_ClientSiteViewModel> GetReport_ClientSitesByClient(int clientId);

        Report_Reports GetReport_ReportsByClientSiteId(Guid clientSiteId);

        void AddClientSite(Report_ClientSiteViewModel clientSites);

        void UpdateClientSite(Report_ClientSiteViewModel clientSites);

        void DeleteClientSite(Guid clientSiteId);
    }
}
