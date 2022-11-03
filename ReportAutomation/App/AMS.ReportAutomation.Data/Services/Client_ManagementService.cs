using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using AMS.ReportAutomation.Data.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.ReportAutomation.Data.Services
{
    public class Client_ManagementService : IClient_ManagementService
    {
        private readonly IReport_ClientsRepository _report_ClientsRepository;
        private readonly IReport_ClientSitesRepository _report_ClientSitesRepository;
        private readonly IReport_ReportsRepository _report_ReportsRepository;
        private readonly IReport_ClientJiraServiceDesksRepository _report_ClientJiraServiceDesksRepository;
        private readonly IAMSLogger _logger;

        public Client_ManagementService(IReport_ReportsRepository report_ReportsRepository, IReport_ClientsRepository report_ClientsRepository, IReport_ClientSitesRepository report_ClientSitesRepository, IReport_ClientJiraServiceDesksRepository report_ClientJiraServiceDesksRepository, IAMSLogger logger)
            : base()
        {
            _report_ClientsRepository = report_ClientsRepository;
            _report_ClientSitesRepository = report_ClientSitesRepository;
            _report_ReportsRepository = report_ReportsRepository;
            _report_ClientJiraServiceDesksRepository = report_ClientJiraServiceDesksRepository;
            _logger = logger;
        }


        public Report_Clients GetReport_Client(int clientId)
        {
            return _report_ClientsRepository.FindFirstOrDefault(x => x.Id == clientId);            
        }

        public void AddClient(string clientName,string logoUrl)
        {
            _report_ClientsRepository.Add(new Report_Clients() {Name = clientName, LogoUrl = logoUrl });
            _report_ClientsRepository.Save();
        }

        public void UpdateClient(int clientId, string clientName,string logoUrl)
        {
            var client = _report_ClientsRepository.FindFirstOrDefault(x => x.Id == clientId);
            if (client != null)
            {
                client.Name = clientName;
                client.LogoUrl = logoUrl;
                _report_ClientsRepository.Edit(client);
                _report_ClientsRepository.Save();
            }
        }

        public void DeleteClient(int clientId)
        {
            var client = _report_ClientsRepository.FindFirstOrDefault(x => x.Id == clientId);
            if (client != null)
            {
                _report_ClientsRepository.Delete(client);
                _report_ClientsRepository.Save();
            }
        }

        public Report_ClientSites GetReport_ClientSite(Guid clientSiteId)
        {
            return _report_ClientSitesRepository.FindFirstOrDefault(x => x.Id == clientSiteId);
        }

        public void AddClientSite(Report_ClientSiteViewModel clientSites)
        {
            var clientSite = new Report_ClientSites() { 
                Id = clientSites.Id,
                ClientId = clientSites.ClientId,
                BrandName = clientSites.BrandName,
                Name = clientSites.Name,
                Description = clientSites.Description,
                LogoUrl = clientSites.LogoUrl,
                SiteUrl = clientSites.SiteUrl,
                CrawlerConfig = clientSites.CrawlerConfig
            };
            _report_ClientSitesRepository.Add(clientSite);

            if(clientSites.Report_ClientSiteViewModels!=null && clientSites.Report_ClientSiteViewModels.Any())
            {
                var report = clientSites.Report_ClientSiteViewModels.First();
                var clientSiteReport = new Report_Reports()
                {
                    Id = report.Id,
                    ClientSiteId = report.ClientSiteId,
                    IsActive = report.IsActive,
                    ReportName = report.ReportName,
                    ToolIds = JsonConvert.SerializeObject(report.ToolIds)
                };
                _report_ReportsRepository.Add(clientSiteReport);
            }

            _report_ClientSitesRepository.Save();
        }

        public void UpdateClientSite(Report_ClientSiteViewModel clientSites)
        {
            var clientSite = _report_ClientSitesRepository.FindFirstOrDefault(x => x.Id == clientSites.Id);
            if (clientSite != null)
            {
                clientSite.Id = clientSites.Id;
                clientSite.ClientId = clientSites.ClientId;
                clientSite.BrandName = clientSites.BrandName;
                clientSite.Name = clientSites.Name;
                clientSite.Description = clientSites.Description;
                clientSite.SiteUrl = clientSites.SiteUrl;
                clientSite.LogoUrl = clientSites.LogoUrl;
                clientSite.CrawlerConfig = clientSites.CrawlerConfig;

                _report_ClientSitesRepository.Edit(clientSite);

                if (clientSites.Report_ClientSiteViewModels != null && clientSites.Report_ClientSiteViewModels.Any())
                {
                    var report = clientSites.Report_ClientSiteViewModels.First();

                    var existReport = _report_ReportsRepository.FindFirstOrDefault(x => x.Id == report.Id);
                    if (existReport == null)
                    {
                        var clientSiteReport = new Report_Reports()
                        {
                            Id = report.Id,
                            ClientSiteId = report.ClientSiteId,
                            IsActive = report.IsActive,
                            ReportName = report.ReportName,
                            ToolIds = JsonConvert.SerializeObject(report.ToolIds)
                        };
                        _report_ReportsRepository.Add(clientSiteReport);
                    }
                    else
                    {
                        existReport.IsActive = report.IsActive;
                        existReport.ReportName = report.ReportName;
                        existReport.ToolIds = JsonConvert.SerializeObject(report.ToolIds);
                        _report_ReportsRepository.Edit(existReport);
                    }
                }

                _report_ClientSitesRepository.Save();
            }
        }

        public void DeleteClientSite(Guid clientSiteId)
        {
            var clientSite = _report_ClientSitesRepository.FindFirstOrDefault(x => x.Id == clientSiteId);
            if (clientSite != null)
            {
                _report_ClientSitesRepository.Delete(clientSite);
                _report_ClientSitesRepository.Save();
            }
        }        

        public IList<Report_Clients> GetReport_Clients()
        {
            return _report_ClientsRepository.GetAll();
        }

        public IList<Report_ClientSiteViewModel> GetReport_ClientSitesByClient(int clientId)
        {
            return _report_ClientSitesRepository.GetReport_ClientSiteViewModels(clientId);
        }

        public Report_Reports GetReport_ReportsByClientSiteId(Guid clientSiteId)
        {
            return _report_ReportsRepository.FindFirstOrDefault(x => x.ClientSiteId == clientSiteId);
        }

        public void AddServiceDesk(Report_ClientJiraServiceDesks serviceDesk)
        {
            _report_ClientJiraServiceDesksRepository.Add(serviceDesk);
            _report_ClientJiraServiceDesksRepository.Save();
        }

        public void UpdateServiceDesk(Report_ClientJiraServiceDesks serviceDesk)
        {
            var existServiceDesk = _report_ClientJiraServiceDesksRepository.FindFirstOrDefault(x => x.Id == serviceDesk.Id);

            if (existServiceDesk != null)
            {
                existServiceDesk.ServiceDeskUrl = serviceDesk.ServiceDeskUrl;
                existServiceDesk.ProjectKey = serviceDesk.ProjectKey;
                existServiceDesk.OrganizationName = serviceDesk.OrganizationName;
                existServiceDesk.OrgNameForProjectIfNotProvided = serviceDesk.OrgNameForProjectIfNotProvided;

                _report_ClientJiraServiceDesksRepository.Edit(existServiceDesk);
                _report_ClientJiraServiceDesksRepository.Save();
            }
        }

        public void DeleteServiceDesk(Guid serviceDeskId)
        {
            var serviceDesk = _report_ClientJiraServiceDesksRepository.FindFirstOrDefault(x => x.Id == serviceDeskId);
            if (serviceDesk != null)
            {
                _report_ClientJiraServiceDesksRepository.Delete(serviceDesk);
                _report_ClientJiraServiceDesksRepository.Save();
            }
        }

        public IList<Report_ClientJiraServiceDesks> GetReport_ClientJiraServiceDesks()
        {
            return _report_ClientJiraServiceDesksRepository.GetAll();
        }
    }
}
