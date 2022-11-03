using System;
using AMS.ReportAutomation.Data.DataContext;
using System.Collections.Generic;
using AMS.ReportAutomation.ReportData.GoogleSearch;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IGoogleSearchConsole_ProcessorService
    {
        void Process();

        List<GoogleSearchData> GetGoogleSearchReportDataByClientId(Guid clientId, long reportToDateTime);
        List<GoogleSearchSitemapModel> GetGoogleSearchSitemapReportDataByClientId(Guid clientId, long reportToDateTime);
        void DeleteOldData(int retentionTime);
    }
}
