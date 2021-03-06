﻿namespace PH.Well.TranSend.Infrastructure
{
    using Common;
    using Common.Contracts;
    using Contracts;

    using PH.Well.Services.EpodServices;

    using Repositories;
    using Repositories.Contracts;
    using Repositories.Read;
    using StructureMap;
    using Well.Services.Contracts;
    using Well.Services;

    public static class DependancyRegister
    {
        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<ITransendImport>().Use<TransendImport>();
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<IDapperProxy>().Use<WellDapperProxy>();
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IAccountRepository>().Use<AccountRepository>();
                    x.For<IWebClient>().Use<WebClient>();
                    x.For<IFtpClient>().Use<FtpClient>();
                    x.For<IFileTypeService>().Use<FileTypeService>();
                    x.For<IFileModule>().Use<FileModule>();
                    x.For<IExceptionEventRepository>().Use<ExceptionEventRepository>();
                    x.For<IOrderImportMapper>().Use<OrderImportMapper>();
                    x.For<IJobService>().Use<JobService>();
                    x.For<IUserNameProvider>().Use<TranSendUserNameProvider>();
                    x.For<PH.Common.Security.Interfaces.IUserNameProvider>().Use<TranSendUserNameProvider>();
                    x.For<IPodTransactionFactory>().Use<PodTransactionFactory>();
                    x.For<IPostImportRepository>().Use<PostImportRepository>();
                    x.For<IGetJobResolutionStatus>().Use<JobService>();
                    x.For<IUserThresholdService>().Use<UserThresholdService>();
                    x.For<ICreditThresholdRepository>().Use<CreditThresholdRepository>();
                    x.For<IUserRepository>().Use<UserRepository>();
                    x.For<IDateThresholdService>().Use<DateThresholdService>();
                    x.For<ILineItemSearchReadRepository>().Use<LineItemSearchReadRepository>();
                    x.For<IDapperReadProxy>().Use<DapperReadProxy>();
                    x.For<IDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IAssigneeReadRepository>().Use<AssigneeReadRepository>();
                    x.For<ISeasonalDateRepository>().Use<SeasonalDateRepository>();
                    x.For<IDateThresholdRepository>().Use<DateThresholdRepository>();
                    x.For<ICustomerRoyaltyExceptionRepository>().Use<CustomerRoyaltyExceptionRepository>();

                    x.For<IEpodImportService>().Use<EpodImportService>();
                    x.For<IImportService>().Use<ImportService>();
                    x.For<IEpodImportMapper>().Use<EpodImportMapper>();
                    x.For<IEpodFileImportCommands>().Use<EpodFileImportCommands>();

                    x.For<IStopService>().Use<StopService>();
                    x.For<IRouteService>().Use<RouteService>();
                    x.For<IActivityService>().Use<ActivityService>();
                    x.For<ILocationService>().Use<LocationService>();
                    x.For<IWellStatusAggregator>().Use<WellStatusAggregator>();
                    x.For<IActivityRepository>().Use<ActivityRepository>();
                    x.For<IDeadlockRetryConfig>().Use<Configuration>();
                    x.For<IDeadlockRetryHelper>().Use<DeadlockRetryHelper>();
                    x.For<IEpodProvider>().Use<EpodFtpProvider>();
                    x.For<IPodService>().Use<PodService>();
                    x.For<INotificationRepository>().Use<NotificationRepository>();

                    x.For<ILineItemActionRepository>().Use<LineItemActionRepository>();
                });
                
                
                
        }
    }
}
