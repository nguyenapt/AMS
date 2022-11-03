using AMS.ReportAutomation.Data.DataContext;

namespace AMS.ReportAutomation.Data.Repository.Interfaces
{
    public interface IReportData_PingdomRepository : IGenericRepository<ReportData_Pingdom>
    {
        ReportData_Pingdom FindByCheckIdAndReportTime(long checkId, long reportTime);
    }
}
