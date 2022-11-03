using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.ReportData.GoogleAnalytic
{
    public class GoogleAnalyticData : ReportDataBase
    {
        public Dictionary<string, List<AudienceOverview>> AudienceOverviews { get; set; } = new Dictionary<string, List<AudienceOverview>>();

        //public Dictionary<string, List<DateUserPair>> ActiveUsers { get; set; } = new Dictionary<string, List<DateUserPair>>();

        public Dictionary<string, List<TrafficItem>> TrafficByDevice { get; set; } = new Dictionary<string, List<TrafficItem>>();
        public Dictionary<string, List<TrafficItem>> TrafficByBrowser { get; set; } = new Dictionary<string, List<TrafficItem>>();
        public Dictionary<string, List<TrafficItem>> TrafficByOS { get; set; } = new Dictionary<string, List<TrafficItem>>();

        public Dictionary<string, List<TrafficItem>> ChannelGrouping { get; set; } = new Dictionary<string, List<TrafficItem>>();

    }

    public class AudienceOverview
    {
        public DateTime Date { get; set; }
        public double? Users { get; set; }
        public double? NewUsers { get; set; }
        public double? Sessions { get; set; }
        public double? SessionsPerUser { get; set; }
        public double? PageViews { get; set; }
        public double? PageSession { get; set; }
        public double? AvgSessionDuration { get; set; }
        public double? BounceRate { get; set; }
    }

    public class TrafficItem
    {
        public DateTime Date { get; set; }
        public double Users { get; set; }
        public double? SessionsPerUser { get; set; }
        public double? BounceRate { get; set; }
    }

    //public class DateUserPair
    //{
    //    public DateTime Date { get; set; }
    //    public int User { get; set; }
    //}
}
