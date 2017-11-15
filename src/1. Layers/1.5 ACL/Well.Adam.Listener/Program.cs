using System;
using System.IO;
using System.Linq;
using PH.Well.Domain.Enums;

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
    using Repositories.Read;
    using Shared.Well.Data.EF.Contracts;
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

            var monitorService = container.GetInstance<IFileMonitorService>();
            var config = new AdamFileMonitorServiceConfig(Configuration.RootFolder,Configuration.ArchiveFolder, Configuration.BranchesToProcess);

            monitorService.Monitor(config);
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IWellEntitiesConnectionString>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IAccountRepository>().Use<AccountRepository>();
                    x.For<IFileMonitorService>().Use<FileMonitorService>();
                    x.For<IOrderImportMapper>().Use<OrderImportMapper>();
                    x.For<IFileService>().Use<FileService>();
                    x.For<IFileModule>().Use<FileModule>();
                    x.For<IFileTypeService>().Use<FileTypeService>();
                    x.For<IAdamImportService>().Use<AdamImportService>();
                    x.For<IImportService>().Use<ImportService>();
                    x.For<IAdamUpdateService>().Use<AdamUpdateService>();
                    x.For<IJobService>().Use<JobService>();
                    x.For<IUserNameProvider>().Use<AdamUserNameProvider>();
                    x.For<PH.Common.Security.Interfaces.IUserNameProvider>().Use<AdamUserNameProvider>();
                    x.For<IBranchRepository>().Use<BranchRepository>();
                    x.For<IDapperProxy>().Use<WellDapperProxy>();
                    x.For<IPostImportRepository>().Use<PostImportRepository>();
                    x.For<IUserThresholdService>().Use<UserThresholdService>();
                    x.For<ICreditThresholdRepository>().Use<CreditThresholdRepository>();
                    x.For<IUserRepository>().Use<UserRepository>();
                    x.For<IDateThresholdService>().Use<DateThresholdService>();
                    x.For<IAssigneeReadRepository>().Use<AssigneeReadRepository>();
                    x.For<IDapperReadProxy>().Use<DapperReadProxy>();
                    x.For<IDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<ILineItemSearchReadRepository>().Use<LineItemSearchReadRepository>();
                    x.For<ISeasonalDateRepository>().Use<SeasonalDateRepository>();
                    x.For<IDateThresholdRepository>().Use<DateThresholdRepository>();
                    x.For<ICustomerRoyaltyExceptionRepository>().Use<CustomerRoyaltyExceptionRepository>();
                    x.For<IAdamImportMapper>().Use<AdamImportMapper>();
                    x.For<IAdamFileImportCommands>().Use<AdamFileImportCommands>();
                    x.For<IStopService>().Use<StopService>();
                    x.For<IRouteService>().Use<RouteService>();
                    x.For<IActivityService>().Use<ActivityService>();
                    x.For<ILocationService>().Use<LocationService>();
                    x.For<IWellStatusAggregator>().Use<WellStatusAggregator>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IStopService>().Use<StopService>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IActivityRepository>().Use<ActivityRepository>();
                    x.For<IDeadlockRetryConfig>().Use<Configuration>();
                    x.For<IDeadlockRetryHelper>().Use<DeadlockRetryHelper>();
                    x.For<IEpodFileProvider>().Use<EpodFileProvider>();
                    x.For<IEpodImportService>().Use<EpodImportService>();
                    x.For<IEpodImportMapper>().Use<EpodImportMapper>();
                    x.For<IEpodFileImportCommands>().Use<EpodFileImportCommands>();
                    x.For<IExceptionEventRepository>().Use<ExceptionEventRepository>();
                    x.For<IPodService>().Use<PodService>();
                    x.For<ILineItemActionRepository>().Use<LineItemActionRepository>();
                    x.For<INotificationRepository>().Use<NotificationRepository>();
                    x.For<IWellCleanConfig>().Use<Configuration>();
                    x.For<IWellCleanUpService>().Use<WellCleanUpService>();
                    x.For<IWellCleanUpRepository>().Use<WellCleanUpRepository>();
                    x.For<IAmendmentService>().Use<AmendmentService>();
                    x.For<IAmendmentFactory>().Use<AmendmentFactory>();
                    x.For<IAmendmentRepository>().Use<AmendmentRepository>();
                    x.For<IImportedFileRepository>().Use<ImportedFileRepository>();
                });
        }
    }
}
