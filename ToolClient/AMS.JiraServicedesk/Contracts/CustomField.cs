namespace AMS.JiraServicedesk.Contracts
{
    public class Organization
    {
        public string id { get; set; }
        public string name { get; set; }
        //public object _links { get; set; }
    }

    public class CustomField
    {
        public string id { get; set; }
        public string value { get; set; }
        //public object _links { get; set; }
    }

    public class CustomField2
    {
        public string id { get; set; }
        public string name { get; set; }
        //public object _links { get; set; }
    }

    public class CustomerRequestTypeField
    {
        public CustomField2 RequestType { get; set; }
    }
}