using System;
using System.Collections.Generic;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.Data.Services
{
    public class ReportSubscription_Service : EntityService<ReportSubscription>, IReportSubscription_Service
    {
        private readonly IReportSubscription_Repository _reportSubscriptionRepository;

        public ReportSubscription_Service(IReportSubscription_Repository reportSubscriptionRepository, IAMSLogger logger)
            : base(reportSubscriptionRepository, logger)
        {
            _reportSubscriptionRepository = reportSubscriptionRepository;

        }
        
        public void DeleteReportSubscriptionConfiguration(Guid id)
        {
            if (id != Guid.Empty)
            {
                var existConfiguration = _reportSubscriptionRepository.FindFirstOrDefault(x => x.Id == id);

                if (existConfiguration != null)
                {
                    _reportSubscriptionRepository.Delete(existConfiguration);
                    this.SafeExecute(() => _reportSubscriptionRepository.Save());
                }
            }
        }
        
        public void RemoveUserReportSubscriptionConfiguration(Guid reportId, string subscribedUser)
        {
            if (reportId == Guid.Empty) throw new ArgumentNullException("reportId");

            var existConfiguration = _reportSubscriptionRepository
                    .FindFirstOrDefault(x => x.ReportId == reportId
                    && x.SubscribedByUser == true && x.SubscribedUser == subscribedUser);

            if (existConfiguration != null)
            {
                _reportSubscriptionRepository.Delete(existConfiguration);
                this.SafeExecute(() => _reportSubscriptionRepository.Save());
            }
        }
        
        public ReportSubscriptionViewModel GetReportSubscriptionConfiguration(Guid reportId, string subscribedUser)
        {
            if (reportId == Guid.Empty) throw new ArgumentNullException("reportId");

            var existConfiguration = _reportSubscriptionRepository
                    .FindFirstOrDefault(x => x.ReportId == reportId
                    && x.SubscribedByUser == true && x.SubscribedUser == subscribedUser);

            if (existConfiguration != null)
            {
                return new ReportSubscriptionViewModel()
                {
                    Id = existConfiguration.Id,
                    Frequency = existConfiguration.Frequency,
                    ReceiverEmails = existConfiguration.ReceiverEmails,
                    ReportId = existConfiguration.ReportId,
                    //ReportName = we don't need it here,
                    SubscribedByUser = existConfiguration.SubscribedByUser,
                    SubscribedUser = existConfiguration.SubscribedUser,
                    DailyEveryDay = existConfiguration.DailyEveryDay,
                    DailyEveryDayNumber = existConfiguration.DailyEveryDayNumber,
                    DailyEveryWeekDay = existConfiguration.DailyEveryWeekDay,
                    HourlyEveryHour = existConfiguration.HourlyEveryHour,
                    IsDaily = existConfiguration.IsDaily,
                    IsHourly = existConfiguration.IsHourly,
                    IsWeekly = existConfiguration.IsWeekly,
                    StartAtHour = existConfiguration.StartAtHour,
                    StartAtMinute = existConfiguration.StartAtMinute,
                    WeeklyDays = existConfiguration.WeeklyDays
                };
            }

            return null;
        }

        public IList<ReportSubscriptionViewModel> GetReportSubscriptionConfigurations()
        {
            return _reportSubscriptionRepository.GetReportSubscriptionViewModels();
        }

        public void SaveReportSubscriptionConfiguration(ReportSubscriptionViewModel configuration)
        {
            var reportSubscription = new ReportSubscription
            {
                Id = configuration.Id,
                ReportId = configuration.ReportId,
                Frequency = configuration.Frequency,
                ReceiverEmails = configuration.ReceiverEmails,
                SubscribedByUser = configuration.SubscribedByUser,
                SubscribedUser = configuration.SubscribedUser,
                StartAtMinute = configuration.StartAtMinute,
                DailyEveryWeekDay = configuration.DailyEveryWeekDay,
                WeeklyDays = configuration.WeeklyDays,
                IsWeekly = configuration.IsWeekly,
                IsHourly = configuration.IsHourly,
                DailyEveryDay = configuration.DailyEveryDay,
                DailyEveryDayNumber = configuration.DailyEveryDayNumber,
                StartAtHour = configuration.StartAtHour,
                IsDaily = configuration.IsDaily,
                HourlyEveryHour = configuration.HourlyEveryHour,
                IsMonthly = configuration.IsMonthly,
                MonthlyDay = configuration.MonthlyDay,
                MonthlyEveryMonth = configuration.MonthlyEveryMonth
            };
            var existData = _reportSubscriptionRepository.FindFirstOrDefault(x => x.Id == configuration.Id);
            
            if (existData == null)
            {
                reportSubscription.Id = Guid.NewGuid();
                _reportSubscriptionRepository.Add(reportSubscription);
            }
            else
            {
                existData.ReportId = configuration.ReportId;
                existData.Frequency = configuration.Frequency;
                existData.ReceiverEmails = configuration.ReceiverEmails;
                existData.SubscribedByUser = configuration.SubscribedByUser;
                existData.SubscribedUser = configuration.SubscribedUser;
                existData.StartAtMinute = configuration.StartAtMinute;
                existData.DailyEveryWeekDay = configuration.DailyEveryWeekDay;
                existData.WeeklyDays = configuration.WeeklyDays;
                existData.IsWeekly = configuration.IsWeekly;
                existData.IsHourly = configuration.IsHourly;
                existData.DailyEveryDay = configuration.DailyEveryDay;
                existData.DailyEveryDayNumber = configuration.DailyEveryDayNumber;
                existData.StartAtHour = configuration.StartAtHour;
                existData.IsDaily = configuration.IsDaily;
                existData.HourlyEveryHour = configuration.HourlyEveryHour;
                existData.IsMonthly = configuration.IsMonthly;
                existData.MonthlyDay = configuration.MonthlyDay;
                existData.MonthlyEveryMonth = configuration.MonthlyEveryMonth;
                _reportSubscriptionRepository.Edit(existData);
            }
            this.SafeExecute(() => _reportSubscriptionRepository.Save());
        }

        public void InsertUserReportSubscriptionConfiguration(ReportSubscriptionViewModel configuration)
        {
            var reportSubscription = new ReportSubscription
            {
                Id = configuration.Id,
                ReportId = configuration.ReportId,
                Frequency = configuration.Frequency,
                ReceiverEmails = configuration.ReceiverEmails,
                SubscribedByUser = configuration.SubscribedByUser,
                SubscribedUser = configuration.SubscribedUser,
                StartAtMinute = configuration.StartAtMinute,
                DailyEveryWeekDay = configuration.DailyEveryWeekDay,
                WeeklyDays = configuration.WeeklyDays,
                IsWeekly = configuration.IsWeekly,
                IsHourly = configuration.IsHourly,
                DailyEveryDay = configuration.DailyEveryDay,
                DailyEveryDayNumber = configuration.DailyEveryDayNumber,
                StartAtHour = configuration.StartAtHour,
                IsDaily = configuration.IsDaily,
                HourlyEveryHour = configuration.HourlyEveryHour,
                IsMonthly = configuration.IsMonthly,
                MonthlyDay = configuration.MonthlyDay,
                MonthlyEveryMonth = configuration.MonthlyEveryMonth
            };
            ReportSubscription existData = null;
            if (configuration.SubscribedByUser == true)
            {
                existData = _reportSubscriptionRepository
                    .FindFirstOrDefault(x => x.ReportId == configuration.ReportId
                        && x.SubscribedByUser == true && x.SubscribedUser == configuration.SubscribedUser);
            }

            if (existData == null)
            {
                reportSubscription.Id = Guid.NewGuid();
                _reportSubscriptionRepository.Add(reportSubscription);
            }
            else
            {
                existData.Frequency = configuration.Frequency;
                existData.ReceiverEmails = configuration.ReceiverEmails;
                existData.StartAtMinute = configuration.StartAtMinute;
                existData.DailyEveryWeekDay = configuration.DailyEveryWeekDay;
                existData.WeeklyDays = configuration.WeeklyDays;
                existData.IsWeekly = configuration.IsWeekly;
                existData.IsHourly = configuration.IsHourly;
                existData.DailyEveryDay = configuration.DailyEveryDay;
                existData.DailyEveryDayNumber = configuration.DailyEveryDayNumber;
                existData.StartAtHour = configuration.StartAtHour;
                existData.IsDaily = configuration.IsDaily;
                existData.HourlyEveryHour = configuration.HourlyEveryHour;
                existData.IsMonthly = configuration.IsMonthly;
                existData.MonthlyDay = configuration.MonthlyDay;
                existData.MonthlyEveryMonth = configuration.MonthlyEveryMonth;
                _reportSubscriptionRepository.Edit(existData);
            }
            this.SafeExecute(() => _reportSubscriptionRepository.Save());
        }
    }
}