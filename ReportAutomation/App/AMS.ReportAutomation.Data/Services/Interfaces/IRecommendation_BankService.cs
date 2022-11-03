using AMS.ReportAutomation.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.ReportAutomation.Data.ViewModel;

namespace AMS.ReportAutomation.Data.Services.Interfaces
{
    public interface IRecommendation_BankService : IEntityService<Recommendation_Bank>
    {
        List<RecommendationModel> GetRecommendationBanks();

        RecommendationModel GetRecommendationBank(Guid id);

        void SaveRecommendation(RecommendationModel model);
        void DeleteRecommendation(Guid id);
    }
}
