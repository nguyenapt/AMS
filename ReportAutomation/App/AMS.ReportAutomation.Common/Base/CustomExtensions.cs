using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AMS.ReportAutomation.Common.Base
{
    public static class CustomExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        public static bool SafeAny<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static string ToQueryString(this object source)
        {
            var properties = source.GetType()
                .GetProperties()
                .ToDictionary(k => k.Name, v => v.GetValue(source));

            var queryString = properties.Where(p => p.Value != null)
                .Select(p => string.Format("{0}={1}", p.Key.ToLower(), p.Value.ToString()));

            return string.Format("?{0}", string.Join("&", queryString));
        }

        /// <summary>
        /// Remove time, only keep date
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static long ToUnixDatestamp(this long unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return (long)(new DateTime(dtDateTime.Year, dtDateTime.Month, dtDateTime.Day) - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
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

        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
