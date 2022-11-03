using System;

namespace AMS.ReportAutomation.Data.ViewModel
{
    public class Report_CustomContentViewModel
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public int ReportDays { get; set; }
        public long ReportToUnixTimestamp { get; set; }
        public string CustomReportContent { get; set; }
        public int UnderReportSection { get; set; }
    }
}
