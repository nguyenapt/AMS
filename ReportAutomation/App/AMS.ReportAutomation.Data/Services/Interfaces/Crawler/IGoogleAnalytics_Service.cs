using AMS.ReportAutomation.Data.ViewModel;
using System.Collections.Generic;
using AMS.ReportAutomation.Data.DataContext;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Crawler
{
    public interface IGoogleAnalytics_Service : IEntityService<Data_GoogleAnalytic>
    {
        string GoogleCredentialsFolder { get; set; }
        void GetReportDataAndSaveToDB(GoogleAnalyticsSite clientSite);
        List<GoogleAnalyticsSite> GetClientSites();
        void DeleteOldData(int retentionTime);
    }
}
