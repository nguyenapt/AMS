using System;
using System.Configuration;

namespace AMS.ReportAutomation.Common.Mail
{
    public class MailSettings
    {
        public static string Host = ConfigurationManager.AppSettings.Get("mail:Host") ?? "mail.Provide it here.se";
        public static int Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("mail:Port") ?? "25");
        public static bool EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mail:EnableSSL"));
        public static string Username = ConfigurationManager.AppSettings.Get("mail:Username");
        public static string Password = ConfigurationManager.AppSettings.Get("mail:Password");
        public static bool UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("mail:UseDefaultCredentials") ?? "false");
        public static string From = ConfigurationManager.AppSettings.Get("mail:From") ?? "noreply@Provide it here.com";
        
        public static string MailSubject = ConfigurationManager.AppSettings.Get("mail:Subject");
        public static string To = ConfigurationManager.AppSettings.Get("mail:To") ?? "amsteam@Provide it here.se";

        public static string MailSubscriptionSubject = ConfigurationManager.AppSettings.Get("mail:SubscriptionSubject");
        public static string MailSubscriptionBody = ConfigurationManager.AppSettings.Get("mail:SubscriptionBody");
        public static string MailSubscriptionReportUrl = ConfigurationManager.AppSettings.Get("mail:SubscriptionReportUrl");
    }
}
