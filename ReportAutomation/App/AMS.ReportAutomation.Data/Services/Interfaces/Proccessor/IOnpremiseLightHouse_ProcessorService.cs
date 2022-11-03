using AMS.ReportAutomation.Data.DataContext;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface IOnpremiseLightHouse_ProcessorService
    {
        void Process();
        void DeleteOldData(int retentionTime);
    }
}
