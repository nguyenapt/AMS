using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.ViewModel;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Crawler
{
    public interface IGoogleSearchConsole_Service : IEntityService<Data_GoogleSearch>
    {
        string GoogleCredentialsFolder { get; set; }
        List<GoogleSearchSite> GetClientSites();
        void GetReportDataAndSaveToDB(GoogleSearchSite clientSite);
        void DeleteOldData(int retentionTime);
    }
}
