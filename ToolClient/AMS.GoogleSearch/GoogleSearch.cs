using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.Webmasters.v3;
using Google.Apis.Webmasters.v3.Data;

namespace AMS.GoogleSearch
{
    public class GoogleSearch
    {
        private string[] _serviceScopes { get; set; }

        public GoogleSearch(string[] serviceScopes)
        {
            this._serviceScopes = serviceScopes;            
        }

        public GoogleSearch()
        {
            this._serviceScopes = new string[] { WebmastersService.Scope.Webmasters };
        }

        /// <summary>
        /// This method requests Authentcation from a user using Oauth2.  
        /// Documentation https://developers.google.com/accounts/docs/OAuth2
        /// </summary>
        /// <param name="oauthSecretJsonFile">Json file where Credentials is stored.</param>
        /// <param name="oauthUser">OAuth email account.</param>
        /// <param name="secretLocation">Where the token file is stored after authenticated.</param>
        /// <returns>DriveService used to make requests against the Drive API.</returns>
        public WebmastersService AuthenticateOauth(string oauthSecretJsonFile, string oauthUser, bool checkIfTokenFileExisted = true)
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

            WebmastersService service = new WebmastersService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
            return service;
        }

        /// <summary>
        /// This method requests Authentcation from a user using ServiceAccount.
        /// </summary>
        /// <param name="serviceAccountSecretJsonFile">Json file where Credentials is stored.</param>
        /// <returns>DriveService used to make requests against the Drive API.</returns>
        public WebmastersService AuthenticateServiceAccount(string serviceAccountSecretJsonFile)
        {
            var initializer = new BaseClientService.Initializer();
            //using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(serviceAccountSecretJson)))
            using (var stream = new FileStream(serviceAccountSecretJsonFile, FileMode.Open, FileAccess.Read))
            {
                initializer.HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(_serviceScopes);
            }
            return new WebmastersService(initializer);
        }

        public SearchAnalyticsQueryResponse GetSearchAnalyticData(WebmastersService webMastersService, string clientSiteUrl, List<string> dimensions, string fromDate, string toDate, string keyword = "", int? rowLimit = null)
        {
            SearchAnalyticsQueryRequest body = new SearchAnalyticsQueryRequest();

            body.StartDate = fromDate;
            body.EndDate = toDate;
            body.Dimensions = dimensions;
            body.RowLimit = rowLimit;

            //Ref: https://developers.google.com/webmaster-tools/search-console-api-original/v3/searchanalytics/query
            //Results are sorted by click count, in descending order, unless you group by date, in which case results are sorted by date, in ascending order(oldest first, newest last). If there is a tie between two rows, the sort order is arbitrary.
            //Ref: https://developers.google.com/webmaster-tools/search-console-api-original/v3/how-tos/search_analytics
            if (keyword != "")
            {
                body.DimensionFilterGroups = new List<ApiDimensionFilterGroup>();
                ApiDimensionFilterGroup filterGroup = new ApiDimensionFilterGroup();
                ApiDimensionFilter filter = new ApiDimensionFilter();
                filter.Dimension = "query";
                filter.Expression = keyword;
                filter.Operator__ = "equals";

                filterGroup.Filters = new List<ApiDimensionFilter>();
                filterGroup.Filters.Add(filter);
                body.DimensionFilterGroups.Add(filterGroup);
            }

            return webMastersService.Searchanalytics.Query(body, clientSiteUrl).Execute();
        }
    }
}
