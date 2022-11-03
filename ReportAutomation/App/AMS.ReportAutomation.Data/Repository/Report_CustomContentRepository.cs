using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.ReportAutomation.Data.Repository
{
    public class Report_CustomContentRepository : GenericRepository<Report_CustomContent>, IReport_CustomContentRepository
    {
        private Entities _dbContext;
        public Report_CustomContentRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public IList<Report_CustomContentViewModel> GetReport_CustomContentViewModels(Guid reportId, long reportToDateTime, uint reportDays)
        {
            var result = (from x in _dbContext.Report_CustomContent
                          where x.ReportId == reportId && x.ReportTo == reportToDateTime && x.ReportDays == reportDays
                          select new Report_CustomContentViewModel()
                          {
                              Id = x.Id,
                              ReportId = x.ReportId,
                              ReportToUnixTimestamp = x.ReportTo,
                              ReportDays = x.ReportDays,
                              CustomReportContent = x.CustomReportContent,
                              UnderReportSection = x.UnderReportSection
                          }).ToList();

            return result;
        }

        public void SaveCustomContentReport(Report_CustomContentViewModel model)
        {
            Report_CustomContent customReport;
            if (model.Id == Guid.Empty)
            {
                customReport = new Report_CustomContent()
                {
                    Id = Guid.NewGuid(),
                    ReportDays = model.ReportDays,
                    ReportId = model.ReportId,
                    ReportTo = model.ReportToUnixTimestamp,
                    CustomReportContent = model.CustomReportContent,
                    UnderReportSection = model.UnderReportSection
                };
                this.Add(customReport);
            }
            else
            {
                customReport = FindFirstOrDefault(x => x.Id == model.Id);
                customReport.ReportDays = model.ReportDays;
                customReport.ReportId = model.ReportId;
                customReport.ReportTo = model.ReportToUnixTimestamp;
                customReport.CustomReportContent = model.CustomReportContent;
                customReport.UnderReportSection = model.UnderReportSection;
                this.Edit(customReport);
            }
            this.SafeExecute(() => this.Save());
        }

        public void DeleteCustomContentReport(Report_CustomContentViewModel model)
        {
            if (model.Id != Guid.Empty)
            {
                var customReport = FindFirstOrDefault(x => x.Id == model.Id);
                if (customReport != null)
                {
                    this.Delete(customReport);
                    this.SafeExecute(() => this.Save());
                }
            }
        }

        public string GetCustomContentReportById(Guid id)
        {
            var customReport = FindFirstOrDefault(x => x.Id == id);
            if (customReport != null)
            {
                return customReport.CustomReportContent;
            }

            return string.Empty;
        }
    }
}
