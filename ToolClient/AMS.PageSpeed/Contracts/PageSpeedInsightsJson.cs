namespace AMS.PageSpeed.Contracts
{
    //Ref: https://developers.google.com/speed/docs/insights/v5/reference/pagespeedapi/runpagespeed (Response)
    public class PageSpeedInsightsJson
    {
        //public string captchaResult { get; set; }
        //public string kind { get; set; }
        public string Id { get; set; }
        public LoadingExperience LoadingExperience { get; set; }
        public LoadingExperience OriginLoadingExperience { get; set; }
        public LighthouseResult LighthouseResult { get; set; }
        public string AnalysisUTCTimestamp { get; set; }
        //public Version version { get; set; }
    }

    ///// <summary>
    ///// The version of PageSpeed used to generate these results.
    ///// </summary>
    //public class Version
    //{
    //    /// <summary>
    //    /// The major version number of PageSpeed used to generate these results.
    //    /// </summary>
    //    public string Major { get; set; }
    //    /// <summary>
    //    /// The minor version number of PageSpeed used to generate these results.
    //    /// </summary>
    //    public string Minor { get; set; }
    //}
}
