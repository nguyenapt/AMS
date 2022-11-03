using System;

namespace AMS.ReportAutomation.Data.ViewModel
{
    public class Release_MilestoneViewModel
    {
        public Guid Id { get; set; }
        public Guid ClientSiteId { get; set; }
        public string ReleaseVersion { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Description { get; set; }

    }
}
