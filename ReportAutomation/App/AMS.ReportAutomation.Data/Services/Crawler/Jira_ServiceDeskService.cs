using AMS.JiraServicedesk;
using AMS.JiraServicedesk.Contracts;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMS.ReportAutomation.Data.Services.Interfaces.Crawler;
using AMS.ReportAutomation.Data.Services.Processor;

namespace AMS.ReportAutomation.Data.Services.Crawler
{
    public class Jira_ServiceDeskService : EntityService<Data_JiraServiceDesk>, IJira_ServiceDeskService
    {
        private readonly IReport_ClientJiraServiceDesksRepository _report_ClientJiraServiceDesksRepository;
        private readonly IJira_ServiceDeskRepository _jira_ServiceDeskRepository;

        public Jira_ServiceDeskService(IReport_ClientJiraServiceDesksRepository report_ClientJiraServiceDesksRepository, IJira_ServiceDeskRepository jira_ServiceDeskRepository, IAMSLogger logger)
            : base(jira_ServiceDeskRepository, logger)
        {
            _report_ClientJiraServiceDesksRepository = report_ClientJiraServiceDesksRepository;
            _jira_ServiceDeskRepository = jira_ServiceDeskRepository;
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _jira_ServiceDeskRepository.BatchDelete(x => x.DataTimeUtc < retentionTimeCalc);
        }

        public void GetIssuesByJQLAndSaveToDB()
        {
            var crawledJQLs = new List<string>();

            //Get the list of configured ServiceDesk Projects
            var clientJiraServiceDesks = _report_ClientJiraServiceDesksRepository.GetAll();
            if (clientJiraServiceDesks != null && clientJiraServiceDesks.Any())
            {
                foreach (var clientDesk in clientJiraServiceDesks)
                {
                    if (string.IsNullOrWhiteSpace(clientDesk.ProjectKey))
                    {
                        _logger.Error("ProjectKey was not provided for {0}!", clientDesk.Id);
                        continue;
                    }

                    //ServiceDeskUrl is default to be BaseUrl of jiraservicedesk1 if not specified
                    var configuration = new ServicedeskConfiguration(clientDesk.ServiceDeskUrl, clientDesk.ProjectKey, !string.IsNullOrWhiteSpace(clientDesk.ServiceDeskUrl) ? "jiraservicedesk" : "jiraservicedesk1");
                    var serviceDesk = new ServiceDesk(configuration);

                    //Take the data of the last 12 full-months
                    for (int monthsAgo = 0; monthsAgo <= 12; monthsAgo++)
                    {
                        //Luckily, the query here has the date data in Utc time already :)
                        var jql = "project=" + configuration.ServiceDeskProjectKey;
                        if (monthsAgo == 0)
                        {
                            //Current month to this moment
                            //jql += "%20and%20created%20>=%20startOfMonth()";
                            //Current month to the end of yesterday
                            jql += "%20and%20created%20>=%20startOfMonth()%20and%20created%20<=%20endOfDay(-1)";                            
                        }
                        else
                        {
                            //Previous month(s)
                            jql += "%20and%20created%20>=%20startOfMonth(" + (0 - monthsAgo) + ")%20and%20created%20<=%20startOfMonth(" + (1 - monthsAgo) + ")";
                        }
                        jql += "&fields=issuetype,priority,summary,status,reporter,assignee,created," + Issue.Const_CustomerRequestTypeField + "," + Issue.Const_OrganizationsField;

                        if (crawledJQLs.Contains(jql))
                        {
                            _logger.Information("In this crawling batch, data record(s) were already crawled for {0} {1}; monthsAgo={2}!", configuration.BaseUrl, configuration.ServiceDeskProjectKey, monthsAgo);
                            continue;
                        }
                        var jsonData = serviceDesk.GetIssuesByJQL(jql);

                        if (jsonData.total == 0)
                        {
                            _logger.Information("No data record found for {0} {1}; monthsAgo={2}!", configuration.BaseUrl, configuration.ServiceDeskProjectKey, monthsAgo);
                            break;
                        }
                        _logger.Information("{3} data record(s) found for {0} {1}; monthsAgo={2}!", configuration.BaseUrl, configuration.ServiceDeskProjectKey, monthsAgo, jsonData.total);
                        crawledJQLs.Add(jql);

                        //
                        //Save crawled data into data_jiraServiceDesk table
                        //
                        // End of the previous month
                        var lastMonthDateTime = DateTime.UtcNow.AddMonths(0 - monthsAgo);
                        lastMonthDateTime = new DateTime(lastMonthDateTime.AddMonths(1).Year, lastMonthDateTime.AddMonths(1).Month, 1, 23, 59, 59).AddDays(-1);
                        if (monthsAgo == 0)
                        {
                            // End of yesterday
                            lastMonthDateTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 23, 59, 59).AddDays(-1);
                        }

                        var data_JiraServiceDesk = new Data_JiraServiceDesk()
                        {
                            Id = Guid.NewGuid(),
                            ServiceDeskUrl = configuration.BaseUrl,
                            ProjectKey = configuration.ServiceDeskProjectKey,
                            DatasetType = Jira_ServiceDesk_ProcessorService.Const_IssuesCreatedInAMonth,
                            JQL = jql,
                            Json = JsonConvert.SerializeObject(jsonData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }),
                            DataTimeUtc = lastMonthDateTime.ToUnixTimestamp(),
                            CrawledTimestamp = DateTime.Now
                        };

                        //TODO: Minor question for Future - If we should crawl existed data? May optimize it (but then what if old data was changed?)
                        var existedData = _jira_ServiceDeskRepository.FindFirstOrDefault(x => x.ServiceDeskUrl == data_JiraServiceDesk.ServiceDeskUrl
                            && x.ProjectKey == data_JiraServiceDesk.ProjectKey
                            && x.DatasetType == data_JiraServiceDesk.DatasetType
                            && x.DataTimeUtc.Value == data_JiraServiceDesk.DataTimeUtc);
                        if (existedData == null)
                        {
                            _jira_ServiceDeskRepository.Add(data_JiraServiceDesk);
                        }
                        else
                        {
                            existedData.Json = JsonConvert.SerializeObject(jsonData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                            existedData.CrawledTimestamp = DateTime.UtcNow;
                            _jira_ServiceDeskRepository.Edit(existedData);
                        }

                        //Sleep to help CPU work better, and not to DDOS Jira System
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                    }                    
                }
                _jira_ServiceDeskRepository.Save();
            }
        }
    }
}
