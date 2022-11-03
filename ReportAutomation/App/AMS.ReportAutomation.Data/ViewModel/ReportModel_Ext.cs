namespace AMS.ReportAutomation.Data.ViewModel
{
    public partial class ReportModel
    {
        public string ClientSiteUrlWithoutProtocol {
            get
            {
                if (string.IsNullOrWhiteSpace(ClientSiteUrl)) return ClientSiteUrl;
                return (ClientSiteUrl.EndsWith("/") ? ClientSiteUrl.Substring(0, ClientSiteUrl.Length - 1) : ClientSiteUrl).Replace("https://", string.Empty).Replace("http://", string.Empty);
            }
        }
    }
}