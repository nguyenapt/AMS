using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;

namespace AMS.ReportAutomation.Data.Repository
{
    public class ReportData_DynomapperRepository : GenericRepository<ReportData_Dynomapper>, IReportData_DynomapperRepository
    {
        public ReportData_DynomapperRepository(Entities context, IAMSLogger logger): base(context,logger)
        {
        }
    }
}