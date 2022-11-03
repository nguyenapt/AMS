using AMS.JiraServicedesk;
using AMS.JiraServicedesk.Contracts;
using AMS.ReportAutomation.Data.Common;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Crawler
{
    public class MyServiceDesk : ServiceDesk
    {
        public MyServiceDesk(ServicedeskConfiguration servicedeskConfig)
            : base(servicedeskConfig, null)
        { }
    }

    public class SampleJiraServiceDeskCrawler
    {
        private IAMSLogger _logger;

        public SampleJiraServiceDeskCrawler(IAMSLogger logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.Information("Start execute JiraServiceDesk crawler job");
            return Task.Run(() => {
                try
                {
                    //TODO: Get the list of Projects from report_clientJiraServiceDesks table
                    var listOfClientJiraServiceDesks = new List<dynamic>();
                    if (listOfClientJiraServiceDesks != null && listOfClientJiraServiceDesks.Any())
                    {
                        foreach (var sd in listOfClientJiraServiceDesks)
                        {
                            if (string.IsNullOrWhiteSpace(sd.ServiceDeskProjectKey))
                            {
                                _logger.Error("ProjectKey was not provided for {0}!", sd.Id);
                                continue;
                            }

                            var serviceDeskConfig = new ServicedeskConfiguration("jiraservicedesk1");
                            if (!string.IsNullOrWhiteSpace(sd.ServiceDeskUrl)) serviceDeskConfig.BaseUrl = sd.ServiceDeskUrl;
                            //TODO: Future - take the appropriate username and password from the config file for the new BaseUrl
                            serviceDeskConfig.ServiceDeskProjectKey = sd.ServiceDeskProjectKey;

                            var serviceDesk = new MyServiceDesk(null);

                            //Take the data of the last month
                            var monthsAgo = 1;
                            var jql = "project=" + serviceDeskConfig.ServiceDeskProjectKey + "%20and%20created%20>=%20startOfMonth(" + (0 - monthsAgo) + ")%20and%20created%20<=%20startOfMonth(" + (1 - monthsAgo) + ")&fields=issuetype,priority,status,assignee,created," + Issue.Const_CustomerRequestTypeField + "," + Issue.Const_OrganizationsField;
                            var jsonData = serviceDesk.GetIssuesByJQL(jql);

                            if (jsonData.total == 0)
                            {
                                _logger.Error("No data record found for {0} {1}!", serviceDeskConfig.BaseUrl, serviceDeskConfig.ServiceDeskProjectKey);
                                continue;
                            }
                            //TODO: Save crawled data into data_jiraServiceDesk table
                            var lastMonthDate = DateTime.UtcNow.AddMonths(0 - monthsAgo);
                            lastMonthDate = new DateTime(lastMonthDate.AddMonths(1).Year, lastMonthDate.AddMonths(1).Month, 1).AddDays(-1);
                            //var dataToInsert = new Object
                            //{
                            //    ServiceDeskUrl = serviceDeskConfig.BaseUrl,
                            //    ProjectKey = serviceDeskConfig.ServiceDeskProjectKey,
                            //    DatasetType = "IssuesCreatedInAMonth",
                            //    JQL = jql,
                            //    Json = JsonConvert.SerializeObject(jsonData.issues),
                            //    DataTimeUtc = lastMonthDate,
                            //    CrawledTimestamp = DateTime.Now
                            //};
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    if (ex.InnerException != null) _logger.Error(ex.InnerException.Message);
                    _logger.Error(ex.StackTrace);
                }               
            });
        }
    }
}
