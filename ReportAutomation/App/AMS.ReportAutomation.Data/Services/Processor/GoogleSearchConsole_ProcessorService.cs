using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;
using AMS.ReportAutomation.ReportData.GoogleSearch;
using Google.Apis.Webmasters.v3.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class GoogleSearchConsole_ProcessorService : IGoogleSearchConsole_ProcessorService
    {
        private readonly IAMSLogger _logger;

        private readonly IReport_ReportsRepository _report_ReportsRepository;

        private readonly IGoogleSearchConsole_Repository _googleSearchConsoleRepository;

        private readonly IGoogleSearchConsole_DetailRepository _googleSearchConsoleDetailRepository;

        private readonly IGoogleSearchConsole_SitemapRepository _googleSearchConsoleSitemapRepository;

        private readonly IReportData_GoogleSearchConsoleRepository _reportData_GoogleSearchConsoleRepository;

        private readonly IReportData_GoogleSearchConsoleSitemapRepository _reportData_GoogleSearchConsoleSitemapRepository;

        private readonly IReport_ClientSitesRepository _report_ClientSitesRepository;

        public GoogleSearchConsole_ProcessorService(IAMSLogger logger, IReport_ReportsRepository report_ReportsRepository, IGoogleSearchConsole_Repository googleSearchConsoleRepository, IReportData_GoogleSearchConsoleRepository reportData_GoogleSearchConsoleRepository, IGoogleSearchConsole_DetailRepository googleSearchConsoleDetailRepository, IReport_ClientSitesRepository report_ClientSitesRepository,
            IGoogleSearchConsole_SitemapRepository googleSearchConsoleSitemapRepository, IReportData_GoogleSearchConsoleSitemapRepository reportData_GoogleSearchConsoleSitemapRepository) : base()
        {
            this._logger = logger;
            this._report_ReportsRepository = report_ReportsRepository;
            this._googleSearchConsoleRepository = googleSearchConsoleRepository;
            this._googleSearchConsoleDetailRepository = googleSearchConsoleDetailRepository;
            this._googleSearchConsoleSitemapRepository = googleSearchConsoleSitemapRepository;
            this._reportData_GoogleSearchConsoleRepository = reportData_GoogleSearchConsoleRepository;
            this._report_ClientSitesRepository = report_ClientSitesRepository;
            this._reportData_GoogleSearchConsoleSitemapRepository = reportData_GoogleSearchConsoleSitemapRepository;
        }

        public void Process()
        {
            //Get the list of active clientSites to report from the database
            var listActiveReportsFromDB = _report_ReportsRepository.FindBy(x => x.IsActive == true);

            foreach (var report in listActiveReportsFromDB)
            {
                dynamic reportConfig = JsonConvert.DeserializeObject(report.ToolIds);

                if (reportConfig.GoogleAnalyticEnabled != null
                    && ((string)reportConfig.GoogleAnalyticEnabled).ToLower() != "false"
                    && ((string)reportConfig.GoogleAnalyticEnabled).ToLower() != "0")
                {
                    ProcessSaveReportData(report.ClientSiteId.Value);

                    ProcessSaveReportSitemapData(report.ClientSiteId.Value);
                }
            }
        }

        protected void ProcessSaveReportData(Guid clientSiteId)
        {
            var utcNow = DateTime.UtcNow;
            var endOfYesterdayUtc = (new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0)).AddSeconds(-1);
            //We don't need to process all records, particularly that old data will still be correct as the analytics was already done for the past days, and also the database will grow very big so the number of crawled records will be big
            //Even if the processor does not run often (let say it is scheduled to run weekly) => it is good enough to process the data for the last 7 days, particularly each data record contained data of multiple days in 1 month.
            const int maxDaysToProcess = 7;
            var maxOldUnixDayToProcess = endOfYesterdayUtc.AddDays(-maxDaysToProcess).ToUnixTimestamp();

            var rawDataRecords = _googleSearchConsoleRepository.FindBy(x => x.ClientSiteId == clientSiteId && x.DataTimeUtc != null && x.DataTimeUtc >= maxOldUnixDayToProcess);
            if (rawDataRecords != null)
            {
                //Group data records by DataTimeUtc
                var rawDataList = rawDataRecords.GroupBy(x => x.DataTimeUtc).ToList();

                //Process data for each DataTimeUtc
                foreach (var dataList in rawDataList)
                {
                    if (!dataList.Any()) continue;

                    var dataTimeUtc = dataList.Key;

                    var lstReturnData = new List<GoogleSearchData>();

                    foreach (var keywordsRawData in dataList)
                    {
                        foreach (var keywordDetailRow in keywordsRawData.Data_GoogleSearch_Detail)
                        {
                            var lstReportModel = new List<GoogleSearchModel>();

                            var returnData = new GoogleSearchData();

                            var reportDataRows = JsonConvert.DeserializeObject<List<ApiDataRow>>(keywordDetailRow.Json);

                            reportDataRows.ForEach(x => lstReportModel.Add(new GoogleSearchModel
                            {
                                Date = x.Keys[0],
                                Page = x.Keys[1],
                                Device = x.Keys[2],
                                Click = x.Clicks,
                                Impression = x.Impressions
                            }));

                            returnData.Keyword = keywordDetailRow.Keyword;
                            //returnData.Data = lstReportModel.OrderByDescending(x => x.Click).ToList();
                            returnData.Data = lstReportModel;
                            lstReturnData.Add(returnData);
                        }
                    }

                    //Save processed data into DB
                    var reportData_GoogleSearch = new ReportData_GoogleSearch
                    {
                        Id = Guid.NewGuid(),
                        ClientSiteId = clientSiteId,
                        //TODO: To delete Keyword column later
                        //Keyword = string.Join(",", lstReturnData.Select(x => x.Keyword)),
                        Json = JsonConvert.SerializeObject(lstReturnData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        ReportTimeUtc = dataTimeUtc,
                        GeneratedTimestamp = DateTime.Now
                    };
                    var existedData = _reportData_GoogleSearchConsoleRepository
                        .FindFirstOrDefault(x => x.ClientSiteId == reportData_GoogleSearch.ClientSiteId && x.ReportTimeUtc == reportData_GoogleSearch.ReportTimeUtc);
                    if (existedData != null)
                    {
                        existedData.Json = reportData_GoogleSearch.Json;
                        existedData.GeneratedTimestamp = reportData_GoogleSearch.GeneratedTimestamp;
                        _reportData_GoogleSearchConsoleRepository.Edit(existedData);
                    }
                    else
                    {
                        _reportData_GoogleSearchConsoleRepository.Add(reportData_GoogleSearch);
                    }
                    _reportData_GoogleSearchConsoleRepository.Save();
                }
            }
        }

        protected void ProcessSaveReportSitemapData(Guid clientSiteId)
        {
            var utcNow = DateTime.UtcNow;
            var endOfYesterdayUtc = (new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0)).AddSeconds(-1);
            const int maxDaysToProcess = 7;
            var maxOldUnixDayToProcess = endOfYesterdayUtc.AddDays(-maxDaysToProcess).ToUnixTimestamp();

            var rawDataRecords = _googleSearchConsoleSitemapRepository.FindBy(x => x.ClientSiteId == clientSiteId && x.DataTimeUtc != null && x.DataTimeUtc >= maxOldUnixDayToProcess);
            if (rawDataRecords != null)
            {
                //Group data records by DataTimeUtc
                var rawDataList = rawDataRecords.GroupBy(x => x.DataTimeUtc).ToList();

                //Process data for each DataTimeUtc
                foreach (var dataList in rawDataList)
                {
                    if (!dataList.Any()) continue;

                    var dataTimeUtc = dataList.Key;

                    var lstReportModel = new List<GoogleSearchSitemapItemModel>();

                    foreach (var rawData in dataList)
                    {
                        var reportDataRows = JsonConvert.DeserializeObject<List<WmxSitemap>>(rawData.Json);

                        reportDataRows.ForEach(x => lstReportModel.Add(new GoogleSearchSitemapItemModel
                        {
                            Path = x.Path,
                            Type = x.Type,
                            LastDownloaded = x.LastDownloaded,
                            LastSubmitted = x.LastSubmitted,
                            Error = x.Errors,
                            Warning = x.Warnings
                        }));
                    }

                    //Save processed data into DB
                    var reportData_GoogleSearchSiteMap = new ReportData_GoogleSearch_Sitemap
                    {
                        Id = Guid.NewGuid(),
                        ClientSiteId = clientSiteId,
                        Json = JsonConvert.SerializeObject(lstReportModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        ReportTimeUtc = dataTimeUtc,
                        GeneratedTimestamp = DateTime.Now
                    };
                    var existedData = _reportData_GoogleSearchConsoleSitemapRepository
                        .FindFirstOrDefault(x => x.ClientSiteId == reportData_GoogleSearchSiteMap.ClientSiteId && x.ReportTimeUtc == reportData_GoogleSearchSiteMap.ReportTimeUtc);
                    if (existedData != null)
                    {
                        existedData.Json = reportData_GoogleSearchSiteMap.Json;
                        existedData.GeneratedTimestamp = reportData_GoogleSearchSiteMap.GeneratedTimestamp;
                        _reportData_GoogleSearchConsoleSitemapRepository.Edit(existedData);
                    }
                    else
                    {
                        _reportData_GoogleSearchConsoleSitemapRepository.Add(reportData_GoogleSearchSiteMap);
                    }
                    _reportData_GoogleSearchConsoleSitemapRepository.Save();
                }
            }
        }

        public List<GoogleSearchData> GetGoogleSearchReportDataByClientId(Guid clientId, long reportToDateTime)
        {
            var retData = new List<GoogleSearchData>();
            var reportData = _reportData_GoogleSearchConsoleRepository
                .FindWhere(x => x.ClientSiteId == clientId && x.ReportTimeUtc <= reportToDateTime)
                .OrderByDescending(x => x.ReportTimeUtc)
                .ThenBy(x => x.GeneratedTimestamp)
                .FirstOrDefault();
            if (reportData != null && reportData.Json != null)
            {
                retData = JsonConvert.DeserializeObject<List<GoogleSearchData>>(reportData.Json);
                retData.ForEach(x =>
                {
                    if (x != null && reportData.ReportTimeUtc != null) x.ConsolidatedTimeUtc = reportData.ReportTimeUtc.Value.UnixTimeStampToDateTimeUtc();
                });

                return retData;
            }
            return null;
        }

        public List<GoogleSearchSitemapModel> GetGoogleSearchSitemapReportDataByClientId(Guid clientId, long reportToDateTime)
        {
            var lstResult = new List<GoogleSearchSitemapModel>();

            var reportData = _reportData_GoogleSearchConsoleSitemapRepository
                .FindWhere(x => x.ClientSiteId == clientId && x.ReportTimeUtc <= reportToDateTime)
                .OrderByDescending(x => x.ReportTimeUtc)
                .ThenBy(x => x.GeneratedTimestamp).ToList();

            if (reportData.SafeAny())
            {
                foreach (var reportDataGoogleSearchSitemap in reportData)
                {
                    lstResult.Add(new GoogleSearchSitemapModel { DataTime = reportDataGoogleSearchSitemap.ReportTimeUtc, Items = JsonConvert.DeserializeObject<List<GoogleSearchSitemapItemModel>>(reportDataGoogleSearchSitemap.Json)});
                }
            }

            return lstResult;
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _reportData_GoogleSearchConsoleRepository.BatchDelete(x => x.ReportTimeUtc < retentionTimeCalc);
        }
    }
}