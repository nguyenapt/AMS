using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.Common.Base
{
    public class JobService : IJobService
    {
        public IList<Type> GetAllJob<T>()
        {
            var type = typeof(T);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p)).ToList();

            return types;
        }

        public IJobDetail CreateJob(Type jobType, string jobId = "")
        {
            return JobBuilder
                .Create(jobType)
                .WithIdentity(jobType.FullName + jobId)
                .WithDescription(jobType.Name)
                .Build();
        }

        public ITrigger CreateTrigger(string name, string cronExpression)
        {
            return TriggerBuilder
                    .Create()
                    .WithIdentity($"{name}-Trigger")
                    .StartNow()
                    .WithCronSchedule(cronExpression)
                    .Build();
        }
    }
}
