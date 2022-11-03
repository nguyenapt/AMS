using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.Repository
{
    public class RecommendationBank_Repository : GenericRepository<Recommendation_Bank>, IRecommendationBank_Repository
    {
        public RecommendationBank_Repository(Entities context,IAMSLogger logger): base(context,logger)
        {

        }
    }
}
