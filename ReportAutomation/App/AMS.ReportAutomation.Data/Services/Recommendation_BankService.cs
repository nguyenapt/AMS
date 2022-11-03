using System;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using AMS.ReportAutomation.ReportData;

namespace AMS.ReportAutomation.Data.Services
{
    public class Recommendation_BankService : EntityService<Recommendation_Bank>, IRecommendation_BankService
    {
        private readonly IRecommendationBank_Repository _recommendationBankRepository;

        public Recommendation_BankService(IRecommendationBank_Repository recommendationBankRepository, IAMSLogger logger)
            : base(recommendationBankRepository, logger)
        {
            _recommendationBankRepository = recommendationBankRepository;

        }

        public RecommendationModel GetRecommendationBank(Guid id)
        {
            return _recommendationBankRepository.FindBy(x => x.Id == id).Select(x => new RecommendationModel
            {
                Id = x.Id,
                ReportSection = x.ReportSection.Value,
                ReportSectionName = Enum.GetValues(typeof(ReportSection))
                    .Cast<ReportSection>()
                    .Select(v => new { Key = v.GetEnumDescription(), Value = ((int)v) }).FirstOrDefault(k => k.Value ==x.ReportSection)?.Key,
                ReportProperty = x.ReportProperty,
                Rule = x.Rule,
                RuleSql = x.RuleSql,
                Recommendation = x.Recommendation
            }).FirstOrDefault();
        }

        public List<RecommendationModel> GetRecommendationBanks()
        {
            return _recommendationBankRepository.GetAll()
                .Select(x => new RecommendationModel
                {
                Id = x.Id,
                    ReportSection = x.ReportSection.Value,
                ReportSectionName = Enum.GetValues(typeof(ReportSection))
                    .Cast<ReportSection>()
                    .Select(v => new  { Key = v.GetEnumDescription(), Value = ((int)v) }).FirstOrDefault(k => k.Value == x.ReportSection)?.Key,
                    ReportProperty = x.ReportProperty,
                Rule = x.Rule,
                RuleSql = x.RuleSql,
                    Recommendation = x.Recommendation
                })?.ToList();
        }

        public void SaveRecommendation(RecommendationModel model)
        {
            Recommendation_Bank recommendation;
            if (model.Id == Guid.Empty)
            {
                recommendation = new Recommendation_Bank
                {
                    Id = Guid.NewGuid(),
                    ReportSection = model.ReportSection,
                    ReportProperty = model.ReportProperty,
                    Recommendation = model.Recommendation,
                    Rule = model.Rule,
                    RuleSql = model.RuleSql
                };
                this.Add(recommendation);
            }
            else
            {
                recommendation = _recommendationBankRepository.FindFirstOrDefault(x => x.Id == model.Id);
                recommendation.ReportSection = model.ReportSection;
                recommendation.ReportProperty = model.ReportProperty;
                recommendation.Recommendation = model.Recommendation;
                recommendation.Rule = model.Rule;
                recommendation.RuleSql = model.RuleSql;
                _recommendationBankRepository.Edit(recommendation);
            }
            this.SafeExecute(this.Save);
        }

        public void DeleteRecommendation(Guid id)
        {
            if (id != Guid.Empty)
            {
                var recommendation = _recommendationBankRepository.FindFirstOrDefault(x => x.Id == id);
                if (recommendation != null)
                {
                    Delete(recommendation);
                    this.SafeExecute(this.Save);
                }
            }
        }
    }
}
