using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.ViewModel;
using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Repository.Interfaces
{
    public interface IRelease_MilestoneRepository : IGenericRepository<Release_Milestone>
    {
        IList<Release_MilestoneViewModel> GetReleaseMilestoneViewModels(Guid clientSiteId);
        IList<Release_MilestoneViewModel> GetReleaseMilestoneViewModels(Guid clientSiteId, DateTime reportFrom, DateTime reportTo);

        void DeleteReleaseMilestone(Guid id);

        void SaveReleaseMilestone(Release_MilestoneViewModel model);

        Release_MilestoneViewModel GetRelaseMilestoneById(Guid id);
    }
}
