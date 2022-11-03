using System.Collections.Generic;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Services.Crawler;
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Crawler
{
    public interface IPageSpeedInsights_Service : IEntityService<Data_PageSpeedInsights>
    {
        List<PSICheckSite> GetClientSites();
                        
        Data_PageSpeedInsights GetPageSpeedData(PSICheckSite siteInfo, PSIRequestParams param, long batchUnixTimestamp);

        void SavePageSpeedData(Data_PageSpeedInsights data);

        void DeleteOldData(int retentionTime);
    }
}
