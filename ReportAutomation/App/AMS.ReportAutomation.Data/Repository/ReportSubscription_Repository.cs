using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace AMS.ReportAutomation.Data.Repository
{
    public class ReportSubscription_Repository : GenericRepository<ReportSubscription>, IReportSubscription_Repository
    {
        private Entities _dbContext;
        public ReportSubscription_Repository(Entities context, IAMSLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public IList<ReportSubscriptionViewModel> GetReportSubscriptionViewModels()
        {
            var result = (from x in _dbContext.ReportSubscriptions
                join rp in _dbContext.Report_Reports on x.ReportId equals rp.Id 
                select new ReportSubscriptionViewModel()
                {
                    Id = x.Id,
                    Frequency = x.Frequency,
                    ReceiverEmails = x.ReceiverEmails,
                    ReportId = x.ReportId,
                    ReportName = rp.ReportName,
                    SubscribedByUser = x.SubscribedByUser,
                    SubscribedUser = x.SubscribedUser,
                    DailyEveryDay = x.DailyEveryDay,
                    DailyEveryDayNumber = x.DailyEveryDayNumber,
                    DailyEveryWeekDay = x.DailyEveryWeekDay,
                    HourlyEveryHour = x.HourlyEveryHour,
                    IsDaily = x.IsDaily,
                    IsHourly = x.IsHourly,
                    IsWeekly = x.IsWeekly,
                    StartAtHour = x.StartAtHour,
                    StartAtMinute = x.StartAtMinute,
                    WeeklyDays = x.WeeklyDays,
                    IsMonthly = x.IsMonthly,
                    MonthlyDay = x.MonthlyDay,
                    MonthlyEveryMonth = x.MonthlyEveryMonth
                }).ToList();

            return result;
        }
    }
}
