using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Common.FileParser;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.ReportData.ScreamingFrog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class ScreamingFrog_ProcessorService : IScreamingFrog_ProcessorService
    {
        public string SpiderOutputFolder { get; set; }

        private readonly IAMSLogger _logger;
        private readonly IReport_ReportsRepository _report_ReportsRepository;
        private readonly IReport_ClientSitesRepository _report_ClientSitesRepository;
        private readonly IReportData_ScreamingFrogRepository _reportData_ScreamingFrogRepository;

        public ScreamingFrog_ProcessorService(IAMSLogger logger, IReport_ReportsRepository report_ReportsRepository, IReport_ClientSitesRepository report_ClientSitesRepository, IReportData_ScreamingFrogRepository reportData_ScreamingFrogRepository)
            : base()
        {
            _logger = logger;
            _report_ReportsRepository = report_ReportsRepository;
            _report_ClientSitesRepository = report_ClientSitesRepository;
            _reportData_ScreamingFrogRepository = reportData_ScreamingFrogRepository;
        }

        public void Process()
        {
            //Get the list of active clientSites to report from the database
            var listActiveReportsFromDB = _report_ReportsRepository.FindBy(x => x.IsActive == true);

            foreach (var report in listActiveReportsFromDB)
            {
                dynamic reportConfig = JsonConvert.DeserializeObject(report.ToolIds);

                if ((reportConfig.ScreamingFrogEnabled != null && ((string)reportConfig.ScreamingFrogEnabled).ToLower() != "false" && ((string)reportConfig.ScreamingFrogEnabled).ToLower() != "0"))
                {
                    ProcessAndSaveReportData(report.ClientSiteId.Value);
                }
            }
        }

        protected void ProcessAndSaveReportData(Guid clientSiteId)
        {
            //Scan folder SpiderOutputFolder and delete old data files
            var directories = Directory.GetDirectories(SpiderOutputFolder, "*" + clientSiteId.ToString(), SearchOption.TopDirectoryOnly);
            if (directories != null && directories.Any())
            {
                var dir = directories.OrderByDescending(x => Directory.GetCreationTimeUtc(x)).First();
                var subDirectories = Directory.GetDirectories(dir);
                //Process the latest 10 should be enough
                subDirectories = subDirectories.OrderByDescending(x => Directory.GetCreationTimeUtc(x)).Take(10).ToArray();
                IParser excelParser = new ExcelParser();
                var lstSfExcelRows = new List<ScreamingFrogExcelRow>();
                foreach (var subDir in subDirectories)
                {
                    //_logger.Information("Processing " + subDir + " " + Directory.GetCreationTimeUtc(subDir));
                    _logger.Information("Processing " + subDir);

                    //Avoid processing the data that was already processed for better system performance
                    var dataTimeUtc = Directory.GetCreationTimeUtc(subDir).ToUnixTimestamp();
                    var foundProcessedData = _reportData_ScreamingFrogRepository
                        .FindWhere(x => x.ReportTimeUtc == dataTimeUtc)
                        .FirstOrDefault();
                    if (foundProcessedData != null)
                    {
                        _logger.Information($"Found processed Screaming Frog data with ReportTimeUtc={dataTimeUtc}; so take it & skip processing!");
                        continue;
                    }

                    //Parse the excel file. Start with index = 1 because the first row (index = 0) is heading
                    List<List<CellModel>> excelInstance = excelParser.Parse(Path.Combine(subDir, "internal_all.xlsx"), null);                    
                    for (int i = 1; i < excelInstance.Count; i++)
                    {
                        List<CellModel> listCell = excelInstance[i];
                        var dataRow = new ScreamingFrogExcelRow();
                        dataRow.Address = listCell[0].Value?.ToString().Trim();
                        dataRow.ContentType = listCell[1].Value?.ToString().Trim();
                        dataRow.StatusCode = int.Parse(listCell[2].Value.ToString().Trim());
                        dataRow.Status = listCell[3].Value?.ToString().Trim();
                        dataRow.Indexability = listCell[4].Value?.ToString().Trim();
                        dataRow.IndexabilityStatus = listCell[5].Value?.ToString().Trim();
                        dataRow.Title1 = listCell[6].Value?.ToString().Trim();
                        dataRow.Title1Length = listCell[7].Value == null ? 0 : long.Parse("0" + listCell[7].Value.ToString().Trim());
                        dataRow.Title1PixelWidth = listCell[8].Value == null ? null : (long?)long.Parse("0" + listCell[8].Value.ToString().Trim());
                        dataRow.MetaDescription1 = listCell[9].Value?.ToString().Trim();
                        dataRow.MetaDescription1Length = listCell[10].Value == null ? 0 : long.Parse("0" + listCell[10].Value.ToString().Trim());
                        dataRow.MetaDescription1PixelWidth = listCell[11].Value == null ? null : (long?)long.Parse("0" + listCell[11].Value.ToString().Trim());
                        dataRow.MetaKeyword1 = listCell[12].Value?.ToString().Trim();
                        dataRow.MetaKeywords1Length = listCell[13].Value == null ? 0 : long.Parse("0" + listCell[13].Value.ToString().Trim());
                        dataRow.H11 = listCell[14].Value?.ToString().Trim();
                        dataRow.H11Length = listCell[15].Value == null ? 0 : long.Parse("0" + listCell[15].Value.ToString().Trim());
                        dataRow.H12 = listCell[16].Value?.ToString().Trim();
                        dataRow.H12Length = listCell[17].Value == null ? 0 : long.Parse("0" + listCell[17].Value.ToString().Trim());
                        dataRow.H21 = listCell[18].Value?.ToString().Trim();
                        dataRow.H21Length = listCell[19].Value == null ? 0 : long.Parse("0" + listCell[19].Value.ToString().Trim());
                        dataRow.H22 = listCell[20].Value?.ToString().Trim();
                        dataRow.H22Length = listCell[21].Value == null ? 0 : long.Parse("0" + listCell[21].Value.ToString().Trim());

                        dataRow.SizeInBytes = listCell[31].Value == null ? null : (double?)double.Parse("0" + listCell[31].Value.ToString().Trim());
                        dataRow.WordCount = listCell[32].Value == null ? null : (long?)long.Parse("0" + listCell[32].Value.ToString().Trim());
                        dataRow.TextRatio = listCell[33].Value == null ? null : (double?)double.Parse("0" + listCell[33].Value.ToString().Trim());
                        dataRow.CrawlDepth = listCell[34].Value == null ? null : (int?)long.Parse("0" + listCell[34].Value.ToString().Trim());

                        dataRow.Inlinks = listCell[36].Value == null ? null : (long?)long.Parse("0" + listCell[36].Value.ToString().Trim());
                        dataRow.UniqueInlinks = listCell[37].Value == null ? null : (long?)long.Parse("0" + listCell[37].Value.ToString().Trim());
                        dataRow.PercentageOfTotal = listCell[38].Value == null ? null : (double?)double.Parse("0" + listCell[38].Value.ToString().Trim());
                        dataRow.Outlinks = listCell[39].Value == null ? null : (long?)long.Parse("0" + listCell[39].Value.ToString().Trim());
                        dataRow.UniqueOutlinks = listCell[40].Value == null ? null : (long?)long.Parse("0" + listCell[40].Value.ToString().Trim());
                        dataRow.ExternalOutlinks = listCell[41].Value == null ? null : (long?)long.Parse("0" + listCell[41].Value.ToString().Trim());
                        dataRow.UniqueExternalOutlinks = listCell[42].Value == null ? null : (long?)long.Parse("0" + listCell[42].Value.ToString().Trim());

                        dataRow.ResponseTimeSec = listCell[48].Value == null ? null : (double?)double.Parse("0" + listCell[48].Value.ToString().Trim());

                        lstSfExcelRows.Add(dataRow);
                    }

                    //Create a structured reportable data
                    var returnData = new ScreamingFrogData();
                    returnData.ContentTypes = new ContentDistribution()
                    {
                        CountAll = lstSfExcelRows.Count(),
                        Html = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html")),
                        Css = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("css")),
                        Script = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("script")),
                        Img = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("image"))
                    };
                    returnData.Indexability = new Indexability()
                    {
                        CountIndexable = lstSfExcelRows.Count(x => x.Indexability == "Indexable"),
                        CountNonIndexable = lstSfExcelRows.Count(x => x.Indexability == "Indexable")
                    };
                    var lstNonIndexable = lstSfExcelRows
                        .Where(x => x.Indexability != "Indexable" && !string.IsNullOrWhiteSpace(x.IndexabilityStatus))
                        .GroupBy(x => x.IndexabilityStatus)
                        .Select(g => new KeyValuePair<string, long>(g.Key, g.Count()))
                        .ToList();
                    if (lstNonIndexable != null)
                    {
                        returnData.Indexability.NonIndexableReasons = new Dictionary<string, long>();
                        foreach (var cd in lstNonIndexable)
                        {
                            returnData.Indexability.NonIndexableReasons.Add(cd.Key, cd.Value);
                        }
                    }
                    returnData.HttpStatusCodes = new HttpStatusCodes()
                    {
                        CountSuccess = lstSfExcelRows.Count(x => x.StatusCode >= 200 && x.StatusCode < 300),
                        CountRedirect = lstSfExcelRows.Count(x => x.StatusCode >= 300 && x.StatusCode < 400),
                        CountNotOk = lstSfExcelRows.Count(x => x.StatusCode >= 400),
                        TopUrlsNotOk = lstSfExcelRows.Where(x => x.StatusCode >= 300)
                        .OrderByDescending(x => x.StatusCode)
                        .Take(50)
                        .Select(x => new UrlStatusCodeNotOk()
                        {
                            Url = x.Address,
                            Code = x.StatusCode,
                            Desc = x.Status
                        }).ToList()
                    };
                    returnData.TopLargestResources = lstSfExcelRows
                        .Where(x => x.SizeInBytes != null)
                        .OrderByDescending(x => x.SizeInBytes)
                        .Take(20)
                        .Select(x => new UrlWithDetails() {
                            Url = x.Address,
                            Title1 = x.Title1,
                            Type = x.ContentType,
                            Bytes = x.SizeInBytes.Value,
                            RespSec = x.ResponseTimeSec,
                            Inlinks = x.Inlinks,
                            UniqueInlinks = x.UniqueInlinks,
                            //PercentageOfTotal = x.PercentageOfTotal,
                            Outlinks = x.Outlinks,
                            UniqueOutlinks = x.UniqueOutlinks,
                            ExtOutlinks = x.ExternalOutlinks,
                            UniqueExtOutlinks = x.UniqueExternalOutlinks,
                            WordCount = x.WordCount,
                            TextRatio = x.TextRatio
                        }).ToList();
                    var lstCrawlDepth = lstSfExcelRows
                        .Where(x => x.CrawlDepth != null)
                        .GroupBy(x => x.CrawlDepth)
                        .Select(g => new KeyValuePair<int, long>(g.Key.Value, g.Count()))
                        .ToList();
                    if (lstCrawlDepth != null)
                    {
                        returnData.CrawlDepth = new Dictionary<int, long>();
                        foreach(var cd in lstCrawlDepth)
                        {
                            returnData.CrawlDepth.Add(cd.Key, cd.Value);
                        }
                    }
                    returnData.TopUrlsWithInlinks = lstSfExcelRows
                        .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && x.Inlinks != null)
                        .OrderByDescending(x => x.Inlinks)
                        .Take(10)
                        .Select(x => new UrlWithDetails()
                        {
                            Url = x.Address,
                            Title1 = x.Title1,
                            Type = x.ContentType,
                            Bytes = x.SizeInBytes.Value,
                            RespSec = x.ResponseTimeSec,
                            Inlinks = x.Inlinks,
                            UniqueInlinks = x.UniqueInlinks,
                            //PercentageOfTotal = x.PercentageOfTotal,
                            Outlinks = x.Outlinks,
                            UniqueOutlinks = x.UniqueOutlinks,
                            ExtOutlinks = x.ExternalOutlinks,
                            UniqueExtOutlinks = x.UniqueExternalOutlinks,
                            WordCount = x.WordCount,
                            TextRatio = x.TextRatio
                        }).ToList();
                    returnData.TopUrlsWithWords = lstSfExcelRows
                        .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && x.WordCount != null)
                        .OrderByDescending(x => x.WordCount)
                        .Take(10)
                        .Select(x => new UrlWithDetails()
                        {
                            Url = x.Address,
                            Title1 = x.Title1,
                            Type = x.ContentType,
                            Bytes = x.SizeInBytes.Value,
                            RespSec = x.ResponseTimeSec,
                            Inlinks = x.Inlinks,
                            UniqueInlinks = x.UniqueInlinks,
                            //PercentageOfTotal = x.PercentageOfTotal,
                            Outlinks = x.Outlinks,
                            UniqueOutlinks = x.UniqueOutlinks,
                            ExtOutlinks = x.ExternalOutlinks,
                            UniqueExtOutlinks = x.UniqueExternalOutlinks,
                            WordCount = x.WordCount,
                            TextRatio = x.TextRatio
                        }).ToList();
                    returnData.ContentSEO = new ContentSEO()
                    {
                        CountUrls = lstSfExcelRows.Count(),
                        //Title missing
                        CountTitle1Missing = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && x.Title1Length <= 0),
                        Top20UrlsTitle1Missing = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.Title1Length <= 0
                            ))
                            .Take(20)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),
                        //Title duplicate
                        CountTitle1Duplicate = lstSfExcelRows.Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && x.Title1Length > 0).GroupBy(x => x.Title1).Count(g => g.Count() > 1),
                        Top10UrlsTitle1Duplicate = lstSfExcelRows.Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && x.Title1Length > 0).GroupBy(x => x.Title1)
                            .Where(g => g.Count() > 1)
                            .Take(10)
                            .Select(g => g.First().ToUrlWithContentInfo())
                            .ToList(),
                        //Title over 60 chars
                        CountTitle1Over60Chars = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.Title1Length > 60),
                        Top10UrlsTitle1Over60Chars = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.Title1Length > 60
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),
                        //Title below 30 chars
                        CountTitle1Below30Chars = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.Title1Length < 30),
                        Top10UrlsTitle1Below30Chars = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.Title1Length < 30
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),

                        //MetaDesc missing
                        CountMetaDescMissing = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.MetaDescription1Length <= 0),
                        Top10UrlsMetaDescMissing = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.MetaDescription1Length <= 0
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),
                        //MetaDesc duplicate
                        CountMetaDescDuplicate = lstSfExcelRows.Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && x.MetaDescription1Length > 0).GroupBy(x => x.MetaDescription1).Count(g => g.Count() > 1),
                        Top10UrlsMetaDescDuplicate = lstSfExcelRows.Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && x.MetaDescription1Length > 0).GroupBy(x => x.MetaDescription1)
                            .Where(g => g.Count() > 1)
                            .Take(10)
                            .Select(g => g.First().ToUrlWithContentInfo())
                            .ToList(),
                        //MetaDesc over 155 chars
                        CountMetaDescOver155Chars = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.MetaDescription1Length > 155),
                        Top10UrlsMetaDescOver155Chars = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.MetaDescription1Length > 155
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),
                        //MetaDesc below 70 chars
                        CountMetaDescBelow70Chars = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.MetaDescription1Length < 70),
                        Top10UrlsMetaDescBelow70Chars = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.MetaDescription1Length < 70
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),

                        //H11 missing
                        CountH11Missing = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.H11Length <= 0),
                        Top10UrlsH11Missing = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.H11Length <= 0
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),
                        //H11 duplicate
                        CountH11Duplicate = lstSfExcelRows.Where(x => x.H11Length > 0).GroupBy(x => x.H11).Count(g => g.Count() > 1),
                        //H11 over 70 chars
                        CountH11Over70Chars = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.H11Length > 70),
                        Top10UrlsH11Over70Chars = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.H11Length > 70
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),

                        //H21 missing
                        CountH21Missing = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.H21Length <= 0),
                        Top10UrlsH21Missing = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.H21Length <= 0
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList(),
                        //H21 duplicate
                        CountH21Duplicate = lstSfExcelRows.Where(x => x.H21Length > 0).GroupBy(x => x.H21).Count(g => g.Count() > 1),
                        //H21 over 70 chars
                        CountH21Over70Chars = lstSfExcelRows.Count(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") &&  x.H21Length > 70),
                        Top10UrlsH21Over70Chars = lstSfExcelRows
                            .Where(x => x.ContentType != null && x.ContentType.ToLower().Contains("text/html") && (
                                x.H21Length > 70
                            ))
                            .Take(10)
                            .Select(x => x.ToUrlWithContentInfo())
                            .ToList()
                    };

                    //Save processed data into DB
                    var reportData_ScreamingFrog = new ReportData_ScreamingFrog
                    {
                        Id = Guid.NewGuid(),
                        ClientSiteId = clientSiteId,
                        Json = JsonConvert.SerializeObject(returnData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        ReportTimeUtc = dataTimeUtc,
                        GeneratedTimestamp = DateTime.Now
                    };
                    var existedData = _reportData_ScreamingFrogRepository
                        .FindFirstOrDefault(x => x.ClientSiteId == reportData_ScreamingFrog.ClientSiteId && x.ReportTimeUtc == reportData_ScreamingFrog.ReportTimeUtc);
                    if (existedData != null)
                    {
                        existedData.Json = reportData_ScreamingFrog.Json;
                        existedData.GeneratedTimestamp = reportData_ScreamingFrog.GeneratedTimestamp;
                        _reportData_ScreamingFrogRepository.Edit(existedData);
                    }
                    else
                    {
                        _reportData_ScreamingFrogRepository.Add(reportData_ScreamingFrog);
                    }
                    _reportData_ScreamingFrogRepository.Save();

                    //Sleep a little bit to cool CPU% down
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _reportData_ScreamingFrogRepository.BatchDelete(x => x.ReportTimeUtc < retentionTimeCalc);
        }        
    }

    internal class ScreamingFrogExcelRow
    {
        public string Address;
        public string ContentType;
        public int StatusCode;
        public string Status;
        public string Indexability;
        public string IndexabilityStatus;
        public string Title1;
        public long Title1Length;
        public long? Title1PixelWidth;
        public string MetaDescription1;
        public long MetaDescription1Length;
        public long? MetaDescription1PixelWidth;
        public string MetaKeyword1;
        public long MetaKeywords1Length;
        public string H11;
        public long H11Length;
        public string H12;
        public long H12Length;
        public string H21;
        public long H21Length;
        public string H22;
        public long H22Length;
        //Meta Robots 1
        //X-Robots-Tag 1
        //Meta Refresh 1
        //Canonical Link Element 1
        //rel="next" 1
        //rel="prev" 1
        //HTTP rel = "next" 1
        //HTTP rel = "prev" 1
        //amphtml Link Element
        public double? SizeInBytes;
        public long? WordCount;
        public double? TextRatio;
        public int? CrawlDepth;
        //Link Score
        public long? Inlinks;
        public long? UniqueInlinks;
        public double? PercentageOfTotal;
        public long? Outlinks;
        public long? UniqueOutlinks;
        public long? ExternalOutlinks;
        public long? UniqueExternalOutlinks;
        //Closest Similarity Match
        //No.Near Duplicates
        //Spelling Errors
        //Grammar Errors
        //Hash
        public double? ResponseTimeSec;
        //Last Modified
        //Redirect URL
        //Redirect Type
        //Cookies
        //HTTP Version
        //URL Encoded Address

        public UrlWithContentInfo ToUrlWithContentInfo()
        {
            return new UrlWithContentInfo()
            {
                Url = this.Address,
                Title1 = this.Title1,
                Title1Length = this.Title1Length,
                MetaDesc1 = this.MetaDescription1,
                MetaDesc1Length = this.MetaDescription1Length,
                H11 = this.H11,
                H11Length = this.H11Length,
                H21 = this.H21,
                H21Length = this.H21Length
            };
        }
    }
}