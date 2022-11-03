using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace AMS.PageSpeed
{
    public class PageSpeedConfiguration
    {
        public string ApiKey { get; private set; }

        public string BaseAddress { get; private set; }
        public string ApiName { get; private set; }
        public PageSpeedConfiguration()
        {
            BaseAddress = GetConfigurationKey("BaseUrl") ?? "https://pagespeedonline.googleapis.com/pagespeedonline/v5/";
            ApiKey = GetConfigurationKey("ApiKey");            
            ApiName = GetConfigurationKey("ApiName");
        }

        private static string GetConfigurationKey(string key)
        {
            var fullKey = string.Format("pagespeed:{0}", key);
            if (ConfigurationManager.AppSettings.AllKeys.Contains(fullKey))
                return ConfigurationManager.AppSettings[fullKey];
            else
                return string.Empty;
        }
    }
}
