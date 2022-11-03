namespace AMS.ReportAutomation.Data.DataContext
{
    public partial class Report_ClientSites
    {
        public string SiteUrlWithoutProtocol {
            get
            {
                if (string.IsNullOrWhiteSpace(SiteUrl)) return SiteUrl;
                return (SiteUrl.EndsWith("/") ? SiteUrl.Substring(0, SiteUrl.Length - 1) : SiteUrl).Replace("https://", string.Empty).Replace("http://", string.Empty);
            }
        }
    }
}