namespace AMS.JiraServicedesk
{
    using System.Configuration;
    using System.Linq;

    public class ServicedeskConfiguration
    {
        public string ConfigPrefix { get; private set; }

        private string _baseUrl;
        public string BaseUrl {
            get
            {
                return _baseUrl;
            }
            set {
                _baseUrl = value;
                if (_baseUrl.EndsWith("//")) _baseUrl = _baseUrl.Substring(0, _baseUrl.Length - 1);
            }
        }
        public string BaseAddress {
            get
            {
                //https://docs.atlassian.com/jira-servicedesk/REST/3.6.2/
                return string.Format("{0}{1}", BaseUrl, "rest/servicedeskapi/");
            }
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServiceDeskProjectKey { get; set; }
        public string ServiceDeskDefaultNoneOrgName { get; private set; }
        public string ServiceDeskDefaultQueueName { get; private set; }
        public string ServiceDeskExcludedQueueNames { get; private set; }
        public string ServiceDeskQueueCreatedLastMonthName { get; private set; }
        public string ServiceDeskQueueCreatedLast7daysName { get; private set; }

        public ServicedeskConfiguration(string configPrefix = "jiraservicedesk") : this(null, null, configPrefix)
        {
        }

        public ServicedeskConfiguration(string baseUrl = null, string projectKey = null, string configPrefix = "jiraservicedesk")
        {
            ConfigPrefix = configPrefix;
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                //Try to find the config that has the specified baseUrl, so we will take the right Username and Password for the specified ServiceDesk.
                for (int i=1; i<100; i++)
                {
                    var sdConfigBaseUrl = GetConfigurationKey("BaseUrl", $"configPrefix{i}");
                    if (!string.IsNullOrWhiteSpace(sdConfigBaseUrl) && RemoveEndSlash(sdConfigBaseUrl.Trim()) == RemoveEndSlash(baseUrl.Trim()))
                    {                        
                        ConfigPrefix = $"configPrefix{i}";
                        break;
                    }
                }
            }

            BaseUrl = GetConfigurationKey("BaseUrl", ConfigPrefix) + "/";
            Username = GetConfigurationKey("Username", ConfigPrefix);
            Password = GetConfigurationKey("Password", ConfigPrefix);
            ServiceDeskProjectKey = !string.IsNullOrWhiteSpace(projectKey) ? projectKey.Trim() : GetConfigurationKey("ServiceDeskProjectKey", ConfigPrefix);
            ServiceDeskDefaultNoneOrgName = GetConfigurationKey("ServiceDeskDefaultNoneOrgName", ConfigPrefix);
            ServiceDeskDefaultQueueName = GetConfigurationKey("ServiceDeskDefaultQueueName", ConfigPrefix);
            ServiceDeskExcludedQueueNames = GetConfigurationKey("ServiceDeskExcludedQueueNames", ConfigPrefix);
            ServiceDeskQueueCreatedLastMonthName = GetConfigurationKey("ServiceDeskQueueCreatedLastMonthName", ConfigPrefix);
            ServiceDeskQueueCreatedLast7daysName = GetConfigurationKey("ServiceDeskQueueCreatedLast7daysName", ConfigPrefix);
        }

        private static string GetConfigurationKey(string key, string configPrefix)
        {
            var fullKey = string.Format("{0}:{1}", configPrefix, key);
            if (ConfigurationManager.AppSettings.AllKeys.Contains(fullKey))
                return ConfigurationManager.AppSettings[fullKey];
            else
                return string.Empty;
        }

        private static string RemoveEndSlash(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return url;
            if (url.EndsWith("/")) return url.Substring(0, url.Length - 1);
            return url;
        }
    }
}