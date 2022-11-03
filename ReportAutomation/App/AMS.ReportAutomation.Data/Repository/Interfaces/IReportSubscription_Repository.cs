using AMS.ReportAutomation.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.Data.Repository.Interfaces
{
    public interface IReportSubscription_Repository : IGenericRepository<ReportSubscription>
    {
        IList<ReportSubscriptionViewModel> GetReportSubscriptionViewModels();
    }
}
