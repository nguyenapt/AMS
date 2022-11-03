namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IPageSpeedInsights_ProcessorService
    {
        void Process();

        void DeleteOldData(int retentionTime);
    }
}
