using System;
using System.Collections.Generic;

namespace AMS.ReportAutomation.ReportData.JiraServiceDesk
{
    public class IncidentReport : ReportDataBase
    {
        public string ServiceDeskBaseUrl { get; set; }
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
        //i.e. Remarks taken from Recommendation Bank
        public List<KeyValuePair<string, string>> Remarks { get; set; } = new List<KeyValuePair<string, string>>();
    }

    public class Ticket
    {
        public string Key { get; set; }
        public string Summary { get; set; }
        public string Priority { get; set; }
        public string CustomerRequestType { get; set; }
        public string Status { get; set; }
        public string Reporter { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
    }
}