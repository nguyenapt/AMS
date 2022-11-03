using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.ReportSubscription
{
    public interface IReportSubscriptionService
    {
        void Start();
        void Stop();
    }
}
