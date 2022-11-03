using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.ViewModel
{
    public class Report_ClientSiteViewModel
    {
        public Guid Id { get; set; }
        public int? ClientId { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public string SiteUrl { get; set; }
        public string CrawlerConfig { get; set; }
        
        public List<ClientSite_ReportViewModel> Report_ClientSiteViewModels { get; set; }
    }

    /// <summary>
    /// Site to perform ScreamingFrog spider crawling.
    /// </summary>
    public class ScreamingFrogCheckSite
    {
        public Guid ClientSiteId { get; set; }
        public ScreamingFrogCrawlerConfig CrawlerConfig { get; set; }
    }
    public class ScreamingFrogCrawlerConfig
    {
        public string ScreamingFrogUrl;
        //public string ScreamingFrogIgnoreRobotTxt;
        [JsonIgnore]
        public string SfUrlWithoutProtocol
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ScreamingFrogUrl)) return ScreamingFrogUrl;
                return (ScreamingFrogUrl.EndsWith("/") ? ScreamingFrogUrl.Substring(0, ScreamingFrogUrl.Length - 1) : ScreamingFrogUrl).Replace("https://", string.Empty).Replace("http://", string.Empty);
            }
        }
        //[JsonIgnore]
        //public bool IsRobotTxtIgnored
        //{
        //    get
        //    {
        //        if (string.IsNullOrWhiteSpace(ScreamingFrogIgnoreRobotTxt)) return false;
        //        if (ScreamingFrogIgnoreRobotTxt.Trim() == "0") return false;
        //        if (ScreamingFrogIgnoreRobotTxt.Trim() == "1") return true;

        //        bool retValue;
        //        if (bool.TryParse(ScreamingFrogIgnoreRobotTxt.Trim().ToLower(), out retValue)) return retValue;
        //        else return false;
        //    }
        //}
    }

    /// <summary>
    /// Site to perform PageSpeedInsights checks.
    /// </summary>
    public class PSICheckSite
    {
        public Guid ClientSiteId { get; set; }
        public PSICrawlerConfig CrawlerConfig { get; set; }
    }
    public class PSICrawlerConfig
    {
        public string PsiUrl;
        public string PsiSitemapUrl;
        public string PsiUrlDeepCrawl;
        [JsonIgnore]
        public bool IsPsiUrlDeepCrawl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(PsiUrlDeepCrawl)) return false;
                if (PsiUrlDeepCrawl.Trim() == "0") return false;
                if (PsiUrlDeepCrawl.Trim() == "1") return true;

                bool retValue;
                if (bool.TryParse(PsiUrlDeepCrawl.Trim().ToLower(), out retValue)) return retValue;
                else return false;
            }
        }
    }

    public class GoogleCrawlerConfig
    {
        public string GoogleServiceAccSecretJsonFile;
        public string GoogleOauthAccEmail;
        public string GoogleOauthAccSecretJsonFile;
    }

    /// <summary>
    /// Site to get GoogleAnalytics data.
    /// </summary>
    public class GoogleAnalyticsSite
    {
        public Guid ClientSiteId { get; set; }
        public string SiteUrl { get; set; }
        public GoogleAnalyticsCrawlerConfig CrawlerConfig { get; set; }
    }
    public class GoogleAnalyticsCrawlerConfig : GoogleCrawlerConfig
    {
        /// <summary>
        /// Google Analytics ViewId
        /// </summary>
        public string GAViewId;
    }

    public class GoogleSearchSite
    {
        public Guid ClientSiteId { get; set; }
        public string SiteUrl { get; set; }
        public GoogleSearchCrawlerConfig CrawlerConfig { get; set; }
    }
    public class GoogleSearchCrawlerConfig : GoogleCrawlerConfig
    {
        public string GCSearchUrl;
        public string GCDimensions;
    }
}
