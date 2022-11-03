using AMS.PageSpeed.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.ReportAutomation.ReportData.PageSpeed
{
    public class PageSpeedInsightsData : ReportDataBase
    {        
        public CategoryScore Desktop { get; set; }
        public CategoryScore Mobile { get; set; }
    }

    public class CategoryScore
    {
        public LoadingExperience LoadingExperience { get; set; }
        public LoadingExperience OriginLoadingExperience { get; set; }
        public LighthouseJson LighthouseJson { get; set; }
        //i.e. Basically, we need this serie to draw a chart. i.e. Yesterday, the day before yesterday, etc.
        public List<LighthouseHistoryAuditsJson> LighthouseHistory { get; set; }
        public double Score { get; set; }
        public long Time { get; set; }
    }

    public class LighthouseJsonExludedAudits : LighthouseJson
    {
        /// <summary>
        /// Clear AuditsJson after done all its related business (happended in base class's implementation).
        /// For example, in case we do not want to receive the detailed audits to keep the LighthouseJson small with only-neccessarily-used data, we can also do this.
        /// </summary>
        public new dynamic AuditsJson
        {
            get => base.AuditsJson;
            set
            {
                base.AuditsJson = value;
                //Clear the value after done all its related business (happended in base class's implementation)
                _auditsJson = null;
            }
        }
    }

    public class LighthouseJson
    {
        public string LightHouseVersion { get; set; }

        public string UserAgent { get; set; }

        public List<AuditRef> AuditRefs { get; set; }

        private dynamic _auditCategoryGroups;
        public dynamic AuditCategoryGroups {
            get
            {
                return _auditCategoryGroups;
            }
            set
            {
                _auditCategoryGroups = value;
                setvalAuditGroups();
                //setvalScoreDistributionByGroup();
            }
        }
        [JsonIgnore]
        public Dictionary<string, AuditCategoryGroup> AuditGroups { get;  private set; }
        private void setvalAuditGroups()
        {
            if (AuditCategoryGroups == null)
            {
                AuditGroups = null;
                return;
            }

            Newtonsoft.Json.Linq.JObject attributesAsJObject = AuditCategoryGroups;
            AuditGroups = attributesAsJObject.ToObject<Dictionary<string, AuditCategoryGroup>>();
        }

        protected dynamic _auditsJson;
        public dynamic AuditsJson
        {
            get
            {
                return _auditsJson;
            }
            set
            {
                _auditsJson = value;
                setvalPerformanceAudits();
                setvalPerformanceMetrics();
                setvalScoreDistribution();
                //setvalScoreDistributionByGroup();

                //
                //Optimize performance by not storing too much data that is never used
                //
                //We do not want most of the items with "scoreDisplayMode"="informative"
                //var uselessInformativeProperties = new string[] {
                //    "long-tasks",
                //    "network-rtt",
                //    "resource-summary",
                //    "user-timings",
                //    "network-server-latency",
                //    "critical-request-chains",
                //    "main-thread-tasks",
                //    "layout-shift-elements",
                //    "diagnostics",
                //    "final-screenshot",
                //    "largest-contentful-paint-element",
                //    "full-page-screenshot"
                //};
                //foreach(string prop in uselessInformativeProperties)
                //{
                //    if (_auditsJson[prop] != null) _auditsJson[prop] = null;
                //}
                Newtonsoft.Json.Linq.JObject attributesAsJObject = _auditsJson;
                var audits = attributesAsJObject.ToObject<Dictionary<string, dynamic>>();
                foreach (var key in audits.Keys)
                {
                    if (_auditsJson[key] != null && _auditsJson[key]["scoreDisplayMode"] != null)
                    {
                        //We do not want most of the items with "scoreDisplayMode"="informative"
                        var usedProperties = new string[] {
                            //Used in AuditsPerf class
                            "network-requests",
                            "screenshot-thumbnails",
                            //Used for AuditsPerfMetrics class
                            "metrics"
                        };
                        if (!usedProperties.Contains(key) && _auditsJson[key]["scoreDisplayMode"] == "informative")
                        {
                            _auditsJson[key] = null;
                            continue;
                        }

                        //We do not want the items with "scoreDisplayMode"="manual" or ="notApplicable" or ="not_applicable" 
                        if (_auditsJson[key]["scoreDisplayMode"] == "manual" || _auditsJson[key]["scoreDisplayMode"] == "notApplicable" || _auditsJson[key]["scoreDisplayMode"] == "not_applicable")
                        {
                            _auditsJson[key] = null;
                            continue;
                        }
                    }                    
                }
            }
        }
        [JsonIgnore]
        public AuditsPerf PerformanceAudits { get; private set; }
        private void setvalPerformanceAudits()
        {
            if (_auditsJson == null)
            {
                PerformanceAudits = null;
                return;
            }

            string json = _auditsJson.ToString();
            if (json.StartsWith("{{") && json.EndsWith("{}}")) json = json.Substring(1, json.Length - 2);
            PerformanceAudits = JsonConvert.DeserializeObject<AuditsPerf>(json);
        }
        [JsonIgnore]
        public AuditsPerfMetrics PerformanceMetrics { get; private set; }
        private void setvalPerformanceMetrics()
        {
            if (_auditsJson == null
                || _auditsJson.metrics == null
                || _auditsJson.metrics.details == null
                || _auditsJson.metrics.details.items == null)
            {
                PerformanceMetrics = null;
                return;
            }

            string json = _auditsJson.metrics.details.items[0].ToString();
            if (json.StartsWith("{{") && json.EndsWith("{}}")) json = json.Substring(1, json.Length - 2);
            PerformanceMetrics = JsonConvert.DeserializeObject<AuditsPerfMetrics>(json);
        }
        [JsonIgnore]
        public Dictionary<string, uint> ScoreDistribution { get; private set; }
        private void setvalScoreDistribution()
        {
            if (_auditsJson == null)
            {
                ScoreDistribution = null;
                return;
            }

            Newtonsoft.Json.Linq.JObject attributesAsJObject = _auditsJson;
            var audits = attributesAsJObject.ToObject<Dictionary<string, AuditInfo>>();
            var retDic = new Dictionary<string, uint>();
            retDic.Add("90-100", (uint)audits.Count(x => x.Value != null && x.Value.Score != null && double.Parse(x.Value.Score) >= 0.9));
            retDic.Add("50-89", (uint)audits.Count(x => x.Value != null && x.Value.Score != null && double.Parse(x.Value.Score) < 0.9 && double.Parse(x.Value.Score) >= 0.5));
            retDic.Add("0-49", (uint)audits.Count(x => x.Value != null && x.Value.Score != null && double.Parse(x.Value.Score) < 0.5));
            ScoreDistribution = retDic;
        }
        //[JsonIgnore]
        //public Dictionary<string, Dictionary<string, uint>> ScoreDistributionByGroup { get; private set; }
        //private void setvalScoreDistributionByGroup()
        //{
        //    if (AuditGroups == null || _auditsJson == null)
        //    {
        //        ScoreDistributionByGroup = null;
        //        return;
        //    }

        //    var auditGroups = AuditGroups.Keys;
        //    var retValue = new Dictionary<string, Dictionary<string, uint>>();
        //    foreach (var auditGroup in auditGroups)
        //    {
        //        var auditRefsInGroup = AuditRefs.Where(x => x.Group == auditGroup).ToList();
        //        var auditsInGroup = new List<dynamic>();
        //        foreach (var auditRef in auditRefsInGroup)
        //        {
        //            if (_auditsJson[auditRef.Id] != null)
        //            {
        //                auditsInGroup.Add(_auditsJson[auditRef.Id]);
        //            }
        //        }

        //        var retDic = new Dictionary<string, uint>();
        //        retDic.Add("90-100", (uint)auditsInGroup.Count(x => x.score != null && x.score >= 0.9));
        //        retDic.Add("50-89", (uint)auditsInGroup.Count(x => x.score != null && x.score < 0.9 && x.score >= 0.5));
        //        retDic.Add("0-49", (uint)auditsInGroup.Count(x => x.score != null && x.score < 0.5));
        //        retValue.Add(auditGroup, retDic);
        //    }
        //    ScoreDistributionByGroup = retValue;
        //}
    }

    public class LighthouseHistoryAuditsJson
    {
        public DateTime Utc { get; set; }
        public double Score { get; set; }
        [JsonProperty("Perf")]
        public AuditsPerf PerformanceAudits { get; set; }
        [JsonProperty("Metrics")]
        public AuditsPerfMetrics PerformanceMetrics { get; set; }
        [JsonProperty("ScoreDist")]
        public Dictionary<string, uint> ScoreDistribution { get; set; }
    }

    public class Constants
    {
        public const string Desktop = "desktop";
        public const string Mobile = "mobile";
        public const string Category_Performance = "perf";
        public const string Category_BestPractices = "best";
        public const string Category_Accessibility = "a11y";
        public const string Category_SEO = "seo";
        public const string Category_PWA = "pwa";
    }
}