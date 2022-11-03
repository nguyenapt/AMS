using AMS.JiraServicedesk.Cache;
using AMS.JiraServicedesk.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AMS.JiraServicedesk
{
    public class ServiceDeskIT : ServiceDesk
    {
        public ServiceDeskIT(ICache memoryCache)
            : base(new ServicedeskConfiguration("jiraservicedesk_IT"), memoryCache)
        { }
    }
    public class ServiceDesk3 : ServiceDesk
    {
        public ServiceDesk3(ICache memoryCache)
            : base(new ServicedeskConfiguration("jiraservicedesk3"), memoryCache)
        { }
    }

    public class ServiceDesk2 : ServiceDesk
    {
        public ServiceDesk2(ICache memoryCache)
            : base(new ServicedeskConfiguration("jiraservicedesk2"), memoryCache)
        { }
    }

    public class ServiceDesk : BaseClient
    {
        protected ICache _memoryCache;

        public ServiceDesk(ServicedeskConfiguration configuration)
            : this(configuration, null)
        {
        }

        public ServiceDesk(ICache memoryCache)
            : this(new ServicedeskConfiguration("jiraservicedesk1"), memoryCache)
        { }

        public ServiceDesk(ServicedeskConfiguration configuration, ICache memoryCache)
            : base(configuration)
        {
            _memoryCache = memoryCache;
        }

        public Info GetInfo()
        {
            //TODO: Add cache for this to boost up performance

            var apiMethod = "info";
            return GetAsync<Info>(apiMethod).Result;
        }

        public ServiceDeskObject[] GetServiceDeskObjects()
        {
            var cacheKey = Configuration.ConfigPrefix + "_GetServiceDeskObjects";
            ServiceDeskObject[] cachedJsonData = null;
            if (_memoryCache != null) cachedJsonData = (ServiceDeskObject[])_memoryCache[cacheKey];
            if (cachedJsonData != null) return cachedJsonData;

            //https://docs.atlassian.com/jira-servicedesk/REST/3.6.2/#servicedeskapi/servicedesk-getServiceDesks
            var queryString = "start=0";
            var apiMethod = "servicedesk?" + queryString;
            var jsonData = GetAsync<ServiceDeskObjectJson>(apiMethod).Result;

            //TODO: Move caching duration to config file
            if (_memoryCache != null && jsonData != null)
                _memoryCache.InsertAbsoluteCache(cacheKey, jsonData.values, TimeSpan.FromMinutes(60));

            return (jsonData != null) ? ((jsonData.size != 0) ? jsonData.values : null) : null;
        }
        private class ServiceDeskObjectJson
        {
            public int size { get; set; }
            public bool isLastPage { get; set; }
            public ServiceDeskObject[] values { get; set; }
        }

        public Organization[] GetOrganizations()
        {
            //TODO: Add cache for this to boost up performance

            //https://docs.atlassian.com/jira-servicedesk/REST/3.6.2/#servicedeskapi/organization-getOrganizations
            //pageSize can't be greater than 50 according to atlassian document?
            var pageSize = 50;
            //TODO: Loop through the pages to concat data from all the pages (refer to the code in GetIssuesInQueue function for example done)
            var queryString = "start=0&limit=" + pageSize;
            var apiMethod = "organization?" + queryString;
            var jsonData = GetAsync<OrganizationJson>(apiMethod).Result;
            return (jsonData != null) ? jsonData.values : null;
        }
        public Organization[] GetOrganizationsByServiceDesk(string serviceDeskId)
        {
            //TODO: Add cache for this to boost up performance

            //https://docs.atlassian.com/jira-servicedesk/REST/3.6.2/#servicedeskapi/servicedesk/{serviceDeskId}/organization-getOrganizations
            //pageSize can't be greater than 50 according to atlassian document?
            var pageSize = 50;
            //TODO: Loop through the pages to concat data from all the pages (refer to the code in GetIssuesInQueue function for example done)
            var queryString = "start=0&limit=" + pageSize;
            var apiMethod = "servicedesk/" + serviceDeskId + "/organization?" + queryString;
            var jsonData = GetAsync<OrganizationJson>(apiMethod).Result;
            return (jsonData != null) ? jsonData.values : null;
        }
        private class OrganizationJson
        {
            public int size { get; set; }
            public bool isLastPage { get; set; }
            public Organization[] values { get; set; }
        }

        public Request[] GetMyOpenRequests()
        {
            //TODO: Add cache for this to boost up performance

            //https://docs.atlassian.com/jira-servicedesk/REST/3.6.2/#servicedeskapi/request-getMyCustomerRequests
            //pageSize can't be greater than 50 according to atlassian document?
            var pageSize = 50;
            //TODO: Loop through the pages to concat data from all the pages (refer to the code in GetIssuesInQueue function for example done)
            var queryString = "requestStatus=OPEN_REQUESTS&start=0&limit=" + pageSize;
            var apiMethod = "request?" + queryString;
            var jsonData = GetAsync<RequestJson>(apiMethod).Result;
            return (jsonData != null) ? ((jsonData.size != 0) ? jsonData.values : null) : null;
        }
        private class RequestJson
        {
            public int size { get; set; }
            public bool isLastPage { get; set; }
            public Request[] values { get; set; }
        }

        public ServiceDeskQueue[] GetQueuesByServiceDesk(string serviceDeskId)
        {
            var cacheKey = Configuration.ConfigPrefix + "_GetQueuesByServiceDesk_" + serviceDeskId;
            ServiceDeskQueue[] cachedJsonData = null;
            if (_memoryCache != null) cachedJsonData = (ServiceDeskQueue[])_memoryCache[cacheKey];
            if (cachedJsonData != null) return cachedJsonData;

            //https://docs.atlassian.com/jira-servicedesk/REST/3.6.2/#servicedeskapi/servicedesk/{serviceDeskId}/queue
            //pageSize can't be greater than 50 according to atlassian document?
            var pageSize = 50;
            //TODO: Loop through the pages to concat data from all the pages (refer to the code in GetIssuesInQueue function for example done)
            var queryString = "includeCount=true&start=0&limit=" + pageSize;
            var apiMethod = "servicedesk/" + serviceDeskId + "/queue?" + queryString;
            var jsonData = GetAsync<ServiceDeskQueueJson>(apiMethod).Result;

            //TODO: Move caching duration to config file
            if (_memoryCache != null && jsonData != null)
                _memoryCache.InsertAbsoluteCache(cacheKey, jsonData.values, TimeSpan.FromMinutes(10));

            return (jsonData != null) ? ((jsonData.size != 0) ? jsonData.values : null) : null;
        }
        private class ServiceDeskQueueJson
        {
            public int size { get; set; }
            public bool isLastPage { get; set; }
            public ServiceDeskQueue[] values { get; set; }
        }

        public Issue[] GetIssuesInQueue(string serviceDeskId, string queueId)
        {
            var cacheKey = Configuration.ConfigPrefix + "_GetIssuesInQueue_" + serviceDeskId + "_" + queueId;
            Issue[] cachedJsonData = null;
            if (_memoryCache != null) cachedJsonData = (Issue[])_memoryCache[cacheKey];
            if (cachedJsonData != null) return cachedJsonData;

            const int maxDataRecords = 1000;
            //TODO: What to do if we have more data than maxDataRecords?

            //https://docs.atlassian.com/jira-servicedesk/REST/3.6.2/#servicedeskapi/servicedesk/{serviceDeskId}/queue-getIssuesInQueue
            var jsonData = new QueueIssuesJson() { size = 0, isLastPage = false, values = new Issue[]{} };

            var pageIndex = 0;
            //pageSize can't be greater than 50 according to atlassian document?
            var pageSize = 50;
            while (jsonData.isLastPage == false)
            {
                var queryString = "start=" + pageIndex + "&limit=" + pageSize;
                var apiMethod = "servicedesk/" + serviceDeskId + "/queue/" + queueId + "/issue?" + queryString;
                var jsonDataReceived = GetAsync<QueueIssuesJson>(apiMethod).Result;
                jsonData.isLastPage = jsonDataReceived.isLastPage;
                if (jsonDataReceived != null && jsonDataReceived.size > 0)
                {
                    jsonData.size += jsonDataReceived.size;
                    jsonData.values = jsonData.values.Concat(jsonDataReceived.values).ToArray();
                }

                pageIndex += pageSize;
                if (pageIndex > maxDataRecords) break;
            }

            //TODO: If error connecting to JIRA ServiceDesk, will jsonData.size = 0 or exception? Because if error but  jsonData.size = 0, we may not want to cache :)
            //TODO: Move caching duration to config file
            if (_memoryCache != null) _memoryCache.InsertAbsoluteCache(cacheKey, jsonData.values, TimeSpan.FromMinutes(60));

            if (jsonData.size == 0) return null;                        
            return jsonData.values;
        }
        private class QueueIssuesJson
        {
            public int size { get; set; }
            public bool isLastPage { get; set; }
            public Issue[] values { get; set; }
            //public object _links { get; set; }
        }

        /// <summary>
        /// Get issues by JQL.
        /// </summary>
        /// <param name="jql">JQL query.</param>
        /// <returns></returns>
        public SearchResultJson GetIssuesByJQL(string jql)
        {
            if (string.IsNullOrWhiteSpace(jql)) throw new ArgumentNullException("jql");

            const int maxDataRecords = 1000;
            //TODO: What to do if we have more data than maxDataRecords?

            //https://community.atlassian.com/t5/Jira-questions/How-to-pass-jql-when-calling-search-REST-API/qaq-p/874031
            //https://community.atlassian.com/t5/Jira-questions/How-to-get-more-than-50-issues-with-search-API/qaq-p/463122
            var jsonData = new SearchResultJson() { total = 0, isLastPage = false, issues = new Issue[] { } };

            var pageIndex = 0;
            var pageSize = 100;
            while (jsonData.isLastPage == false)
            {
                var queryString = "&startAt=" + pageIndex + "&maxResults=" + pageSize;
                var apiMethod = "/rest/api/2/search?jql=" + jql + queryString;
                var jsonDataReceived = GetAsync<SearchResultJson>(apiMethod).Result;
                jsonData.isLastPage = (jsonDataReceived.startAt + jsonDataReceived.maxResults >= jsonDataReceived.total);
                if (jsonDataReceived != null && jsonDataReceived.total > 0)
                {
                    jsonData.total += jsonDataReceived.total;
                    jsonData.issues = jsonData.issues.Concat(jsonDataReceived.issues).ToArray();
                }

                pageIndex += pageSize;
                if (pageIndex > maxDataRecords) break;
            }

            return jsonData;
        }

        /// <summary>
        /// Get issues created in a month.
        /// </summary>
        /// <param name="projectKey">Service Desk Project Key.</param>
        /// <param name="monthsAgo">How many months ago? 0: current month, 1: last month, etc.</param>
        /// <returns></returns>
        public Issue[] GetIssuesCreatedInAMonth(string projectKey, int monthsAgo)
        {
            if (string.IsNullOrWhiteSpace(projectKey)) throw new ArgumentNullException("projectKey");
            if (monthsAgo < 0) throw new ArgumentOutOfRangeException();

            var cacheKey = Configuration.ConfigPrefix + "_GetIssuesCreatedInAMonth_" + projectKey + "_" + monthsAgo;
            Issue[] cachedJsonData = null;
            if (_memoryCache != null) cachedJsonData = (Issue[])_memoryCache[cacheKey];
            if (cachedJsonData != null) return cachedJsonData;

            var jsonData = GetIssuesByJQL("project=" + projectKey + "%20and%20created%20>=%20startOfMonth(" + (0 - monthsAgo) + ")%20and%20created%20<=%20startOfMonth(" + (1 - monthsAgo) + ")&fields=issuetype,priority,status,assignee,created,timespent," + Issue.Const_CustomerRequestTypeField + "," + Issue.Const_OrganizationsField + "," + Issue.Const_ComponentField + "," + Issue.Const_MarketField + "," + Issue.Const_WebsiteTierField);

            //TODO: If error connecting to JIRA ServiceDesk, will jsonData.total = 0 or exception? Because if error but  jsonData.total = 0, we may not want to cache :)
            //TODO: Move caching duration to config file
            if (_memoryCache != null) _memoryCache.InsertAbsoluteCache(cacheKey, jsonData.issues, TimeSpan.FromMinutes(60));

            if (jsonData.total == 0) return null;
            return jsonData.issues;
        }
        public class SearchResultJson
        {
            public string expand { get; set; }
            public int startAt { get; set; }
            public int maxResults { get; set; }
            public int total { get; set; }
            public bool isLastPage { get; set; }
            public Issue[] issues { get; set; }
        }
    }

    public class BaseClient
    {
        private readonly HttpClient _baseClient;

        public ServicedeskConfiguration Configuration { get; }

        protected BaseClient(ServicedeskConfiguration configuration)
        {
            Configuration = configuration;

            _baseClient = new HttpClient()
            {
                BaseAddress = new Uri(configuration.BaseAddress)
            };
            //TODO: Change to OAuth so it is more secure
            //https://developer.atlassian.com/server/jira/platform/oauth/
            _baseClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", configuration.Username, configuration.Password))));

            // Create request headers
            var headers = MakeHeaders();
            // Add headers to request
            headers.ToList().ForEach(h => _baseClient.DefaultRequestHeaders.Add(h.Key, h.Value));
        }

        /// <summary>
        /// Create the headers used to sign an API request.
        /// </summary>
        /// <returns>Returns a dictionary of headers to use with an API call.</returns>
        private Dictionary<string, string> MakeHeaders()
        {
            return new Dictionary<string, string>
            {
                ["X-ExperimentalApi"] = "opt-in"
            };
        }

        #region Rest Methods

        internal Task<T> GetAsync<T>(string apiMethod)
        {
            return SendAsync<T>(apiMethod, null, HttpMethod.Get);
        }

        internal Task<T> PostAsync<T>(string apiMethod, object data)
        {
            return SendAsync<T>(apiMethod, data, HttpMethod.Post);
        }

        internal Task<T> PutAsync<T>(string apiMethod, object data)
        {
            return SendAsync<T>(apiMethod, data, HttpMethod.Put);
        }

        internal Task<T> DeleteAsync<T>(string apiMethod)
        {
            return DeleteAsync<T>(apiMethod, null);
        }

        internal Task<T> DeleteAsync<T>(string apiMethod, object data)
        {
            return SendAsync<T>(apiMethod, data, HttpMethod.Delete);
        }

        #endregion

        #region Shared Methods

        private async Task<T> SendAsync<T>(string apiMethod, object data, HttpMethod httpMethod)
        {
            var request = new HttpRequestMessage(httpMethod, apiMethod);

            if (data != null)
            {
                request.Content = GetFormUrlEncodedContent(data);
            }

            using (request)
            using (var response = await _baseClient.SendAsync(request).ConfigureAwait(false))
            using (var content = response.Content)
            using (var stream = await content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                var jsonString = await reader.ReadToEndAsync();
                //TODO: Take HTTP Response Code instead
                if (jsonString.Contains("Unauthorized (401)") || jsonString.Contains("You don't have permission to access")) throw new UnauthorizedAccessException();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
        }

        private static StringContent GetFormUrlEncodedContent(object anonymousObject)
        {
            var properties = from propertyInfo in anonymousObject.GetType().GetProperties()
                             where propertyInfo.GetValue(anonymousObject, null) != null
                             select new KeyValuePair<string, string>(WebUtility.UrlEncode(propertyInfo.Name), WebUtility.UrlEncode(propertyInfo.GetValue(anonymousObject, null).ToString()));
            var dict = properties.ToDictionary((k) => k.Key, (k) => k.Value);
            var postData = string.Join("&",
                dict.Select(kvp =>
                    string.Format("{0}={1}", kvp.Key, kvp.Value)));

            return new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
        }

        #endregion
    }
}
