using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using System.Linq;

namespace AMS.ReportAutomation.Data.Repository
{
    public class ReportData_PingdomRepository : GenericRepository<ReportData_Pingdom>, IReportData_PingdomRepository
    {

        public ReportData_PingdomRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {

        }

        public ReportData_Pingdom FindByCheckIdAndReportTime(long checkId, long reportTime)
        {
            return _entities.Set<ReportData_Pingdom>().FirstOrDefault(d => d.CheckId == checkId && d.ReportTimeUtc == reportTime);
        }
    }
}
