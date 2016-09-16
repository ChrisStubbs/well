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
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            Scan(
                scan => {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

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
            For<IExceptionEventService>().Use<ExceptionEventService>();
            For<IBranchRepository>().Use<BranchRepository>();
            For<IUserRepository>().Use<UserRepository>();
            For<IBranchService>().Use<BranchService>();
            For<IActiveDirectoryService>().Use<ActiveDirectoryService>();
            For<IAdamRepository>().Use<AdamRepository>();
            For<IJobDetailRepository>().Use<JobDetailRepository>();
            For<IJobDetailDamageRepo>().Use<JobDetailDamageRepo>();
            For<IDeliveryService>().Use<DeliveryService>();
            For<IUserRoleProvider>().Use<UserRoleProvider>();
            For<IUserStatsRepository>().Use<UserStatsRepository>();
            For<IAuditRepository>().Use<AuditRepository>();
            For<ISeasonalDateRepository>().Use<SeasonalDateRepository>();

            // Mappers
            For<IRouteModelsMapper>().Use<RouteModelsMapper>();
            For<IBranchModelMapper>().Use<BranchModelMapper>();
            For<IDeliveryToDetailMapper>().Use<DeliveryToDetailMapper>();
        }
    }
}