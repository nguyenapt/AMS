using System.Linq;
using System.Configuration;

namespace AMS.SpeedCurve
{
    public class SpeedCurveConfiguration
    {
        public string ApiKey { get; private set; }

        public string BaseAddress { get; private set; }


        public SpeedCurveConfiguration()
        {
            ApiKey = GetConfigurationKey("ApiKey");
            BaseAddress = GetConfigurationKey("BaseUrl");
        }

        private static string GetConfigurationKey(string key)
        {
            var fullKey = string.Format("speedcurve:{0}", key);
            if (ConfigurationManager.AppSettings.AllKeys.Contains(fullKey))
                return ConfigurationManager.AppSettings[fullKey];
            else
                return string.Empty;
        }
    }
}
