using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Newtonsoft.Json;
using PingdomClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AMS.ReportAutomation.Common.FileParser;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.ReportData.Dynomapper;
using AMS.ReportAutomation.ReportData.ScreamingFrog;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class Dynomapper_ProcessorService : IDynomapper_ProcessorService
    {
        public string OutputFolder { get; set; }
        private readonly IAMSLogger _logger;
        private readonly IDynomapper_Repository _dynomapperRepository;
        private readonly IReport_ReportsRepository _report_ReportsRepository;
        private readonly IReportData_DynomapperRepository _reportData_DynomapperRepository;

        public Dynomapper_ProcessorService(IAMSLogger logger, IDynomapper_Repository dynomapperRepository, IReport_ReportsRepository report_ReportsRepository, IReportData_DynomapperRepository reportData_DynomapperRepository)
            : base()
        {
            _logger = logger;
            _dynomapperRepository = dynomapperRepository;
            _report_ReportsRepository = report_ReportsRepository;
            _reportData_DynomapperRepository = reportData_DynomapperRepository;
        }

        public void Process()
        {
            var listActiveReportsFromDB = _report_ReportsRepository.FindBy(x => x.IsActive == true);

            foreach (var report in listActiveReportsFromDB)
            {
                dynamic reportConfig = JsonConvert.DeserializeObject(report.ToolIds);

                if ((reportConfig.DynomapperEnabled != null && ((string)reportConfig.DynomapperEnabled).ToLower() != "false" && ((string)reportConfig.DynomapperEnabled).ToLower() != "0"))
                {
                    ProcessAndSaveReportData(report.ClientSiteId.Value);
                }
            }
        }

        protected void ProcessAndSaveReportData(Guid clientSiteId)
        {
            var directories = Directory.GetDirectories(OutputFolder, "*" + clientSiteId.ToString(), SearchOption.TopDirectoryOnly);
            if (directories != null && directories.Any())
            {
                var dir = directories.OrderByDescending(x => Directory.GetCreationTimeUtc(x)).First();
                var subDirectories = Directory.GetDirectories(dir);
                subDirectories = subDirectories.OrderByDescending(x => Directory.GetCreationTimeUtc(x)).Take(10).ToArray();
                foreach (var subDir in subDirectories)
                {
                    _logger.Information("Processing " + subDir);

                    var dataTimeUtc = Directory.GetCreationTimeUtc(subDir).ToUnixTimestamp();
                    var foundProcessedData = _reportData_DynomapperRepository
                        .FindWhere(x => x.ReportTimeUtc == dataTimeUtc)
                        .FirstOrDefault();
                    if (foundProcessedData != null)
                    {
                        _logger.Information($"Found processed Dynomapper data with ReportTimeUtc={dataTimeUtc}; so take it & skip processing!");
                        continue;
                    }

                    var returnData = new DynomapperData();
                    if (File.Exists($"{subDir}\\{clientSiteId}_HomePage.pdf"))
                    {
                        returnData.HomePageUrl = $"../{new DirectoryInfo(OutputFolder).Name}/{Directory.GetParent(subDir).Name}/{new DirectoryInfo(subDir).Name}/{clientSiteId}_HomePage.pdf";
                    }
                    if (File.Exists($"{subDir}\\{clientSiteId}_SummaryReport.pdf"))
                    {
                        returnData.SummaryUrl = $"../{new DirectoryInfo(OutputFolder).Name}/{Directory.GetParent(subDir).Name}/{new DirectoryInfo(subDir).Name}/{clientSiteId}_SummaryReport.pdf";
                    }

                    if (File.Exists($"{subDir}\\{clientSiteId}.html"))
                    {
                        returnData.Content = File.ReadAllText($"{subDir}\\{clientSiteId}.html");
                    }

                    var reportData_Dynomapper = new ReportData_Dynomapper
                    {
                        Id = Guid.NewGuid(),
                        ClientSiteId = Guid.Parse("af418ff1-9290-49c3-aea5-1a4235179fa1"),
                        Json = JsonConvert.SerializeObject(returnData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        ReportTimeUtc = dataTimeUtc,
                        GeneratedTimestamp = DateTime.Now
                    };
                    var existedData = _reportData_DynomapperRepository
                        .FindFirstOrDefault(x => x.ClientSiteId == reportData_Dynomapper.ClientSiteId && x.ReportTimeUtc == reportData_Dynomapper.ReportTimeUtc);
                    if (existedData != null)
                    {
                        existedData.Json = reportData_Dynomapper.Json;
                        existedData.GeneratedTimestamp = reportData_Dynomapper.GeneratedTimestamp;
                        _reportData_DynomapperRepository.Edit(existedData);
                    }
                    else
                    {
                        _reportData_DynomapperRepository.Add(reportData_Dynomapper);
                    }
                    _reportData_DynomapperRepository.Save();

                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _reportData_DynomapperRepository.BatchDelete(x => x.ReportTimeUtc < retentionTimeCalc);
        }
    }
}