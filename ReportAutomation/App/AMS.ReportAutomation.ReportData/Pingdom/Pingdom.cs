using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.ReportData.Pingdom
{
    public class UptimeAndResponse : ReportDataBase
    {
        //i.e. Last 7 days, last 30 days
        public List<KeyValuePair<string, SummaryUptime>> Summary { get; set; } = new List<KeyValuePair<string, SummaryUptime>>();
        //i.e. Basically, we need this serie to draw a chart. i.e. Yesterday, the day before yesterday, etc.
        public List<PerformanceUptime> Performance { get; set; } = new List<PerformanceUptime>();
        //i.e. Remarks taken from Recommendation Bank
        public List<KeyValuePair<string, string>> Remarks { get; set; } = new List<KeyValuePair<string, string>>();
    }

    public class SummaryUptime
    {
        public decimal UptimePercentage { get; set; }
        public int Outages { get; set; }
        public long DownTimeSec { get; set; }
        public long AvgResponseMillisec { get; set; }
    }

    public class PerformanceUptime
    {
        public DateTime StartTimeUtc { get; set; }
        [JsonIgnore]
        public DateTime StartDateUtc {
            get {
                return new DateTime(StartTimeUtc.Year, StartTimeUtc.Month, StartTimeUtc.Day);
            }
        }
        public long AvgResponseMillisec { get; set; }
        public long UpTimeSec { get; set; }
        public long DownTimeSec { get; set; }
        public long UnmonitoredSec { get; set; }
    }

    public class PerformanceUptimeSimple
    {
        public DateTime Utc { get; set; }
        public double AvgRespSec { get; set; }
        public long DownSec { get; set; }
    }
}
