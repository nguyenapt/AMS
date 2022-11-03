using System;

namespace PingdomClient.Extensions
{
    using System.Linq;

    public static class CustomExtensions
    {
        public static string ToQueryString(this object source)
        {
            var properties = source.GetType()
                .GetProperties()
                .ToDictionary(k => k.Name, v => v.GetValue(source));

            var queryString = properties.Where(p => p.Value != null)
                .Select(p => string.Format("{0}={1}", p.Key.ToLower(), p.Value.ToString()));

            return string.Format("?{0}", string.Join("&", queryString));
        }

        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static DateTime UnixTimeStampToDateTimeUtc(this long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
        public static DateTime UnixTimeStampToDateTimeUtc(this int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }
    }
}