using System.Collections.Generic;
using System.IO;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Threading;
using Google.Apis.Util.Store;

namespace AMS.GoogleAnalytic
{
    public class GoogleAnalytic
    {
        private string[] _serviceScopes { get; set; }
        private string _gaViewId { get; set; }

        public GoogleAnalytic(string[] serviceScopes, string gaViewId)
        {
            _serviceScopes = serviceScopes;
            _gaViewId = gaViewId;
        }

        public GoogleAnalytic()
        {
            _serviceScopes = new string[] { AnalyticsReportingService.Scope.AnalyticsReadonly };
        }

        /// <summary>
        /// This method requests Authentcation from a user using Oauth2.  
        /// Documentation https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="oauthSecretJsonFile">Json file where Credentials is stored.</param>
        /// <param name="oauthUser">OAuth email account.</param>
        /// <param name="secretLocation">Where the token file is stored after authenticated.</param>
        /// <returns>DriveService used to make requests against the Drive API.</returns>
        public AnalyticsReportingService AuthenticateOauth(string oauthSecretJsonFile, string oauthUser, bool checkIfTokenFileExisted = true)
        {
            UserCredential credential;
            var oauthTokenLocation = Path.Combine(Path.GetDirectoryName(oauthSecretJsonFile), Path.GetFileNameWithoutExtension(oauthSecretJsonFile), ".token");
            if (checkIfTokenFileExisted)
            {
                var tokenFiles = Directory.GetFiles(oauthTokenLocation, "*.*", SearchOption.TopDirectoryOnly);
                if (tokenFiles == null || tokenFiles.Length < 1)
                {
                    throw new System.Exception($"No OAuth authentication token file found in {oauthTokenLocation}. Please go to SchedulerAdmin program to make sure your CrawlerConfig is correct, and to pre-authenticate and generate token to be used by this service!");
                }
            }

            //using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(oauthSecretJson)))
            using (var stream = new FileStream(oauthSecretJsonFile, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                                                                         _serviceScopes,
                                                                         oauthUser,
                                                                         CancellationToken.None,
                                                                         new FileDataStore(oauthTokenLocation, true)).Result;
            }

            return new AnalyticsReportingService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
        }

        /// <summary>
        /// This method requests Authentcation from a user using ServiceAccount.
        /// </summary>
        /// <param name="serviceAccountSecretJsonFile">Json file where Credentials is stored.</param>
        /// <returns>DriveService used to make requests against the Drive API.</returns>
        public AnalyticsReportingService AuthenticateServiceAccount(string serviceAccountSecretJsonFile)
        {
            var initializer = new BaseClientService.Initializer();
            //using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(serviceAccountSecretJson)))
            using (var stream = new FileStream(serviceAccountSecretJsonFile, FileMode.Open, FileAccess.Read))
            {
                initializer.HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(_serviceScopes);
            }
            return new AnalyticsReportingService(initializer);
        }

        public Dictionary<string, List<Report>> GetAnalyticsData(AnalyticsReportingService analyticsReporting, List<DateRange> dateRanges)
        {
            var dicReport = new Dictionary<string, List<Report>>();

            //Channels: Organic Search, Referral, etc.
            Dimension dimChannel = new Dimension() { Name = "ga:channelGrouping" };
            //Devices: desktop, mobile, table, etc.
            Dimension dimDeviceCategory = new Dimension() { Name = "ga:deviceCategory" };
            //Browsers: Chrome, Firefox, Safari, etc.
            Dimension dimBrowser = new Dimension() { Name = "ga:browser" };
            //Countries: Vietnam, China, etc.
            //Dimension dimCountry = new Dimension() { Name = "ga:country" };
            //OS: Windows, Ubuntu, etc.
            Dimension dimOperatingSystem = new Dimension() { Name = "ga:operatingSystem" };

            Dimension dimDate = new Dimension() { Name = "ga:date" };

            Metric mtcUsers = new Metric() { Expression = "ga:users", Alias = "Users" };
            Metric mtcNewUsers = new Metric() { Expression = "ga:newUsers", Alias = "NewUsers" };
            Metric mtcSessions = new Metric() { Expression = "ga:sessions", Alias = "Session" };
            Metric mtcSessionsPerUser = new Metric() { Expression = "ga:sessionsPerUser", Alias = "SessionsPerUser" };
            Metric mtcPageviews = new Metric() { Expression = "ga:pageviews", Alias = "Pageviews" };
            Metric mtcPageviewsPerSession = new Metric() { Expression = "ga:pageviewsPerSession", Alias = "PageviewsPerSession" };
            Metric mtcAvgSessionDuration = new Metric() { Expression = "ga:avgSessionDuration", Alias = "AvgSessionDuration" };
            Metric mtcBounceRate = new Metric() { Expression = "ga:bounceRate", Alias = "BounceRate" };


            var audienceOverViewDataReport = RequestReportData(analyticsReporting, dateRanges,
                new List<Dimension>{ dimDate },
                new List<Metric>
                {
                    mtcUsers, mtcNewUsers, mtcSessions, mtcSessionsPerUser, mtcPageviews, mtcPageviewsPerSession, mtcAvgSessionDuration, mtcBounceRate
                });
            dicReport.Add(DatasetType.AudienceOverview, audienceOverViewDataReport);

            //Metric dayUsers = new Metric() { Expression = "ga:1dayUsers", Alias = "1dayUsers" };
            //var dayUsersReport = RequestReportData(analyticsReporting, dateRanges,
            //    new List<Dimension> { dimDate },
            //    new List<Metric>
            //    {
            //        dayUsers
            //    });
            //dicReport.Add(DatasetType.Users1Day, dayUsersReport);

            //Metric weekUsers = new Metric() { Expression = "ga:7dayUsers", Alias = "7dayUsers" };
            //var weekUsersReport = RequestReportData(analyticsReporting, dateRanges, 
            //    new List<Dimension> { dimDate },
            //    new List<Metric>
            //    {
            //        weekUsers
            //    });
            //dicReport.Add(DatasetType.Users1Week, weekUsersReport);

            //Metric twoWeekUsers = new Metric() { Expression = "ga:14dayUsers", Alias = "14dayUsers" };
            //var twoWeekUsersReport = RequestReportData(analyticsReporting, dateRanges,
            //    new List<Dimension> { dimDate },
            //    new List<Metric>
            //    {
            //        twoWeekUsers
            //    });
            //dicReport.Add(DatasetType.Users2Weeks, twoWeekUsersReport);

            //Metric fourWeekUsers = new Metric() { Expression = "ga:28dayUsers", Alias = "28dayUsers" };
            //var fourWeekUsersReport = RequestReportData(analyticsReporting, dateRanges,
            //    new List<Dimension> { dimDate },
            //    new List<Metric>
            //    {
            //        fourWeekUsers
            //    });
            //dicReport.Add(DatasetType.Users4Weeks, fourWeekUsersReport);

            var webTraficByDeviceReportData = RequestReportData(analyticsReporting, dateRanges,
                new List<Dimension> { dimDeviceCategory, dimDate },
                new List<Metric>
                {
                    mtcUsers, mtcSessionsPerUser
                });
            dicReport.Add(DatasetType.TrafficByDevice, webTraficByDeviceReportData);

            var browserReportData = RequestReportData(analyticsReporting, dateRanges,
                new List<Dimension> { dimBrowser, dimDate },
                new List<Metric>
                {
                    mtcUsers, mtcSessionsPerUser
                });
            dicReport.Add(DatasetType.TrafficByBrowser, browserReportData);

            var osReportData = RequestReportData(analyticsReporting, dateRanges,
                new List<Dimension> { dimOperatingSystem, dimDate },
                new List<Metric>
                {
                    mtcUsers, mtcSessionsPerUser
                });
            dicReport.Add(DatasetType.TrafficByOS, osReportData);

            //var countryReportData = RequestReportData(analyticsReporting, dateRanges,
            //    new List<Dimension> { dimCountry, dimDate },
            //    new List<Metric>
            //    {
            //        mtcUsers, mtcSessionsPerUser
            //    });
            //dicReport.Add(DatasetType.TrafficByCountry, countryReportData);

            var channelReportData = RequestReportData(analyticsReporting, dateRanges,
                new List<Dimension> { dimChannel, dimDate },
                new List<Metric>
                {
                    mtcUsers, mtcBounceRate
                });
            dicReport.Add(DatasetType.ChannelGrouping, channelReportData);

            return dicReport;
        }

        public List<Report> RequestReportData(AnalyticsReportingService analyticsReporting, List<DateRange> dateRanges, List<Dimension> dimensions = null, List<Metric> metrics = null )
        {
            var requests = new List<ReportRequest>()
            {
                new ReportRequest
                {
                    ViewId = _gaViewId,
                    DateRanges = dateRanges ?? null,
                    Dimensions = dimensions ?? null,
                    Metrics = metrics ?? null
                }
            };

            GetReportsRequest getReport = new GetReportsRequest() { ReportRequests = requests };
            GetReportsResponse response = analyticsReporting.Reports.BatchGet(getReport).Execute();
            return (List<Report>)response.Reports;
        }
    }

}
