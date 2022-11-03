using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Common.Base
{
    public enum ResolutionType
    {
        Hour = 0,
        Day = 1,
        Week = 2
    }

    public enum SchedulerType
    {
        CrawData = 0,
        ProcessorData = 1
    }
}
