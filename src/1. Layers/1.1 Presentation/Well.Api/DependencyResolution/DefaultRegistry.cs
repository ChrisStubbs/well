using PH.Shared.Well.Data.EF;

namespace PH.Well.Api.DependencyResolution
{
    using Mapper;

    using Mapper.Contracts;
    using Common;
    using Common.Contracts;
    using Common.Security;
    using Repositories;
    using Repositories.Contracts;
    using Services;
    using Services.Contracts;
    using Repositories.Read;
    using Services.DeliveryActions;
    using Services.EpodServices;
    using Services.Mappers;
    using Services.Validation;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using Shared.Well.Data.EF.Contracts;
    using Infrastructure;

    public class DefaultRegistry : Registry
    {
        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<IUserNameProvider>().Use<UserNameProvider>();
            For<PH.Common.Security.Interfaces.IUserNameProvider>().Use<UserNameProvider>();
            For<IWellDbConfiguration>().Use<WelllApiDbConfiguration>();
            For<IWellDapperProxy>().Use<WellDapperProxy>();
            For<IDapperReadProxy>().Use<DapperReadProxy>();
            For<IDapperProxy>().Use<WellDapperProxy>();
            For<IDbConfiguration>().Use<WelllApiDbConfiguration>();
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
            For<IJobService>().Use<JobService>();
            For<IStopService>().Use<StopService>();
            For<IRouteService>().Use<RouteService>();
            For<IActivityService>().Use<ActivityService>();
            For<ILocationService>().Use<LocationService>();
            For<IBranchRepository>().Use<BranchRepository>();
            For<IUserRepository>().Use<UserRepository>();
            For<IBranchService>().Use<BranchService>();
            For<IActiveDirectoryService>().Use<ActiveDirectoryService>();
            For<IAdamRepository>().Use<AdamRepository>();
            For<IJobDetailRepository>().Use<JobDetailRepository>();
            For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
            For<INotificationRepository>().Use<NotificationRepository>();
            For<IUserRoleProvider>().Use<UserRoleProvider>();
            For<IUserStatsRepository>().Use<UserStatsRepository>();
            For<ISeasonalDateRepository>().Use<SeasonalDateRepository>();
            For<ICreditThresholdRepository>().Use<CreditThresholdRepository>();
            For<IWidgetRepository>().Use<WidgetRepository>();
            For<IUserThresholdService>().Use<UserThresholdService>();
            For<ICreditTransactionFactory>().Use<CreditTransactionFactory>();
            For<IPodTransactionFactory>().Use<PodTransactionFactory>();
            For<IGlobalUpliftTransactionFactory>().Use<GlobalUpliftTransactionFactory>();
            For<IWellCleanUpService>().Use<WellCleanUpService>();
            For<IWellCleanUpRepository>().Use<WellCleanUpRepository>();
            For<IWellCleanConfig>().Use<WellCleanConfig>();
            For<IAmendmentService>().Use<AmendmentService>();
            For<IAmendmentRepository>().Use<AmendmentRepository>();
            For<IAmendmentFactory>().Use<AmendmentFactory>();

            // EF Contexts
            For<WellEntities>().Use<WellEntities>();

            // Mappers
            For<IBranchModelMapper>().Use<BranchModelMapper>();
            For<IDeliveryToDetailMapper>().Use<DeliveryToDetailMapper>();
            For<ISeasonalDateMapper>().Use<SeasonalDateMapper>();
            For<ICreditThresholdMapper>().Use<CreditThresholdMapper>();
            For<IWidgetWarningMapper>().Use<WidgetWarningMapper>();
            For<IDeliveryLineToJobDetailMapper>().Use<DeliveryLineToJobDetailMapper>();
            For<IJobDetailToDeliveryLineCreditMapper>().Use<JobDetailToDeliveryLineCreditMapper>();
            For<ISingleRouteMapper>().Use<SingleRouteMapper>();
            For<IStopMapper>().Use<StopMapper>();
            For<IDeliveryLineCreditMapper>().Use<DeliveryLineCreditMapper>();
            For<IOrderImportMapper>().Use<OrderImportMapper>();

            // delivery lines
            For<IDeliveryLinesAction>().Use<DeliveryLinesCredit>();

            // routes
            For<IRouteReadRepository>().Use<RouteReadRepository>();

            // search
            For<IAppSearchService>().Use<AppSearchService>();
            For<IAppSearchReadRepository>().Use<AppSearchReadRepository>();

            For<IAssigneeReadRepository>().Use<AssigneeReadRepository>();
            For<IStopStatusService>().Use<StopStatusService>();

            // Location/activity/line item
            For<ILocationRepository>().Use<LocationRepository>();
            For<ILineItemSearchReadRepository>().Use<LineItemSearchReadRepository>();
            For<ILineItemActionReadRepository>().Use<LineItemActionReadRepository>();
            For<ILineItemExceptionMapper>().Use<LineItemExceptionMapper>();
            For<ILineItemActionRepository>().Use<LineItemActionRepository>();
            For<ILineItemActionService>().Use<LineItemActionService>();

            // lookup
            For<ILookupService>().Use<LookupService>();
            For<ILookupRepository>().Use<LookupRepository>();

            For<ISubmitActionService>().Use<SubmitActionService>();
            For<ISubmitActionValidation>().Use<SubmitActionValidation>();
            For<IActionSummaryMapper>().Use<ActionSummaryMapper>();

            For<ILineItemActionCommentRepository>().Use<LineItemActionCommentRepository>();
            For<IDateThresholdService>().Use<DateThresholdService>();

            For<IGetJobResolutionStatus>().Use<JobService>();
            For<IActivityRepository>().Use<ActivityRepository>();
            For<IBulkEditService>().Use<BulkEditService>();
            For<IPatchSummaryMapper>().Use<PatchSummaryMapper>();
            For<IPostImportRepository>().Use<PostImportRepository>();
            For<IManualCompletionService>().Use<ManualCompletionService>();
            For<ICommentReasonRepository>().Use<CommentReasonRepository>();
            For<IDateThresholdRepository>().Use<DateThresholdRepository>();
            For<ICustomerRoyaltyExceptionRepository>().Use<CustomerRoyaltyExceptionRepository>();
            For<IEpodFileImportCommands>().Use<EpodFileImportCommands>();
            For<IEpodImportMapper>().Use<EpodImportMapper>();

            For<IStopService>().Use<StopService>();
            For<IRouteService>().Use<RouteService>();
            For<IActivityService>().Use<ActivityService>();
            For<ILocationService>().Use<LocationService>();
            For<IWellStatusAggregator>().Use<WellStatusAggregator>();
            For<IPodService>().Use<PodService>();
            For<IBranchProvider>().Use<BranchProvider>();
            For<IConnectionStringFactory>().Use<WellApiConnectionStringFactory>();
            For<IWellEntitiesConnectionString>().Use<WelllApiDbConfiguration>();
            For<IDbMultiConfiguration>().Use<WelllApiDbConfiguration>();
            For<IApprovalService>().Use<ApprovalService>();
            For<INotificationService>().Use<NotificationService>();
        }
    }
}