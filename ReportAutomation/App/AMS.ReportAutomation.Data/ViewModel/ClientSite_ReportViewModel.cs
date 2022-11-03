using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.ViewModel
{
    public class ClientSite_ReportViewModel
    {
        public Guid Id { get; set; }
        public Guid? ClientSiteId { get; set; }
        public string ClientSiteName { get; set; }
        public bool? IsActive { get; set; }
        public Dictionary<string, string> ToolIds { get; set; } = new Dictionary<string, string>();
        public string ToolIdString { get; set; }
        public string ReportName { get; set; }
    }
}
