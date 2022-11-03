using AMS.ReportAutomation.Data.DataContext;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Crawler
{
    public interface IJira_ServiceDeskService : IEntityService<Data_JiraServiceDesk>
    {
        void GetIssuesByJQLAndSaveToDB();

        void DeleteOldData(int retentionTime);
    }
}
