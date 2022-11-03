namespace AMS.Dareboost
{
    using System.Configuration;
    using System.Linq;

    public class DareboostConfiguration
    {
        public string ApiToken { get; private set; }

        public string BaseAddress { get; private set; }

        public DareboostConfiguration()
        {
            ApiToken = GetConfigurationKey("ApiToken");
            BaseAddress = GetConfigurationKey("BaseUrl");
        }

        private static string GetConfigurationKey(string key)
        {
            var fullKey = string.Format("dareboost:{0}", key);
            if (ConfigurationManager.AppSettings.AllKeys.Contains(fullKey))
                return ConfigurationManager.AppSettings[fullKey];
            else
                return string.Empty;
        }
    }
}