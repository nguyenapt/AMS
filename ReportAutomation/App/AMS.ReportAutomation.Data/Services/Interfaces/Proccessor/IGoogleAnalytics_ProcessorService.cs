using System;
using AMS.ReportAutomation.Data.DataContext;
using System.Collections.Generic;
using AMS.ReportAutomation.ReportData.GoogleAnalytic;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IGoogleAnalytics_ProcessorService
    {
        void Process();

        GoogleAnalyticData GetGoogleAnalyticReportDataByClientId(Guid clientId, long reportToDateTime);

        void DeleteOldData(int retentionTime);
    }
}
