using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Newtonsoft.Json;
using PingdomClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using System.Management.Instrumentation;
using System.Management.Automation.Runspaces;
using System.Collections;
using System.Runtime.CompilerServices;
using AMS.ReportAutomation.Data.Exceptions;
using AMS.Selenium.Config;
using AMS.Selenium.DynoMapper;
using AMS.Selenium.DynoMapper.Interfaces;

namespace AMS.ReportAutomation.Data.Services.Crawler
{
    public class DynoMapperSelenium_Service : EntityService<Dynomapper>, IDynoMapperSelenium_Service
    {
        protected readonly IAMSLogger _logger;
        private readonly IDynomapper_Repository _dynomapperRepository;
        private readonly IReport_ClientSitesRepository _reportClientSitesRepository;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DataFolder { get; set; }
        public string OutputFolder { get; set; }
        private DynomapperDownloadReport _dynomapper;

        public DynomapperDownloadReport DynomapperCraw
        {
            get
            {
                if (_dynomapper == null)
                {
                    _dynomapper = new DynomapperDownloadReport(UserName, Password, new WorkSpaceFolder(DataFolder, OutputFolder));
                }

                return _dynomapper;
            }
        }


        public DynoMapperSelenium_Service(IAMSLogger logger, IDynomapper_Repository dynomapperRepository, IReport_ClientSitesRepository reportClientSitesRepository) : base(dynomapperRepository, logger)
        {
            _logger = logger;
            _dynomapperRepository = dynomapperRepository;
            _reportClientSitesRepository = reportClientSitesRepository;
        }

        public void Crawl()
        {
            if (string.IsNullOrWhiteSpace(OutputFolder))
                throw new Exception("You must configure OutputFolder before crawling.");

            try
            {
                //Get list project and save to DB
                var projects = GetListProjectAndSaveToDB();

                // Download detail report
                if (projects != null && projects.Any())
                {
                    foreach (var project in projects)
                    {
                        DynomapperCraw.DownloadReportForProject(project.ProjectName,project.ClientSiteId.Value);

                        DynomapperCraw.DownloadBoardChartForProject(project.ProjectName, project.ClientSiteId.Value);

                        DynomapperCraw.DownloadChartForProject(project.ProjectName, project.ClientSiteId.Value);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime);

            var directories = Directory.GetDirectories(OutputFolder);
            if (directories != null)
            {
                foreach (var dir in directories)
                {
                    var subDirectories = Directory.GetDirectories(dir);
                    foreach (var subDir in subDirectories)
                    {
                        if (Directory.GetCreationTimeUtc(subDir) < retentionTimeCalc)
                        {
                            Directory.Delete(subDir, true);
                        }
                    }
                }
            }
        }

        public List<Dynomapper> GetListProjectAndSaveToDB()
        {
            var projects = DynomapperCraw.GetListingProjects();

            if (projects != null && projects.Any())
            {
                foreach (var project in projects)
                {
                    Dynomapper dynomapper;

                    var existedData = _dynomapperRepository.FindFirstOrDefault(x => x.ProjectName == project);
                    if (existedData == null)
                    {
                        dynomapper = new Dynomapper
                        {
                            Id = Guid.NewGuid(),
                            ProjectName = project,
                            ClientSiteId = _reportClientSitesRepository.FindFirstOrDefault(x => x.SiteUrl == project)?.Id,
                            LatestRunDate = DynomapperCraw.GetLatestRunDate(project)
                        };

                        _dynomapperRepository.Add(dynomapper);
                    }
                    else
                    {
                        existedData.ClientSiteId = _reportClientSitesRepository
                            .FindFirstOrDefault(x => x.SiteUrl == project)?.Id;
                        existedData.LatestRunDate = DynomapperCraw.GetLatestRunDate(project);
                        _dynomapperRepository.Edit(existedData);
                    }

                    this.SafeExecute(() => _dynomapperRepository.Save());
                }
            }
            return _dynomapperRepository.FindWhere(x=>x.ClientSiteId !=null).ToList();
        }

    }
}