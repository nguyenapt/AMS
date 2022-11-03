using AMS.ReportAutomation.Data.DataContext;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IDynomapper_ProcessorService
    {
        string OutputFolder { get; set; }
        void Process();
        void DeleteOldData(int retentionTime);
    }
}
