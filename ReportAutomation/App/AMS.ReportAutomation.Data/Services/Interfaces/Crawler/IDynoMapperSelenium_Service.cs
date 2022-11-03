using System.Collections.Generic;
using AMS.ReportAutomation.Data.DataContext;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Crawler
{
    public interface IDynoMapperSelenium_Service
    {
        string UserName { get; set; }
        string Password { get; set; }
        string DataFolder { get; set; }
        string OutputFolder { get; set; }

        void Crawl();

        void DeleteOldData(int retentionTime);

        List<Dynomapper> GetListProjectAndSaveToDB();
    }
}
