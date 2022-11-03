using AMS.ReportAutomation.Data.DataContext;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Crawler
{
    public interface IPingdom_CheckService : IEntityService<Data_Pingdom_Check>
    {
        Data_Pingdom_Check GetById(long id);

        //void AddPingDomCheckDetail(Data_Pingdom_CheckDetail checkDetail);
        List<int> GetListIdPingDomCheckAndSaveToDB();

        void GetPingDomCheckAndSaveToDB();        

        void GetPingDomSummaryAndSaveToDB(long checkId);

        void DeleteOldData(int retentionTime);
    }
}
