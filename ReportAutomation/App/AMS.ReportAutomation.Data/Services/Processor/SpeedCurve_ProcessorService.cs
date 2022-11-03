using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Newtonsoft.Json;
using PingdomClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AMS.ReportAutomation.Data.Services.Interfaces.Proccessor;

namespace AMS.ReportAutomation.Data.Services.Processor
{
    public class SpeedCurve_ProcessorService : ISpeedCurve_ProcessorService
    {
        public void DeleteOldData(int retentionTime)
        {
        }

        public void Process()
        {
        }
    }
}