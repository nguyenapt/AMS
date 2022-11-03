using AMS.ReportAutomation.ReportSubscription.AutofacConfig;
using Autofac;
using Autofac.Extras.Quartz;
using Serilog;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using Topshelf;
using Topshelf.Autofac;
using AMS.ReportAutomation.Common.Base;

namespace AMS.ReportAutomation.ReportSubscription
{
    public class Program
    {
        public static IContainer container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            container = BuildContainer();

            var rc = HostFactory.Run(x =>
            {
                x.UseAutofacContainer(container);

                x.Service<IReportSubscriptionService>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.StartAutomaticallyDelayed();

                x.SetDescription("Send email for subscription");
                x.SetDisplayName("AMS Report Subscription");
                x.SetServiceName("AMSReportSubscription");
            });

        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new ConventionModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new EFModule());            

            var schedulerConfig = new NameValueCollection
            {
                { "quartz.scheduler.instanceName", "ReportSubscriptionJobScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" }
            };

            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .WriteTo.RollingFile(Path.Combine(ConfigurationManager.AppSettings["serilogFileLocation"], @"log-{Date}.txt"))
                  .CreateLogger();
            }).SingleInstance();

            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = c => schedulerConfig
            });

            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(Program).Assembly));

            builder.RegisterType<SerilogAdapter>()
                .As<IAMSLogger>()
                .InstancePerLifetimeScope();

            builder.RegisterType<JobService>()
                .As<IJobService>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
