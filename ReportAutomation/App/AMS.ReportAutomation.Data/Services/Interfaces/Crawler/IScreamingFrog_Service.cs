namespace AMS.ReportAutomation.Data.Services.Interfaces.Crawler
{
    public interface IScreamingFrog_Service
    {
        string SpiderProgramFolder { get; set; }
        string SpiderConfigFile { get; set; }
        string SpiderOutputFolder { get; set; }

        void Crawl();

        void DeleteOldData(int retentionTime);
    }
}
