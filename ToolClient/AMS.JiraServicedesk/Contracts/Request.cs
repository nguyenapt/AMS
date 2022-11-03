namespace AMS.JiraServicedesk.Contracts
{
    public class Request
    {
        //public object _expands { get; set; }
        public string issueId { get; set; }
        public string issueKey { get; set; }
        public int requestTypeId { get; set; }
        public int serviceDeskId { get; set; }
        //public object createdDate { get; set; }
        //public object reporter { get; set; }
        //public object requestFieldValues { get; set; }
        //public object currentStatus { get; set; }
        //public object _links { get; set; }
    }
}