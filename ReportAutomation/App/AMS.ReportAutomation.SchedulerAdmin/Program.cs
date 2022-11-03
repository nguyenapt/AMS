using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.SchedulerAdmin.AutofacConfig;
using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMS.ReportAutomation.SchedulerAdmin
{
    static class Program
    {
        public static IContainer Container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Container = Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Container.Resolve<AdminForm>());
        }

        static Autofac.IContainer Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());

            builder.RegisterType<AdminForm>();
            builder.RegisterType<SchedulerForm>();
            builder.RegisterType<ClientsForm>();
            builder.RegisterType<AddClientSiteForm>();
            builder.RegisterType<EditClientForm>();
            builder.RegisterType<ServiceDeskForm>();
            builder.RegisterType<EditServiceDeskForm>();
            builder.RegisterType<PingdomCheckListForm>();
            builder.RegisterType<ReportSubscriptionForm>();
            builder.RegisterType<EditReportSubscriptionForm>();

            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .WriteTo.RollingFile(Path.Combine(ConfigurationManager.AppSettings["serilogFileLocation"], @"log-admin-{Date}.txt"))
                  .CreateLogger();
            }).SingleInstance();

            builder.RegisterType<SerilogAdapter>()
                .As<IAMSLogger>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
