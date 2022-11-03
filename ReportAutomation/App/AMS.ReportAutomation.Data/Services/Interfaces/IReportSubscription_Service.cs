using System;
using System.Collections.Generic;
using AMS.ReportAutomation.Data.ViewModel;
using AMS.ReportAutomation.Data.DataContext;

namespace AMS.ReportAutomation.Data.Services.Interfaces
{
    public interface IReportSubscription_Service : IEntityService<ReportSubscription>
    {
        IList<ReportSubscriptionViewModel> GetReportSubscriptionConfigurations();
        ReportSubscriptionViewModel GetReportSubscriptionConfiguration(Guid reportId, string subscribedUser);

        void InsertUserReportSubscriptionConfiguration(ReportSubscriptionViewModel configuration);
        void RemoveUserReportSubscriptionConfiguration(Guid reportId, string subscribedUser);

        void SaveReportSubscriptionConfiguration(ReportSubscriptionViewModel configuration);
        void DeleteReportSubscriptionConfiguration(Guid id);
    }
}
