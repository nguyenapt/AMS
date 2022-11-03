namespace AMS.JiraServicedesk.Contracts
{
    public class ServiceDeskQueue
    {
        public string id { get; set; }
        public string name { get; set; }
        public string jql { get; set; }
        public string[] fields { get; set; }
        public int issueCount { get; set; }
        //public object _links { get; set; }
    }
}