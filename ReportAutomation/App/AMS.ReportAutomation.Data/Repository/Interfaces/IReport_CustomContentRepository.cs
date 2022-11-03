using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.ViewModel;
using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Repository.Interfaces
{
    public interface IReport_CustomContentRepository : IGenericRepository<Report_CustomContent>
    {
        IList<Report_CustomContentViewModel> GetReport_CustomContentViewModels(Guid reportId, long reportTo, uint reportDays);

        void DeleteCustomContentReport(Report_CustomContentViewModel model);

        void SaveCustomContentReport(Report_CustomContentViewModel model);

        string GetCustomContentReportById(Guid id);
    }
}
