using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.ViewModel
{
    public class ReportSubscriptionViewModel
    {
        public Guid Id { get; set; }
        public Guid ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReceiverEmails { get; set; }
        public string Frequency { get; set; }
        public bool? SubscribedByUser { get; set; }
        public bool? IsHourly { get; set; }
        public int? HourlyEveryHour { get; set; }
        public bool? IsDaily { get; set; }
        public bool? DailyEveryDay { get; set; }
        public bool? DailyEveryWeekDay { get; set; }
        public int? DailyEveryDayNumber { get; set; }
        public bool? IsWeekly { get; set; }
        public string WeeklyDays { get; set; }
        public int? StartAtHour { get; set; }
        public int? StartAtMinute { get; set; }
        public string SubscribedUser { get; set; }
        public bool? IsMonthly { get; set; }
        public int? MonthlyDay { get; set; }
        public int? MonthlyEveryMonth { get; set; }
    }
}
