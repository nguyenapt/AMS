using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Data.ViewModel
{
    public class RecommendationModel : IRecommendationBank
    {
        public Guid Id { get; set; }
        public int ReportSection { get; set; }
        public string ReportSectionName { get; set; }
        public string ReportProperty { get; set; }
        public string Rule { get; set; }
        public string RuleSql { get; set; }
        public string Recommendation { get; set; }
    }
}
