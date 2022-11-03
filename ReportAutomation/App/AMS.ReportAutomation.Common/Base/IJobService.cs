using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Common.Base
{
    public interface IJobService
    {
        IList<Type> GetAllJob<T>();

        ITrigger CreateTrigger(string name,string cronExpression);

        IJobDetail CreateJob(Type jobType,string jobId = "");
    }
}
