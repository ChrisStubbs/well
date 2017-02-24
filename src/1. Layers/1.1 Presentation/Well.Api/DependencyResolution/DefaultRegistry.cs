namespace PH.Well.Api.DependencyResolution
{
    using Mapper;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Common.Security;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;
    using Repositories.Read;
    using Services.EpodServices;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<IUserNameProvider>().Use<UserNameProvider>();
            For<IWellDbConfiguration>().Use<WellDbConfiguration>();
            For<IWellDapperProxy>().Use<WellDapperProxy>();
            For<IDapperReadProxy>().Use<DapperReadProxy>();
            For<IDapperProxy>().Use<WellDapperProxy>();
            For<IDbConfiguration>().Use<WellDbConfiguration>();
            For<ILogger>().Use<NLogger>();
            For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
            For<IServerErrorResponseHandler>().Use<ServerErrorResponseHandler>();
            For<IEventLogger>().Use<EventLogger>();
            For<IStopRepository>().Use<StopRepository>();
            For<IAccountRepository>().Use<AccountRepository>();
            For<IJobRepository>().Use<JobRepository>();
            For<IDeliveryReadRepository>().Use<DeliveryReadRepository>();
            For<IExceptionEventRepository>().Use<ExceptionEventRepository>();
            For<IDeliveryLineActionService>().Use<DeliveryLineActionService>();
            For<IJobStatusService>().Use<JobStatusService>();

            For<IBranchRepository>().Use<BranchRepository>();
            For<IUserRepository>().Use<UserRepository>();
            For<IBranchService>().Use<BranchService>();
            For<IActiveDirectoryService>().Use<ActiveDirectoryService>();
            For<IAdamRepository>().Use<AdamRepository>();
            For<IJobDetailRepository>().Use<JobDetailRepository>();
            For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
            For<IDeliveryService>().Use<DeliveryService>();
            For<INotificationRepository>().Use<NotificationRepository>();
            For<IUserRoleProvider>().Use<UserRoleProvider>();
            For<IUserStatsRepository>().Use<UserStatsRepository>();
            For<IAuditRepository>().Use<AuditRepository>();
            For<ISeasonalDateRepository>().Use<SeasonalDateRepository>();
            For<ICreditThresholdRepository>().Use<CreditThresholdRepository>();
            For<ICleanPreferenceRepository>().Use<CleanPreferenceRepository>();
            For<IJobDetailActionRepository>().Use<JobDetailActionRepository>();
            For<IWidgetRepository>().Use<WidgetRepository>();
            For<IUserThresholdService>().Use<UserThresholdService>();
            For<IEpodUpdateService>().Use<EpodUpdateService>();
            For<ICreditTransactionFactory>().Use<CreditTransactionFactory>();
            For<IPodTransactionFactory>().Use<PodTransactionFactory>();

            // Mappers
            For<IBranchModelMapper>().Use<BranchModelMapper>();
            For<IDeliveryToDetailMapper>().Use<DeliveryToDetailMapper>();
            For<ISeasonalDateMapper>().Use<SeasonalDateMapper>();
            For<ICreditThresholdMapper>().Use<CreditThresholdMapper>();
            For<ICleanPreferenceMapper>().Use<CleanPreferenceMapper>();
            For<IWidgetWarningMapper>().Use<WidgetWarningMapper>();
            For<IDeliveryLineToJobDetailMapper>().Use<DeliveryLineToJobDetailMapper>();
            For<IDeliverLineToDeliveryLineCreditMapper>().Use<DeliverLineToDeliveryLineCreditMapper>();

            //delivery lines
            For<IDeliveryLinesAction>().Use<DeliveryLinesCredit>();
            For<IDeliveryLinesAction>().Use<DeliveryLinesClose>();
        }
    }
}