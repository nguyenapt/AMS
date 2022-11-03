namespace AMS.Detectify
{
    using System.Configuration;
    using System.Linq;

    public class DetectifyConfiguration
    {
        public string ApiKey { get; private set; }

        public string BaseAddress { get; private set; }

        public string SecretKey { get; private set; }

        public DetectifyConfiguration()
        {
            ApiKey = GetConfigurationKey("ApiKey");
            BaseAddress = GetConfigurationKey("BaseUrl");
            SecretKey = GetConfigurationKey("SecretKey");
        }

        private static string GetConfigurationKey(string key)
        {
            var fullKey = string.Format("detectify:{0}", key);
            if (ConfigurationManager.AppSettings.AllKeys.Contains(fullKey))
                return ConfigurationManager.AppSettings[fullKey];
            else
                return string.Empty;
        }
    }
}