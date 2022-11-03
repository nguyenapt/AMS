using System.ComponentModel;

namespace AMS.ReportAutomation.ReportData
{
    public enum ReportSection
    {
        [Description("Overview")]
        Overview = 0,
        [Description("Pingdom Check")]
        PingdomCheck = 1,
        [Description("Client Jira service desk")]
        ClientJiraServiceDesk = 2,
        [Description("PSI Performance")]
        PSIPerformance = 3,
        [Description("PSI Accessibility")]
        PSIAccessibility = 4,
        [Description("PSI SEO")]
        PSISeo = 5,
        [Description("PSI Pwa")]
        PSIPwa = 6
    }
}
