using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.ReportData.GoogleSearch
{
    public class GoogleSearchData : ReportDataBase
    {
        public string Keyword { get; set; }
        public List<GoogleSearchModel> Data { get; set; } = new List<GoogleSearchModel>();
    }

    public class GoogleSearchModel
    {
        public string Date { get; set; }
        public string Page { get; set; }
        public string Device { get; set; }
        public double? Click { get; set; }
        public double? Impression { get; set; }
    }

    public class GoogleSearchSitemapModel
    {
        public long? DataTime { get; set; }
        public List<GoogleSearchSitemapItemModel> Items { get; set; }
    }

    public class GoogleSearchSitemapItemModel
    {
        public string Path { get; set; }
        public string Type { get; set; }
        public long? Warning { get; set; }
        public long? Error { get; set; }
        public DateTime? LastDownloaded { get; set; }
        public DateTime? LastSubmitted { get; set; }
    }
}
