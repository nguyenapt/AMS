using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;

namespace AMS.ReportAutomation.Data.Repository
{
    public class ReportData_PageSpeedRepository : GenericRepository<ReportData_PageSpeedInsights>, IReportData_PageSpeedRepository
    {
        public ReportData_PageSpeedRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {
        }
    }
}