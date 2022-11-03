using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using Newtonsoft.Json;
using PingdomClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;

namespace AMS.ReportAutomation.Data.Services.Crawler
{
    public class Pingdom_CheckService : EntityService<Data_Pingdom_Check>, IPingdom_CheckService
    {
        public class PingdomClientException : Exception
        {
            public PingdomClientException(string message) : base(message) { }
        }

        private readonly IPingdom_CheckRepository _pingdom_CheckRepository;
        private readonly IPingdom_CheckSummaryRepository _pingdom_CheckSummaryRepository;
        //Multi-threading
        private readonly object dbOperationThreadLock = new object();

        public Pingdom_CheckService(IPingdom_CheckRepository pingdom_CheckRepository, IPingdom_CheckSummaryRepository pingdom_CheckSummaryRepository, IAMSLogger logger)
            : base(pingdom_CheckRepository, logger)
        {
            _pingdom_CheckRepository = pingdom_CheckRepository;
            _pingdom_CheckSummaryRepository = pingdom_CheckSummaryRepository;
        }

        public Data_Pingdom_Check GetById(long id)
        {
            return _pingdom_CheckRepository.FindFirstOrDefault(x => x.Id == id);
        }

        //public void AddPingDomCheckDetail(Data_Pingdom_CheckDetail checkDetail)
        //{
        //    _pingdom_CheckDetailRepository.Add(checkDetail);
        //}

        public void GetPingDomCheckAndSaveToDB()
        {
            var allChecks = GetPingdomChecksList();
            SavePingDomCheck(allChecks);
        }

        public List<int> GetListIdPingDomCheckAndSaveToDB()
        {
            var allChecks = GetPingdomChecksList();
            SavePingDomCheck(allChecks);
            return allChecks.Select(x => x.Id)?.ToList();
        }

        private void SavePingDomCheck(IEnumerable<PingdomClient.Contracts.Check> allChecks)
        {
            if (allChecks != null && allChecks.Any())
            {
                foreach (var check in allChecks)
                {
                    Data_Pingdom_Check pingdom;

                    var existedData = _pingdom_CheckRepository.FindFirstOrDefault(x => x.Id == check.Id);
                    if (existedData == null)
                    {
                        pingdom = new Data_Pingdom_Check()
                        {
                            Id = check.Id,
                            HostName = check.HostName,
                            Name = check.Name,
                            LastTestTimeUtc = check.LastTestTime,
                            Json = JsonConvert.SerializeObject(check, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                            CrawledTimestamp = DateTime.Now
                        };

                        _pingdom_CheckRepository.Add(pingdom);
                    }
                    else
                    {
                        existedData.HostName = check.HostName;
                        existedData.Name = check.Name;
                        existedData.LastTestTimeUtc = check.LastTestTime;
                        existedData.Json = JsonConvert.SerializeObject(check);
                        existedData.CrawledTimestamp = DateTime.Now;
                        _pingdom_CheckRepository.Edit(existedData);
                    }

                    this.SafeExecute(() => _pingdom_CheckRepository.Save());
                }
            }
        }

        //public void GetPingDomCheckDetailAndSaveToDB(long checkId)
        //{
        //    var checkDetails = GetPingdomActionsList(checkId.ToString());

        //    if (checkDetails != null && checkDetails.Any())
        //    {
        //        foreach (var checkDetail in checkDetails)
        //        {
        //            var pingdomDetail = new Data_Pingdom_CheckDetail()
        //            {
        //                Id = Guid.NewGuid(),
        //                CheckId = checkId,
        //                Time = checkDetail.Time,
        //                CrawledTimestamp = DateTime.Now,
        //                Json = JsonConvert.SerializeObject(checkDetail, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore })
        //            };
        //            _pingdom_CheckDetailRepository.Add(pingdomDetail);
        //            this.SafeExecute(() => _pingdom_CheckDetailRepository.Save());
        //        }
        //    }
        //}

        public void GetPingDomSummaryAndSaveToDB(long checkId)
        {
            //var checkSummaryByDay = GetSummaryPerformance_ResolutionDay(int.Parse(checkId.ToString()));
            //SaveSummaryPerformanceToDB(checkId, checkSummaryByDay);
            var checkSummaryByHour = GetSummaryPerformance_ResolutionHour(int.Parse(checkId.ToString()));
            SaveSummaryPerformanceToDB(checkId, checkSummaryByHour);
        }

        private IEnumerable<PingdomClient.Contracts.Check> GetPingdomChecksList()
        {
            var result = Pingdom.Client.Checks.GetChecksList().Result;
            if (result.HasErrors)
            {
                throw new PingdomClientException($"Cannot get Get Pingdom Checks List: {result.ErrorMessage}");
            }
            return result.Checks;
        }

        //private IEnumerable<PingdomClient.Contracts.Alert> GetPingdomActionsList(string checkId)
        //{
        //    var args = new PingdomClient.Contracts.ActionArgs
        //    {
        //        CheckIds = checkId,
        //        From = DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)).ToUnixTimestamp(),
        //        To = DateTime.UtcNow.ToUnixTimestamp()
        //    };
        //    var result = Pingdom.Client.Actions.GetActionsList(args).Result;
        //    if (result.HasErrors)
        //    {
        //        throw new PingdomClientException($"Cannot get Get Pingdom Actions List: {result.ErrorMessage}");
        //    }
        //    return result.Actions.Alerts.OrderByDescending(x => x.Time).ToList();
        //}

        private PingdomClient.Contracts.Summary GetSummaryPerformance_ResolutionDay(int checkId)
        {
            var utcNow = DateTime.UtcNow;
            var args = new PingdomClient.Contracts.PerformanceArgs
            {
                From = utcNow.Subtract(TimeSpan.FromDays(30)).ToUnixTimestamp(),
                To = utcNow.ToUnixTimestamp(),
                Resolution = PingdomClient.Contracts.Resolution.Day,
                Order = PingdomClient.Contracts.Order.Desc,
                includeuptime = true
            };
            var result = Pingdom.Client.Performance.GetSummaryPerformance(checkId, args).Result;
            if (result.HasErrors)
            {
                throw new PingdomClientException($"Cannot get Pingdom Summary Performance Resolution=Day: {result.ErrorMessage}");
            }
            return result.Summary.Days != null && result.Summary.Days.Any() ? result.Summary : null;
        }
        private PingdomClient.Contracts.Summary GetSummaryPerformance_ResolutionHour(int checkId)
        {
            var daysRange = 30;
            var utcNow = DateTime.UtcNow;
            var startOfTomorrowUtc = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0).AddDays(1);
            var maxOldDate = startOfTomorrowUtc.Subtract(TimeSpan.FromDays(daysRange + 1)).ToUnixTimestamp();

            //Get the latest crawled data
            IEnumerable<PingdomClient.Contracts.Uptime> rawPerfDataFromDB = null;
            lock (dbOperationThreadLock)
            {
                var crawledDataFromDB = _pingdom_CheckSummaryRepository
                    .FindWhere(x => x.CheckId == checkId && x.Resolution == (int)ResolutionType.Hour && x.DataTimeUtc >= maxOldDate)
                    .OrderByDescending(x => x.DataTimeUtc)
                    .FirstOrDefault();
                rawPerfDataFromDB = (crawledDataFromDB == null || crawledDataFromDB.Json == null) ? null : JsonConvert.DeserializeObject<IEnumerable<PingdomClient.Contracts.Uptime>>(crawledDataFromDB.Json);
            }

            var finalSummary = new List<PingdomClient.Contracts.Uptime>();
            for (int i = 0; i <= daysRange; i++)
            {
                //00:00:00
                var fromTime = startOfTomorrowUtc.Subtract(TimeSpan.FromDays(i + 1)).ToUnixTimestamp();
                //23:59:59
                var toTime = (i == 0) ? utcNow.ToUnixTimestamp() : startOfTomorrowUtc.Subtract(TimeSpan.FromDays(i)).AddSeconds(-1).ToUnixTimestamp();
                //_logger.Information($"fromTime:{fromTime.UnixTimeStampToDateTimeUtc()}, toTime:{toTime.UnixTimeStampToDateTimeUtc()}");

                //If data range is not today (i != 0, because in that case we saved incomplete data) and the data was crawled before, just take it instead of re-crawling
                bool foundCrawledData = false;
                int foundCrawledDataMustCount = 24;
                if (i != 0 && rawPerfDataFromDB != null)
                {
                    var foundData = rawPerfDataFromDB.Where(x => long.Parse(x.StartTime) >= fromTime && long.Parse(x.StartTime) <= toTime).ToList();
                    if (foundData != null && foundData.Any())
                    {
                        //To avoid the case that the crawler does not run often because in that case we saved incomplete data => it must have a sufficient number of records
                        if (foundData.Count() < foundCrawledDataMustCount)
                        {
                            _logger.Information($"Found crawled Pingdom Summary Performance data Resolution=Hour checkId={checkId}, daysAgo={i}; but only {foundData.Count()} records, < threshold {foundCrawledDataMustCount}");
                        }
                        else
                        {
                            finalSummary.AddRange(foundData);
                            foundCrawledData = true;
                            _logger.Information($"Found crawled Pingdom Summary Performance data ({foundData.Count()} records) Resolution=Hour checkId={checkId}, daysAgo={i}; so take it & skip crawling data!");
                            //foreach (var element in foundData)
                            //{
                            //    _logger.Debug(long.Parse(element.StartTime).UnixTimeStampToDateTimeUtc().ToString());
                            //}
                        }                        
                    }
                }
                if (!foundCrawledData)
                {
                    //Start to crawl
                    var args = new PingdomClient.Contracts.PerformanceArgs
                    {
                        From = fromTime,
                        To = toTime,
                        Resolution = PingdomClient.Contracts.Resolution.Hour,
                        Order = PingdomClient.Contracts.Order.Desc,
                        includeuptime = true
                    };
                    var result = Pingdom.Client.Performance.GetSummaryPerformance(checkId, args).Result;
                    if (result.HasErrors)
                    {
                        throw new PingdomClientException($"Could not get Pingdom Summary Performance data Resolution=Hour: {result.ErrorMessage} checkId={checkId}");
                    }
                    if (result.Summary != null && result.Summary.Hours != null) finalSummary.AddRange(result.Summary.Hours);
                    _logger.Information($"Crawled Pingdom Summary Performance data successfully, Resolution=Hour checkId={checkId} daysAgo={i}");

                    //Sleep to help CPU work better, and not to DDOS Pingdom System
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }

            return finalSummary.Any() ? new PingdomClient.Contracts.Summary() { Hours = finalSummary } : null;
        }

        private void SaveSummaryPerformanceToDB(long checkId, PingdomClient.Contracts.Summary checkSummary)
        {
            if (checkSummary == null)
            {
                _logger.Information("Nothing to save");
                return;
            }

            lock (dbOperationThreadLock)
            {
                if (checkSummary.Hours != null)
                {
                    var pingdomSummaryHour = new Data_Pingdom_Check_SummaryPerformance()
                    {
                        Id = Guid.NewGuid(),
                        CheckId = checkId,
                        CrawledTimestamp = DateTime.Now,
                        Resolution = (int)ResolutionType.Hour,
                        Json = JsonConvert.SerializeObject(checkSummary.Hours, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        DataTimeUtc = long.Parse(checkSummary.Hours.Max(x => x.StartTime))
                    };
                    _pingdom_CheckSummaryRepository.Add(pingdomSummaryHour);

                    var existedData = _pingdom_CheckSummaryRepository
                        .FindFirstOrDefault(x => x.CheckId == checkId && x.Resolution == pingdomSummaryHour.Resolution && x.DataTimeUtc == pingdomSummaryHour.DataTimeUtc);
                    if (existedData == null)
                    {
                        _pingdom_CheckSummaryRepository.Add(pingdomSummaryHour);
                    }
                    else
                    {
                        existedData.Json = pingdomSummaryHour.Json;
                        existedData.DataTimeUtc = pingdomSummaryHour.DataTimeUtc;
                        existedData.CrawledTimestamp = DateTime.Now;
                        _pingdom_CheckSummaryRepository.Edit(existedData);
                    }
                }

                if (checkSummary.Days != null)
                {
                    var pingdomSummaryDay = new Data_Pingdom_Check_SummaryPerformance()
                    {
                        Id = Guid.NewGuid(),
                        CheckId = checkId,
                        CrawledTimestamp = DateTime.Now,
                        Resolution = (int)ResolutionType.Day,
                        Json = JsonConvert.SerializeObject(checkSummary.Days, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        DataTimeUtc = long.Parse(checkSummary.Days.Max(x => x.StartTime))
                    };

                    var existedData = _pingdom_CheckSummaryRepository
                        .FindFirstOrDefault(x => x.CheckId == checkId && x.Resolution == pingdomSummaryDay.Resolution && x.DataTimeUtc == pingdomSummaryDay.DataTimeUtc);
                    if (existedData == null)
                    {
                        _pingdom_CheckSummaryRepository.Add(pingdomSummaryDay);
                    }
                    else
                    {
                        existedData.Json = pingdomSummaryDay.Json;
                        existedData.DataTimeUtc = pingdomSummaryDay.DataTimeUtc;
                        existedData.CrawledTimestamp = DateTime.Now;
                        _pingdom_CheckSummaryRepository.Edit(existedData);
                    }
                }

                if (checkSummary.Weeks != null)
                {
                    var pingdomSummaryWeek = new Data_Pingdom_Check_SummaryPerformance()
                    {
                        Id = Guid.NewGuid(),
                        CheckId = checkId,
                        CrawledTimestamp = DateTime.Now,
                        Resolution = (int)ResolutionType.Week,
                        Json = JsonConvert.SerializeObject(checkSummary.Weeks, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                        DataTimeUtc = long.Parse(checkSummary.Weeks.Max(x => x.StartTime))
                    };

                    var existedData = _pingdom_CheckSummaryRepository
                        .FindFirstOrDefault(x => x.CheckId == checkId && x.Resolution == pingdomSummaryWeek.Resolution && x.DataTimeUtc == pingdomSummaryWeek.DataTimeUtc);
                    if (existedData == null)
                    {
                        _pingdom_CheckSummaryRepository.Add(pingdomSummaryWeek);
                    }
                    else
                    {
                        existedData.Json = pingdomSummaryWeek.Json;
                        existedData.DataTimeUtc = pingdomSummaryWeek.DataTimeUtc;
                        existedData.CrawledTimestamp = DateTime.Now;
                        _pingdom_CheckSummaryRepository.Edit(existedData);
                    }
                }

                this.SafeExecute(() => _pingdom_CheckSummaryRepository.Save());
            }
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _pingdom_CheckSummaryRepository.BatchDelete(x => x.DataTimeUtc < retentionTimeCalc);            
        }
    }
}