using Newtonsoft.Json;
using System.Collections.Generic;

namespace AMS.PageSpeed.Contracts
{
    /// <summary>
    /// Lighthouse response for the audit url as an object.
    /// </summary>
    public class LighthouseResult
    {
        /// <summary>
        /// The original requested url.
        /// </summary>
        public string RequestedUrl { get; set; }
        /// <summary>
        /// The final resolved url that was audited.
        /// </summary>
        public string FinalUrl { get; set; }
        /// <summary>
        /// The lighthouse version that was used to generate this LHR.
        /// </summary>
        public string LightHouseVersion { get; set; }
        /// <summary>
        /// The user agent that was used to run this LHR.
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// The time that this run was fetched.
        /// </summary>
        public string FetchTime { get; set; }
        ///// <summary>
        ///// Environment settings that were used when making this LHR.
        ///// </summary>
        //public object environment { get; set; }
        /// <summary>
        /// List of all run warnings in the LHR. Will always output to at least `[]`.
        /// </summary>
        public string[] RunWarnings { get; set; }
        /// <summary>
        /// Map of audits in the LHR.
        /// </summary>
        public dynamic Audits { get; set; }
        public Categories Categories { get; set; }
        /// <summary>
        /// The configuration settings for this LHR.
        /// </summary>
        public ConfigSettings ConfigSettings { get; set; }
        public dynamic CategoryGroups { get; set; }
        /// <summary>
        /// Object containing the code + message of any thrown runtime errors.
        /// </summary>
        public RuntimeError RuntimeError { get; set; }
        ///// <summary>
        ///// Timing information for this LHR. Timing.total: the total duration of Lighthouse's run.
        ///// </summary>
        //public object timing { get; set; }
        ///// <summary>
        ///// The internationalization strings that are required to render the LHR.
        ///// </summary>
        //public object i18n { get; set; }
    }

    /// <summary>
    /// Object containing the code + message of any thrown runtime errors.
    /// </summary>
    public class RuntimeError
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class ConfigSettings
    {
        /// <summary>
        /// The form factor the emulation should use. Acceptable values are: "UNKNOWN_FORM_FACTOR", "desktop", "mobile", "none".
        /// </summary>
        public string EmulatedFormFactor { get; set; }
        /// <summary>
        /// The locale setting. i.e. "en-US".
        /// </summary>
        public string Locale { get; set; }
        //public string[] OnlyCategories { get; set; }
        //public string Channel { get; set; }
    }

    public class AuditsPerf
    {
        [JsonProperty("first-contentful-paint")]
        public AuditInfo Metrics_first_contentful_paint;	//https://web.dev/performance-scoring/ (Lighthouse 6) weigtht-15	Group-metrics
        [JsonProperty("speed-index")]
        public AuditInfo Metrics_speed_index;	//15	metrics
        [JsonProperty("largest-contentful-paint")]
        public AuditInfo Metrics_largest_contentful_paint;	//25	metrics
        [JsonProperty("interactive")]
        public AuditInfo Metrics_interactive;	//15	metrics
        [JsonProperty("total-blocking-time")]
        public AuditInfo Metrics_total_blocking_time;	//25	metrics
        [JsonProperty("cumulative-layout-shift")]
        public AuditInfo Metrics_cumulative_layout_shift;	//5	metrics

        //https://web.dev/performance-scoring/ (Lighthouse 5)
        [JsonProperty("first-meaningful-paint")]
        public AuditInfo Perf_first_meaningful_paint;
        [JsonProperty("first-cpu-idle")]
        public AuditInfo Perf_first_cpu_idle;
        //[JsonProperty("first-contentful-paint")]
        //public AuditInfo Perf_first_contentful_paint;

        //[JsonProperty("estimated-input-latency")]
        //public AuditInfo Perf_estimated_input_latency;
        //[JsonProperty("max-potential-fid")]
        //public AuditInfo Perf_max_potential_first_input_delay;

        [JsonProperty("network-requests")]
        public NetworkRequestsInfo NetworkRequests;

        [JsonProperty("screenshot-thumbnails")]
        public AuditFilmstripInfo ScreenshotThumbnails;
    }

    public class AuditsPerfMetrics
    {
        /* Example:
              {
              "observedFirstPaintTs": 871283277707,
              "observedFirstVisualChangeTs": 871283263602,
              "observedTimeOriginTs": 871278158602,
              "observedTraceEnd": 10296,
              "observedCumulativeLayoutShift": 0.0062419478737997264,
              "observedFirstContentfulPaint": 5119,
              "observedFirstPaint": 5119,
              "totalBlockingTime": 84,
              "observedTimeOrigin": 0,
              "observedSpeedIndexTs": 871283367770,
              "firstMeaningfulPaint": 710,
              "observedFirstContentfulPaintTs": 871283277707,
              "observedNavigationStartTs": 871278158602,
              "speedIndex": 4079,
              "estimatedInputLatency": 20,
              "observedFirstMeaningfulPaint": 5119,
              "observedFirstMeaningfulPaintTs": 871283277707,
              "interactive": 3126,
              "observedLargestContentfulPaintTs": 871283354975,
              "cumulativeLayoutShift": 0.0062419478737997264,
              "observedLastVisualChangeTs": 871287246602,
              "observedSpeedIndex": 5209,
              "observedFirstVisualChange": 5105,
              "observedDomContentLoaded": 7037,
              "firstContentfulPaint": 710,
              "observedDomContentLoadedTs": 871285195795,
              "observedLoad": 8241,
              "firstCPUIdle": 2773,
              "observedLargestContentfulPaint": 5196,
              "observedTraceEndTs": 871288454136,
              "observedLoadTs": 871286399295,
              "maxPotentialFID": 149,
              "largestContentfulPaint": 1928,
              "observedNavigationStart": 0,
              "observedLastVisualChange": 9088
              }
         */

        /*
         * Page Load & Rendering time
         */
        //[JsonProperty("firstCPUIdle")]
        //public double FirstCPUIdle;
        //[JsonProperty("interactive")]
        //public double Interactive;

        [JsonProperty("observedFirstVisualChange")]
        public double ObservedFirstVisualChange;
        [JsonProperty("observedLastVisualChange")]
        public double ObservedLastVisualChange;

        [JsonProperty("observedDomContentLoaded")]
        public double ObservedDomContentLoaded;
        [JsonProperty("observedLoad")]
        public double ObservedLoad;
        [JsonProperty("observedTraceEnd")]
        public double ObservedTraceEnd;

        /*
         * Page Rendering time: Take the painting measurements from AuditsPerf class instead
         */
    }

    //public class AuditsNonPerf
    //{
    //    [JsonProperty("viewport")]
    //    public AuditInfo SEO_Mobile_viewport;	//1	seo-mobile
    //    [JsonProperty("meta-description")]
    //    public AuditInfo SEO_Content_meta_description;	//1	seo-content
    //    [JsonProperty("http-status-code")]
    //    public AuditInfo SEO_Crawl_http_status_code;	//1	seo-crawl
    //    [JsonProperty("link-text")]
    //    public AuditInfo SEO_Content_link_text;	//1	seo-content
    //    [JsonProperty("crawlable-anchors")]
    //    public AuditInfo SEO_Crawl_crawlable_anchors;	//1	seo-crawl
    //    [JsonProperty("is-crawlable")]
    //    public AuditInfo SEO_Crawl_is_crawlable;	//1	seo-crawl
    //    [JsonProperty("robots-txt")]
    //    public AuditInfo SEO_Crawl_robots_txt;	//1	seo-crawl
    //    [JsonProperty("hreflang")]
    //    public AuditInfo SEO_Content_hreflang;	//1	seo-content
    //    [JsonProperty("canonical")]
    //    public AuditInfo SEO_Content_canonical;	//1	seo-content
    //    [JsonProperty("plugins")]
    //    public AuditInfo SEO_Content_plugins;	//1	seo-content

    //    [JsonProperty("document-title")]
    //    public AuditInfo SEO_Content_A11y_document_title;   //1	seo-content, 3	a11y-names-labels
    //    [JsonProperty("image-alt")]
    //    public AuditInfo SEO_Content_A11y_image_alt;	//1	seo-content, 10	a11y-names-labels

    //    [JsonProperty("aria-allowed-attr")]
    //    public AuditInfo A11y_aria_allowed_attr;	//10	a11y-aria
    //    [JsonProperty("aria-hidden-body")]
    //    public AuditInfo A11y_aria_hidden_body;	//10	a11y-aria
    //    [JsonProperty("aria-hidden-focus")]
    //    public AuditInfo A11y_aria_hidden_focus;	//3	a11y-aria
    //    [JsonProperty("aria-valid-attr-value")]
    //    public AuditInfo A11y_aria_valid_attr_value;	//10	a11y-aria
    //    [JsonProperty("aria-valid-attr")]
    //    public AuditInfo A11y_aria_valid_attr;	//10	a11y-aria
    //    [JsonProperty("button-name")]
    //    public AuditInfo A11y_button_name;	//10	a11y-names-labels
    //    [JsonProperty("bypass")]
    //    public AuditInfo A11y_bypass;	//3	a11y-navigation
    //    [JsonProperty("color-contrast")]
    //    public AuditInfo A11y_color_contrast;	//3	a11y-color-contrast        
    //    [JsonProperty("heading-order")]
    //    public AuditInfo A11y_heading_order;	//2	a11y-navigation
    //    [JsonProperty("html-has-lang")]
    //    public AuditInfo A11y_html_has_lang;	//3	a11y-language
    //    [JsonProperty("html-lang-valid")]
    //    public AuditInfo A11y_html_lang_valid;	//3	a11y-language
    //    [JsonProperty("label")]
    //    public AuditInfo A11y_label;	//10	a11y-names-labels
    //    [JsonProperty("link-name")]
    //    public AuditInfo A11y_link_name;	//3	a11y-names-labels
    //    [JsonProperty("list")]
    //    public AuditInfo A11y_list;	//3	a11y-tables-lists
    //    [JsonProperty("listitem")]
    //    public AuditInfo A11y_listitem;	//3	a11y-tables-lists
    //    [JsonProperty("meta-viewport")]
    //    public AuditInfo A11y_meta_viewport;	//10	a11y-best-practices
    //    [JsonProperty("td-headers-attr")]
    //    public AuditInfo A11y_td_headers_attr;	//3	a11y-tables-lists
    //    [JsonProperty("th-has-data-cells")]
    //    public AuditInfo A11y_th_has_data_cells;	//3	a11y-tables-lists
    //    [JsonProperty("video-caption")]
    //    public AuditInfo A11y_video_caption;	//10	a11y-audio-video
    //    [JsonProperty("video-description")]
    //    public AuditInfo A11y_video_description;	//10	a11y-audio-video

    //    [JsonProperty("is-on-https")]
    //    public AuditInfo BP_is_on_https;	//1	best-practices-trust-safety
    //    [JsonProperty("external-anchors-use-rel-noopener")]
    //    public AuditInfo BP_external_anchors_use_rel_noopener;	//1	best-practices-trust-safety
    //    [JsonProperty("geolocation-on-start")]
    //    public AuditInfo BP_geolocation_on_start;	//1	best-practices-trust-safety
    //    [JsonProperty("notification-on-start")]
    //    public AuditInfo BP_notification_on_start;	//1	best-practices-trust-safety
    //    [JsonProperty("no-vulnerable-libraries")]
    //    public AuditInfo BP_no_vulnerable_libraries;	//1	best-practices-trust-safety
    //    [JsonProperty("password-inputs-can-be-pasted-into")]
    //    public AuditInfo BP_password_inputs_can_be_pasted_into;	//1	best-practices-ux
    //    [JsonProperty("image-aspect-ratio")]
    //    public AuditInfo BP_image_aspect_ratio;	//1	best-practices-ux
    //    [JsonProperty("image-size-responsive")]
    //    public AuditInfo BP_image_size_responsive;	//1	best-practices-ux
    //    [JsonProperty("doctype")]
    //    public AuditInfo BP_doctype;	//1	best-practices-browser-compat
    //    [JsonProperty("charset")]
    //    public AuditInfo BP_charset;	//1	best-practices-browser-compat
    //    [JsonProperty("no-unload-listeners")]
    //    public AuditInfo BP_no_unload_listeners;	//1	best-practices-general
    //    [JsonProperty("appcache-manifest")]
    //    public AuditInfo BP_appcache_manifest;	//1	best-practices-general
    //    [JsonProperty("deprecations")]
    //    public AuditInfo BP_deprecations;	//1	best-practices-general
    //    [JsonProperty("errors-in-console")]
    //    public AuditInfo BP_errors_in_console; //1	best-practices-general
    //}

    public class NetworkRequestsInfo
    {
        /// <summary>
        /// The audit's id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The human readable title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The description of the audit.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Details of the audit.
        /// </summary>
        public NetworkRequestDetails Details { get; set; }
    }

    public class NetworkRequestDetails
    {
        public string Type { get; set; }
        public AuditInfoDetailHeading[] Headings { get; set; }

        /*
         * Example
            [{
              "finished": true,
              "startTime": 9071.261000004597,
              "resourceSize": 584,
              "statusCode": 200,
              "resourceType": "XHR",
              "url": "https://api.livechatinc.com/v3.2/customer/rtm/sjs/322/tpeuxtwy/xhr?bh=7k6pw65aaq&license_id=9134275&t=1608287896620",
              "transferSize": 902,
              "endTime": 9273.80299998913,
              "mimeType": "application/javascript"
            },
            {
              "finished": false,
              "statusCode": -1,
              "resourceType": "XHR",
              "resourceSize": 0,
              "url": "https://api.livechatinc.com/v3.2/customer/rtm/sjs/322/tpeuxtwy/xhr?bh=7k6pw65aaq&license_id=9134275&t=1608287896828",
              "transferSize": 0,
              "startTime": 9278.5080000758171
            }]
         */
        public NetworkRequestItem[] Items { get; set; }
    }

    public class NetworkRequestItem
    {
        public string Url { get; set; }
        public int StatusCode { get; set; }
        public string MimeType { get; set; }
        public string ResourceType { get; set; }
        public double ResourceSize { get; set; }
        public double TransferSize { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public bool Finished { get; set; }
    }

    public class AuditFilmstripInfo
    {
        /// <summary>
        /// The audit's id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The human readable title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The description of the audit.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Details of the audit.
        /// </summary>
        public AuditFilmstrip Details { get; set; }
    }

    public class AuditFilmstrip
    {
        public string Type { get; set; }
        public int Scale { get; set; }
        public FilmstripItem[] Items { get; set; }
    }

    public class FilmstripItem
    {
        /// <summary>
        /// Base64 image
        /// </summary>
        public string Data { get; set; }
        public string Timestamp { get; set; }
        public int Timing { get; set; }
    }

    public class AuditInfo
    {
        /// <summary>
        /// The audit's id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The human readable title.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The description of the audit.
        /// </summary>
        public string Description { get; set; }
        public string Score { get; set; }
        /// <summary>
        /// The enumerated score display mode. Acceptable values are: "SCORE_DISPLAY_MODE_UNSPECIFIED", "binary", "error", "informative", "manual", "not_applicable", "numeric"
        /// </summary>
        public string ScoreDisplayMode { get; set; }
        public double NumericValue { get; set; }
        /// <summary>
        /// The value that should be displayed on the UI for this audit.
        /// </summary>
        public string DisplayValue { get; set; }
        /// <summary>
        /// An explanation of the errors in the audit.
        /// </summary>
        public string Explanation { get; set; }
        /// <summary>
        /// An error message from a thrown error inside the audit.
        /// </summary>
        public string ErrorMessage { get; set; }
        public string[] Warnings { get; set; }
        /// <summary>
        /// Freeform details section of the audit.
        /// </summary>
        public AuditInfoDetail Details { get; set; }
    }
    public class AuditInfoDetail
    {
        public List<AuditInfoDetailHeading> Headings { get; set; }
        public string Type { get; set; }
        public double OverallSavingsMs { get; set; }
        public long OverallSavingsBytes { get; set; }
        public dynamic[] Items { get; set; }
        //Note: There are special cases where more properties are used, such as "chains"; but we can't cover all. In special cases, we will use the parent dynamic object for data.
    }
    public class AuditInfoDetailHeading
    {
        public AuditInfoDetailHeading SubItemsHeading { get; set; }
        public string Key { get; set; }
        public string ItemType { get; set; }
        //public string DisplayUnit { get; set; }
        public string Text { get; set; }
        public string Label { get; set; }
        public string ValueType { get; set; }
        //public double Granularity { get; set; }
    }

    public class Categories
    {
        public CategoryInfo Performance { get; set; }
        public CategoryInfo SEO { get; set; }
        public CategoryInfo Accessibility { get; set; }

        [JsonProperty("best-practices")]
        public CategoryInfo BestPractices { get; set; }
        public CategoryInfo PWA { get; set; }
    }

    public class CategoryInfo
    {
        /// <summary>
        /// The string identifier of the category.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The human-friendly name of the category.
        /// </summary>
        public string Title { get; set; }
        public double Score { get; set; }
        ///// <summary>
        ///// A description for the manual audits in the category.
        ///// </summary>
        //public string ManualDescription { get; set; }
        /// <summary>
        /// A more detailed description of the category and its importance.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// An array of references to all the audit members of this category.
        /// </summary>
        public List<AuditRef> AuditRefs { get; set; }
    }

    /// <summary>
    /// The category group that the audit belongs to.
    /// </summary>
    public class AuditCategoryGroup
    {
        ///// <summary>
        ///// The Id of group that the audit belongs to.
        ///// </summary>
        //public string Id { get; set; }
        /// <summary>
        /// The human-friendly name of the group.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// A more detailed description of the group.
        /// </summary>
        public string Description { get; set; }
    }

    public class AuditRef
    {
        /// <summary>
        /// The audit ref id.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The weight this audit's score has on the overall category score.
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// The category group that the audit belongs to (optional).
        /// </summary>
        public string Group { get; set; }
    }
}
