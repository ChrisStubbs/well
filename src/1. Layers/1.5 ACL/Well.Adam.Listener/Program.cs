namespace PH.Well.Adam.Listener
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;
    using PH.Well.Services.EpodServices;

    using StructureMap;

    public class Program
    {
        public static void Main(string[] args)
        {
            var container = InitIoc();

            var eventLogger = container.GetInstance<IEventLogger>();

            eventLogger.TryWriteToEventLog(
                EventSource.WellTaskRunner,
                "Processing ADAM imports...",
                5422,
                EventLogEntryType.Information);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            var monitorService = container.GetInstance<IAdamFileMonitorService>();

            monitorService.Monitor(Configuration.RootFolder);
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        private static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IAccountRepository>().Use<AccountRepository>();
                    x.For<IAdamFileMonitorService>().Use<AdamFileMonitorService>();
                    x.For<IRouteMapper>().Use<RouteMapper>();
                    x.For<IFileService>().Use<FileService>();
                    x.For<IFileModule>().Use<FileModule>();
                    x.For<IFileTypeService>().Use<FileTypeService>();
                    x.For<IAdamImportService>().Use<AdamImportService>();
                    x.For<IAdamUpdateService>().Use<AdamUpdateService>();
                    x.For<IJobStatusService>().Use<JobStatusService>();
                    x.For<IUserNameProvider>().Use<AdamUserNameProvider>();
                    x.For<IBranchRepository>().Use<BranchRepository>();
                    x.For<IDapperProxy>().Use<WellDapperProxy>();
                    x.For<IPostImportRepository>().Use<PostImportRepository>();
                });
        }
    }
}
