using AMS.ReportAutomation.Data.DataContext;
using System.Collections.Generic;

namespace AMS.ReportAutomation.Data.Services.Interfaces.Proccessor
{
    public interface ISpeedCurve_ProcessorService
    {
        void Process();
        void DeleteOldData(int retentionTime);
    }
}
