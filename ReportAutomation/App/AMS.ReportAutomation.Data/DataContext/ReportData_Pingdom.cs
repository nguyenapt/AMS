//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMS.ReportAutomation.Data.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReportData_Pingdom
    {
        public System.Guid Id { get; set; }
        public Nullable<long> CheckId { get; set; }
        public Nullable<long> ReportTimeUtc { get; set; }
        public string Json { get; set; }
        public Nullable<System.DateTime> GeneratedTimestamp { get; set; }
    
        public virtual Data_Pingdom_Check Data_Pingdom_Check { get; set; }
    }
}