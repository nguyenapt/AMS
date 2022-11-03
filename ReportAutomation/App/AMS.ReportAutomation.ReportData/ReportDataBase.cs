using System;

namespace AMS.ReportAutomation.ReportData
{
    public class ReportDataBase
    {
        public ReportDataBase()
        {
            ConsolidatedTimeUtc = DateTime.UtcNow;
        }

        public DateTime ConsolidatedTimeUtc { get; set; }
    }
}
