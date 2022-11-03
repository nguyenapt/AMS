using AMS.ReportAutomation.ReportData.JiraServiceDesk;
using System;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IJira_ServiceDesk_ProcessorService
    {
        void Process();
        IncidentReport GetIncidentDataByClientJiraServiceDeskId(Guid clientJiraServiceDeskId, long reportToDateTime);

        void DeleteOldData(int retentionTime);
    }
}
