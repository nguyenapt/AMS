using Newtonsoft.Json;
using System.Collections.Generic;

namespace AMS.ReportAutomation.ReportData.ScreamingFrog
{
    public class ScreamingFrogData : ReportDataBase
    {
        public ContentDistribution ContentTypes { get; set; }
        public Indexability Indexability { get; set; }
        public HttpStatusCodes HttpStatusCodes { get; set; }
        public List<UrlWithDetails> TopLargestResources { get; set; }
        public Dictionary<int, long> CrawlDepth { get; set; }
        public List<UrlWithDetails> TopUrlsWithInlinks { get; set; }
        public List<UrlWithDetails> TopUrlsWithWords { get; set; }
        public ContentSEO ContentSEO { get; set; }
    }

    public class ContentDistribution
    {
        /// <summary>
        /// Count all crawled Urls.
        /// </summary>
        public long CountAll { get; set; }
        /// <summary>
        /// Count the number of HTML Pages.
        /// </summary>
        public long Html { get; set; }
        /// <summary>
        /// Count the number of CSS files.
        /// </summary>
        public long Css { get; set; }
        /// <summary>
        /// Count the number of script files.
        /// </summary>
        public long Script { get; set; }
        /// <summary>
        /// Count the number of Images.
        /// </summary>
        public long Img { get; set; }
        /// <summary>
        /// Count the number of other contents.
        /// </summary>
        [JsonIgnore]
        public long OtherContentTypes
        {
            get
            {
                return CountAll - Html - Css - Script - Img;
            }
        }
    }

    public class Indexability
    {
        public long CountIndexable { get; set; }
        public long CountNonIndexable { get; set; }
        public Dictionary<string, long> NonIndexableReasons { get; set; }
        //public List<UrlNonIndexable> TopNonIndexableUrls { get; set; }
    }

    public class HttpStatusCodes
    {
        public long CountSuccess { get; set; }
        public long CountRedirect { get; set; }
        public long CountNotOk { get; set; }
        public List<UrlStatusCodeNotOk> TopUrlsNotOk { get; set; }
    }

    public class ContentSEOSimple
    {
        public long CountUrls { get; set; }
        public long CountTitle1Missing { get; set; }
        public long CountTitle1Duplicate { get; set; }
        public long CountTitle1Over60Chars { get; set; }
        public long CountTitle1Below30Chars { get; set; }
        //public long CountTitle1Over554Pixels { get; set; }
        //public long CountTitle1Below200Pixels { get; set; }
        public long CountMetaDescMissing { get; set; }
        public long CountMetaDescDuplicate { get; set; }
        public long CountMetaDescOver155Chars { get; set; }
        public long CountMetaDescBelow70Chars { get; set; }
        //public long CountMetaDescOver1005Pixels { get; set; }
        //public long CountMetaDescBelow400Pixels { get; set; }
        public long CountH11Missing { get; set; }
        public long CountH11Duplicate { get; set; }
        public long CountH11Over70Chars { get; set; }

        public long CountH21Missing { get; set; }
        public long CountH21Duplicate { get; set; }
        public long CountH21Over70Chars { get; set; }
    }

    public class ContentSEO : ContentSEOSimple
    {
        public List<UrlWithContentInfo> Top20UrlsTitle1Missing { get; set; }
        public List<UrlWithContentInfo> Top10UrlsTitle1Duplicate { get; set; }
        public List<UrlWithContentInfo> Top10UrlsTitle1Over60Chars { get; set; }
        public List<UrlWithContentInfo> Top10UrlsTitle1Below30Chars { get; set; }
        public List<UrlWithContentInfo> Top10UrlsMetaDescMissing { get; set; }
        public List<UrlWithContentInfo> Top10UrlsMetaDescDuplicate { get; set; }
        public List<UrlWithContentInfo> Top10UrlsMetaDescOver155Chars { get; set; }
        public List<UrlWithContentInfo> Top10UrlsMetaDescBelow70Chars { get; set; }
        public List<UrlWithContentInfo> Top10UrlsH11Missing { get; set; }
        public List<UrlWithContentInfo> Top10UrlsH11Over70Chars { get; set; }
        public List<UrlWithContentInfo> Top10UrlsH21Missing { get; set; }
        public List<UrlWithContentInfo> Top10UrlsH21Over70Chars { get; set; }
    }

    public class UrlInfoBase
    {
        /// <summary>
        /// Address.
        /// </summary>
        public string Url { get; set; }
    }

    public class UrlWithDetails : UrlInfoBase
    {
        public string Title1;

        /// <summary>
        /// Content type.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Size (bytes).
        /// </summary>
        public double Bytes { get; set; }
        /// <summary>
        /// Response time (seconds).
        /// </summary>
        public double? RespSec { get; set; }

        public long? Inlinks;
        public long? UniqueInlinks;
        //public double? PercentageOfTotal;
        public long? Outlinks;
        public long? UniqueOutlinks;
        public long? ExtOutlinks;
        public long? UniqueExtOutlinks;

        public long? WordCount;
        public double? TextRatio;
    }
    public class UrlWithContentInfo : UrlInfoBase
    {
        public string Title1;
        public long? Title1Length;
        //public long? Title1PixelWidth;
        public string MetaDesc1;
        public long? MetaDesc1Length;
        //public long? MetaDesc1PixelWidth;
        //public string MetaKeyword1;
        //public long? MetaKeywords1Length;
        public string H11;
        public long? H11Length;
        //public string H12;
        //public long? H12Length;
        public string H21;
        public long? H21Length;
        //public string H22;
        //public long? H22Length;
    }

    //public class UrlNonIndexable : UrlInfoBase
    //{
    //    /// <summary>
    //    /// Reason why indexing failed.
    //    /// </summary>
    //    public string Why { get; set; }
    //}

    public class UrlStatusCodeNotOk : UrlInfoBase
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// HTTP status code description.
        /// </summary>
        public string Desc { get; set; }
    }
}