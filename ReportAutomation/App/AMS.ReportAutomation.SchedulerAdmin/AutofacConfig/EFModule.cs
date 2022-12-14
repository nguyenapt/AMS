using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.DataContext;
using AMS.ReportAutomation.Data.Repository;
using AMS.ReportAutomation.Data.Repository.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.SchedulerAdmin.AutofacConfig
{
    public class EFModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType(typeof(UnitOfWork<Entities>)).As(typeof(IUnitOfWork<Entities>)).InstancePerRequest();

            builder.RegisterType<Entities>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
