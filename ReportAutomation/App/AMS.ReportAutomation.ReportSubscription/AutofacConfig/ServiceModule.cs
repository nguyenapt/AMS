using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.Data.Services;
using AMS.ReportAutomation.Data.Services.Interfaces;
using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AMS.ReportAutomation.ReportSubscription.AutofacConfig
{
    public class ServiceModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("AMS.ReportAutomation.Data"))
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();
        }
    }
}
