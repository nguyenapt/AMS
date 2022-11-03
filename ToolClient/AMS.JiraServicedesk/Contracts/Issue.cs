using Newtonsoft.Json;
using System;
using System.Text;

namespace AMS.JiraServicedesk.Contracts
{
    public class Issue
    {
        //TODO: Does customfield_10001 work for Niteco247 JIRA ServiceDesk only? Need to do something smarter
        public const string Const_CustomerRequestTypeField = "customfield_10001";
        //TODO: Does customfield_10002 work for Niteco247 JIRA ServiceDesk only? Need to do something smarter
        public const string Const_OrganizationsField = "customfield_10002";

        //TODO: Custom fields work for Niteco247 JIRA ServiceDesk only? Need to do something smarter
        public const string Const_ComponentField = "customfield_10205";
        public const string Const_MarketField = "customfield_10206";
        public const string Const_WebsiteTierField = "customfield_10210";

        public string id { get; set; }
        public string self { get; set; }
        public string key { get; set; }
        public IssueFields fields { get; set; }
    }
    public class IssueFields
    {
        public string summary { get; set; }
        public IssueType IssueType { get; set; }
        public string duedate { get; set; }
        public string created { get; set; }
        public DateTime? CreatedOn {
            get {
                try
                {
                    return System.DateTime.Parse(created);
                }
                catch(Exception)
                {
                    return null;
                }
            }
        }
        public string updated { get; set; }
        public DateTime? UpdatedOn
        {
            get
            {
                try
                {
                    return DateTime.Parse(updated);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public IssuePriority Priority { get; set; }
        public JiraAccount Reporter { get; set; }
        public JiraAccount Assignee { get; set; }
        public IssueStatus Status { get; set; }

        [JsonProperty(Issue.Const_OrganizationsField)]
        public Organization[] Organizations { get; set; }
        public string OrganizationNames
        {
            get
            {
                if (Organizations == null) return null;
                if (Organizations.Length == 0) return null;

                var sbOrgNames = new StringBuilder();
                for(int i=0; i<Organizations.Length; i++)
                {
                    if (i == 0) sbOrgNames.Append(Organizations[i].name);
                    else sbOrgNames.AppendFormat(",{0}", Organizations[i].name);
                }
                return sbOrgNames.ToString();
            }
        }

        [JsonProperty(Issue.Const_CustomerRequestTypeField)]
        public CustomerRequestTypeField CustomerRequestType { get;set;}


        [JsonProperty(Issue.Const_ComponentField)]
        public CustomField Component { get; set; }
        
        [JsonProperty(Issue.Const_MarketField)]
        public CustomField Market { get; set; }

        [JsonProperty(Issue.Const_WebsiteTierField)]
        public CustomField WebsiteTier { get; set; }

        public int? timespent { get; set; }
    }
    public class IssueType
    {
        public string self { get; set; }
        //public string id { get; set; }
        //public string description { get; set; }
        //public string iconUrl { get; set; }
        public string name { get; set; }
        //public bool subtask { get; set; }
        //public int avatarId { get; set; }
    }
    public class IssuePriority
    {
        public string self { get; set; }
        //public string iconUrl { get; set; }
        public string name { get; set; }
        //public int id { get; set; }
    }
    public class IssueStatus
    {
        public string self { get; set; }
        public string description { get; set; }
        //public string iconUrl { get; set; }
        public string name { get; set; }
        //public int id { get; set; }
        //public object statusCategory { get; set; }
    }
    public class JiraAccount
    {
        public string self { get; set; }
        public string name { get; set; }
        //public string key { get; set; }
        public string emailAddress { get; set; }
        //public object avatarUrls { get; set; }
        public string displayName { get; set; }
        public bool active { get; set; }
        //public bool timeZone { get; set; }
    }
}