using AMS.JiraServicedesk;
using AMS.JiraServicedesk.Contracts;
using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using AMS.ReportAutomation.ReportData.JiraServiceDesk;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class Jira_ServiceDesk_ProcessorService : IJira_ServiceDesk_ProcessorService
    {
        public const string Const_IssuesCreatedInAMonth = "IssuesCreatedInAMonth";
        public const string Const_IncidentReport = "IncidentReport";

        private readonly IAMSLogger _logger;

        private readonly IJira_ServiceDeskRepository _jira_ServiceDeskRepository;

        private readonly IReport_ClientJiraServiceDesksRepository _report_ClientJiraServiceDesksRepository;

        private readonly IReportData_JiraServiceDeskRepository _reportData_JiraServiceDeskRepository;

        public Jira_ServiceDesk_ProcessorService(IAMSLogger logger, IJira_ServiceDeskRepository jira_ServiceDeskRepository, IReport_ClientJiraServiceDesksRepository report_ClientJiraServiceDesksRepository, IReportData_JiraServiceDeskRepository reportData_JiraServiceDeskRepository)
            : base()
        {
            _logger = logger;
            _report_ClientJiraServiceDesksRepository = report_ClientJiraServiceDesksRepository;
            _reportData_JiraServiceDeskRepository = reportData_JiraServiceDeskRepository;
            _jira_ServiceDeskRepository = jira_ServiceDeskRepository;
        }        

        public void Process()
        {
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
                    //Make sure clientDesk.OrganizationName always has value, because it will be used as a dictionary key for grouping data
                    if (string.IsNullOrWhiteSpace(clientDesk.OrganizationName)) clientDesk.OrganizationName = clientDesk.OrgNameForProjectIfNotProvided;
                    if (string.IsNullOrWhiteSpace(clientDesk.OrganizationName)) throw new Exception("Service Desk OrganizationName value must be provided for grouping data");

                    //ServiceDeskUrl is default to be BaseUrl of jiraservicedesk1 if not specified
                    var configuration = new ServicedeskConfiguration(clientDesk.ServiceDeskUrl, clientDesk.ProjectKey, !string.IsNullOrWhiteSpace(clientDesk.ServiceDeskUrl) ? "jiraservicedesk" : "jiraservicedesk1");
                    var serviceDesk = new ServiceDesk(configuration);
                    var rawDataList = _jira_ServiceDeskRepository
                        .FindBy(x => x.ServiceDeskUrl == configuration.BaseUrl
                            && x.ProjectKey == configuration.ServiceDeskProjectKey
                            && x.DatasetType == Const_IssuesCreatedInAMonth);

                    if (rawDataList != null)
                    {
                        foreach (var rawData in rawDataList)
                        {
                            IncidentReport reportData = null;

                            dynamic jsonData = JsonConvert.DeserializeObject(rawData.Json);
                            var issues = ((JArray)jsonData.issues).ToObject<Issue[]>();
                            //Merge with the data of the previous month so it has more than 30 days of data
                            if (rawData.DataTimeUtc.HasValue)
                            {
                                var dataTimeUtc = rawData.DataTimeUtc.Value.UnixTimeStampToDateTimeUtc();
                                var firstDayOfTheDataMonth = new DateTime(dataTimeUtc.Year, dataTimeUtc.Month, 1, 0, 0, 0).ToUnixTimestamp();
                                var rawDataOfPreviousMonth = rawDataList.Where(x => x.DataTimeUtc != null && x.DataTimeUtc.Value <= firstDayOfTheDataMonth).OrderByDescending(x => x.DataTimeUtc).FirstOrDefault();
                                dynamic jsonDataOfPreviousMonth = rawDataOfPreviousMonth != null ? JsonConvert.DeserializeObject(rawDataOfPreviousMonth.Json) : null;
                                var issuesOfPreviousMonth = jsonDataOfPreviousMonth != null ? ((JArray)jsonDataOfPreviousMonth.issues).ToObject<Issue[]>() : null;
                                if (issuesOfPreviousMonth != null)
                                {
                                    var list = new List<Issue>();
                                    if (issues != null) list.AddRange(issues);
                                    list.AddRange(issuesOfPreviousMonth);
                                    issues = list.ToArray();                                    
                                }
                            }

                            var issuesByOrganization = GroupIssuesByOrganization(issues, clientDesk.OrgNameForProjectIfNotProvided);
                            Issue[] issuesToInsertIntoDB = issuesByOrganization == null || !issuesByOrganization.ContainsKey(clientDesk.OrganizationName) ?
                                null : issuesByOrganization[clientDesk.OrganizationName];
                            if (issuesToInsertIntoDB != null)
                            {
                                reportData = new IncidentReport();
                                reportData.ServiceDeskBaseUrl = configuration.BaseUrl;
                                reportData.Tickets = issuesToInsertIntoDB.Where(x => x.fields.IssueType.name.ToLower() == "incident").Select(x => new Ticket()
                                {
                                    Key = x.key,
                                    Summary = x.fields.summary,
                                    Priority = x.fields.Priority != null ? x.fields.Priority.name : null,
                                    CustomerRequestType = (x.fields.CustomerRequestType != null && x.fields.CustomerRequestType.RequestType != null) ? x.fields.CustomerRequestType.RequestType.name : null,
                                    Status = x.fields.Status != null ? x.fields.Status.name : null,
                                    Reporter = x.fields.Reporter != null ? x.fields.Reporter.name.Replace("alert@pingdom.com", "247-Monitoring") : null,
                                    CreatedOnUtc = x.fields.CreatedOn != null ? x.fields.CreatedOn.Value.ToUniversalTime() : (DateTime?)null
                                }).OrderByDescending(x => x.CreatedOnUtc).ToList();
                            }

                            //Save processed data into DB
                            var dataToInsert = new ReportData_JiraServiceDesk
                            {
                                Id = Guid.NewGuid(),
                                ClientServiceDeskId = clientDesk.Id,
                                DatasetType = Const_IncidentReport,
                                ReportTimeUtc = rawData.DataTimeUtc,
                                Json = reportData == null ? null : JsonConvert.SerializeObject(reportData),
                                GeneratedTimestamp = DateTime.Now
                            };
                            var existedData = _reportData_JiraServiceDeskRepository
                                .FindFirstOrDefault(x => x.ClientServiceDeskId == dataToInsert.ClientServiceDeskId && x.ReportTimeUtc == dataToInsert.ReportTimeUtc && x.DatasetType == dataToInsert.DatasetType);
                            if (existedData != null)
                            {
                                existedData.Json = dataToInsert.Json;
                                existedData.GeneratedTimestamp = dataToInsert.GeneratedTimestamp;
                                _reportData_JiraServiceDeskRepository.Edit(existedData);
                            }
                            else
                            {
                                _reportData_JiraServiceDeskRepository.Add(dataToInsert);
                            }
                        }
                    }
                }
                _reportData_JiraServiceDeskRepository.Save();
            }
        }

        private Dictionary<string, Issue[]> GroupIssuesByOrganization(Issue[] issues, string defaultNoneOrgNameIfNotProvided)
        {
            Dictionary<string, Issue[]> issuesByOrg = null;
            if (issues != null && issues.Length > 0)
            {
                issues = AssignDefaultOrgNameWhereNotProvided(issues, defaultNoneOrgNameIfNotProvided);
                issuesByOrg = new Dictionary<string, Issue[]>();
                var organizations = issues.Select(x => RemoveAlertSystemOrgName(x.fields.OrganizationNames)).Distinct().ToList();
                if (organizations != null)
                {
                    bool checkpointExistedNoneOrg = false;
                    foreach (var org in organizations)
                    {
                        if (!string.IsNullOrWhiteSpace(org))
                        {
                            //Issues which have Org info
                            issuesByOrg.Add(org, issues.Where(x => !string.IsNullOrEmpty(x.fields.OrganizationNames) && RemoveAlertSystemOrgName(x.fields.OrganizationNames) == org).ToArray());
                        }
                        else
                        {
                            checkpointExistedNoneOrg = true;
                        }
                    }
                    //Issues which do not have Org info
                    if (checkpointExistedNoneOrg) throw new Exception("Need to correct code to make sure Organization default value is assigned to where it is not provided");
                }
            }

            return issuesByOrg;
        }
        private Issue[] AssignDefaultOrgNameWhereNotProvided(Issue[] issues, string defaultNoneOrgNameIfNotProvided)
        {
            if (issues == null) return null;

            //Make sure Organization default value is assigned to where it is not provided
            for (var issueIdx = 0; issueIdx < issues.Length; issueIdx++)
            {
                if (string.IsNullOrWhiteSpace(issues[issueIdx].fields.OrganizationNames))
                    issues[issueIdx].fields.Organizations = new[] { new Organization() { name = !string.IsNullOrWhiteSpace(defaultNoneOrgNameIfNotProvided) ? defaultNoneOrgNameIfNotProvided : "N/A" } };
            }

            return issues;
        }
        private string RemoveAlertSystemOrgName(string organizations)
        {
            if (string.IsNullOrWhiteSpace(organizations)) return organizations;
            //TODO: Move the hard-coded string "Alert System," to config file?
            return organizations.Replace("Alert System,", string.Empty).Replace(",Alert System", string.Empty);
        }

        public IncidentReport GetIncidentDataByClientJiraServiceDeskId(Guid clientJiraServiceDeskId, long reportToDateTime)
        {
            var reportData = _reportData_JiraServiceDeskRepository
                .FindWhere(x => x.ClientServiceDeskId == clientJiraServiceDeskId && x.DatasetType == Const_IncidentReport && x.ReportTimeUtc <= reportToDateTime)
                .OrderByDescending(x => x.ReportTimeUtc)
                .ThenBy(x => x.GeneratedTimestamp)
                .FirstOrDefault();
            if (reportData != null && reportData.Json != null)
            {
                var retData = JsonConvert.DeserializeObject<IncidentReport>(reportData.Json);
                if (retData != null)
                {
                    if (reportData.ReportTimeUtc != null) retData.ConsolidatedTimeUtc = reportData.ReportTimeUtc.Value.UnixTimeStampToDateTimeUtc();
                }
                return retData;
            }

            return null;
        }

        public void DeleteOldData(int retentionTime)
        {
            var retentionTimeCalc = DateTime.UtcNow.AddHours(-retentionTime).ToUnixTimestamp();
            _reportData_JiraServiceDeskRepository.BatchDelete(x => x.ReportTimeUtc < retentionTimeCalc);
        }
    }
}
