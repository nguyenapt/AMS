using AMS.ReportAutomation.Common.Base;
using AMS.ReportAutomation.ReportDataProcessor.AutofacConfig;
using Autofac;
using Autofac.Extras.Quartz;
using Serilog;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using Topshelf;
using Topshelf.Autofac;

namespace AMS.ReportAutomation.ReportDataProcessor
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

                x.Service<IProcessorService>(s =>
                {
                    s.ConstructUsingAutofacContainer();
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.StartAutomaticallyDelayed();

                x.SetDescription("Prepare ready-to-report data");
                x.SetDisplayName("AMS Processor Service");
                x.SetServiceName("AMSProcessorService");
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
                { "quartz.scheduler.instanceName", "ProcessorJobScheduler" },
                { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                { "quartz.threadPool.threadCount", "3" }
            };

            builder.RegisterModule(new QuartzAutofacFactoryModule
            {
                ConfigurationProvider = c => schedulerConfig
            });

            builder.RegisterModule(new QuartzAutofacJobsModule(typeof(Program).Assembly));

            builder.Register<ILogger>((c, p) =>
            {
                return new LoggerConfiguration()
                  .WriteTo.RollingFile(Path.Combine(ConfigurationManager.AppSettings["serilogFileLocation"], @"log-{Date}.txt"))
                  .CreateLogger();
            }).SingleInstance();

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
