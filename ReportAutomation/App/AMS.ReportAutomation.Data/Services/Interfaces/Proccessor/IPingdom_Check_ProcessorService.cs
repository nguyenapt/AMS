using AMS.ReportAutomation.ReportData.Pingdom;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IPingdom_Check_ProcessorService
    {
        void Process();

        UptimeAndResponse GetPingdomReportDataByCheckId(long checkId, long reportToDateTime);
        Dictionary<long, UptimeAndResponse> GetPingdomReportDataByCheckIds(List<long> checkIds, long reportToDateTime);

        void DeleteOldData(int retentionTime);
    }
}
