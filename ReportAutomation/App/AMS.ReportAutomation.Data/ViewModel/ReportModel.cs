using System;

namespace AMS.ReportAutomation.Data.ViewModel
{
    public partial class ReportModel
    {
        public Guid Id { get; set; }
        public Guid? ClientSiteId { get; set; }
        public string ClientSiteName { get; set; }
        public string ClientSiteLogo { get; set; }
        public string ClientSiteBrandName { get; set; }
        public string ClientSiteUrl { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientLogoUrl { get; set; }
        public bool? IsActive { get; set; }
        public string ReportName { get; set; }
        public string ToolIds { get; set; }
    }
}
