using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;

namespace AMS.ReportAutomation.Data.Repository
{
    public class ReportData_ScreamingFrogRepository : GenericRepository<ReportData_ScreamingFrog>, IReportData_ScreamingFrogRepository
    {
        public ReportData_ScreamingFrogRepository(Entities context, IAMSLogger logger): base(context,logger)
        {
        }
    }
}