namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IScreamingFrog_ProcessorService
    {
        string SpiderOutputFolder { get; set; }

        void Process();
        void DeleteOldData(int retentionTime);
    }
}
