using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.ReportAutomation.Data.Repository
{
    public class Release_MilestoneRepository : GenericRepository<Release_Milestone>, IRelease_MilestoneRepository
    {
        private Entities _dbContext;
        public Release_MilestoneRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public Release_MilestoneViewModel GetRelaseMilestoneById(Guid id)
        {
            return FindBy(x => x.Id == id).Select(x=> new Release_MilestoneViewModel
            {
                Id = x.Id,
                ClientSiteId = x.ClientSiteId.Value,
                ReleaseDate = x.ReleaseDate,
                ReleaseVersion = x.ReleaseVersion,
                Description = x.Description
            }).FirstOrDefault();
        }

        public IList<Release_MilestoneViewModel> GetReleaseMilestoneViewModels(Guid clientSiteId)
        {
            var result = (from x in _dbContext.Release_Milestone
                where x.ClientSiteId == clientSiteId
                select new Release_MilestoneViewModel()
                {
                    Id = x.Id,
                    ClientSiteId = x.ClientSiteId.Value,
                    ReleaseDate = x.ReleaseDate,
                    Description = x.Description,
                    ReleaseVersion = x.ReleaseVersion
                }).ToList();

            return result;
        }

        public IList<Release_MilestoneViewModel> GetReleaseMilestoneViewModels(Guid clientSiteId, DateTime reportFrom, DateTime reportTo)
        {
            var result = (from x in _dbContext.Release_Milestone
                where x.ClientSiteId == clientSiteId && x.ReleaseDate >= reportFrom && x.ReleaseDate <= reportTo
                select new Release_MilestoneViewModel()
                {
                    Id = x.Id,
                    ClientSiteId = x.ClientSiteId.Value,
                    ReleaseDate = x.ReleaseDate,
                    Description = x.Description,
                    ReleaseVersion = x.ReleaseVersion
                }).ToList();

            return result;
        }

        public void SaveReleaseMilestone(Release_MilestoneViewModel model)
        {
            Release_Milestone releaseMilestone;
            if (model.Id == Guid.Empty)
            {
                releaseMilestone = new Release_Milestone()
                {
                    Id = Guid.NewGuid(),
                    ClientSiteId = model.ClientSiteId,
                    ReleaseDate = model.ReleaseDate,
                    Description = model.Description,
                    ReleaseVersion = model.ReleaseVersion
                };
                this.Add(releaseMilestone);
            }
            else
            {
                releaseMilestone = FindFirstOrDefault(x => x.Id == model.Id);
                releaseMilestone.ReleaseDate = model.ReleaseDate;
                releaseMilestone.Description = model.Description;
                releaseMilestone.ReleaseVersion = model.ReleaseVersion;
                this.Edit(releaseMilestone);
            }
            this.SafeExecute(() => this.Save());
        }

        public void DeleteReleaseMilestone(Guid id)
        {
            if (id != Guid.Empty)
            {
                var releaseMilestone = FindFirstOrDefault(x => x.Id == id);
                if (releaseMilestone != null)
                {
                    this.Delete(releaseMilestone);
                    this.SafeExecute(() => this.Save());
                }
            }
        }
    }
}
