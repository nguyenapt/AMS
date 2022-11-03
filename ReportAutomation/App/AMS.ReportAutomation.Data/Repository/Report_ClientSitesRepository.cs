using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AMS.ReportAutomation.Data.Repository
{
    public class Report_ClientSitesRepository : GenericRepository<Report_ClientSites>, IReport_ClientSitesRepository
    {
        private Entities _dbContext;
        public Report_ClientSitesRepository(Entities context, IAMSLogger logger) : base(context, logger)
        {
            this._dbContext = context;
        }

        public List<Report_ClientSiteViewModel> GetReport_ClientSiteViewModels(int clientId)
        {
            var result = this.SafeExecute<List<Report_ClientSiteViewModel>>(() => (from rcs in _dbContext.Report_ClientSites
                                                                             where rcs.ClientId == clientId
                                                                             select new Report_ClientSiteViewModel
                                                                             {
                                                                                 Id = rcs.Id,
                                                                                 ClientId = rcs.ClientId,
                                                                                 BrandName = rcs.BrandName,
                                                                                 Name = rcs.Name,
                                                                                 LogoUrl = rcs.LogoUrl,
                                                                                 SiteUrl = rcs.SiteUrl,
                                                                                 Description = rcs.Description,
                                                                                 CrawlerConfig = rcs.CrawlerConfig,
                                                                                 Report_ClientSiteViewModels = (from rr in _dbContext.Report_Reports
                                                                                                                where rr.ClientSiteId == rcs.Id
                                                                                                                select new ClientSite_ReportViewModel
                                                                                                                {
                                                                                                                    Id = rr.Id,
                                                                                                                    ClientSiteId = rr.ClientSiteId,
                                                                                                                    IsActive = rr.IsActive,
                                                                                                                    ReportName = rr.ReportName,
                                                                                                                    ToolIdString = rr.ToolIds
                                                                                                                }).ToList()
                                                                             }).ToList());

            foreach(var item in result)
            {
                if(item.Report_ClientSiteViewModels !=null && item.Report_ClientSiteViewModels.Any())
                {
                    foreach(var report in item.Report_ClientSiteViewModels)
                    {
                        report.ToolIds = JsonConvert.DeserializeObject<Dictionary<string, string>>(report.ToolIdString);
                    }
                }
            }

            return result;
        }
    }
}
