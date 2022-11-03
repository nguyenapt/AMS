using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;

namespace AMS.ReportAutomation.Data.Repository
{
    public class PageSpeedInsights_Repository : GenericRepository<Data_PageSpeedInsights>, IPageSpeedInsights_Repository
    {
        public PageSpeedInsights_Repository(Entities context, IAMSLogger logger) : base(context, logger)
        {

        }
    }
}
