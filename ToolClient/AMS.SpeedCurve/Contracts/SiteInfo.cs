using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SpeedCurve.Contracts
{
    public class SiteInfo
    {
        public List<SiteDetail> Sites { get; set; }
    }

    public class SiteDetail
    {
        [JsonProperty("site_id")]
        public string SiteId { get; set; }

        public string Name { get; set; }

        public string Checks_Scheduled { get; set; }

        public List<Urls> Urls { get; set; }

    }

    public class Urls
    {
        public string Url_Id { get; set; }
        public string Label { get; set; }
        public string Url { get; set; }

    }
}
