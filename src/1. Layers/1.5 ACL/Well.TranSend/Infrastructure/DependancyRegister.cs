﻿namespace PH.Well.TranSend.Infrastructure
{
    using Common;
    using Common.Contracts;
    using Contracts;

    using PH.Well.Services.EpodServices;

    using Repositories;
    using Repositories.Contracts;
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
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<IEpodUpdateService>().Use<EpodUpdateService>();
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
                    x.For<IAdamImportService>().Use<AdamImportService>();
                    x.For<IExceptionEventRepository>().Use<ExceptionEventRepository>();
                    x.For<IDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteMapper>().Use<RouteMapper>();
                    x.For<IJobStatusService>().Use<JobStatusService>();
                    x.For<IUserNameProvider>().Use<TranSendUserNameProvider>();
                    x.For<IPodTransactionFactory>().Use<PodTransactionFactory>();
                    x.For<IPostImportRepository>().Use<PostImportRepository>();
                    x.For<IJobResolutionStatus>().Use<JobResolutionStatus>();
                    x.For<IUserThresholdService>().Use<UserThresholdService>();
                    x.For<ICreditThresholdRepository>().Use<CreditThresholdRepository>();
                    x.For<IUserRepository>().Use<UserRepository>();
#if DEBUG
                    x.For<IEpodProvider>().Use<EpodFileProvider>();
#else
                    x.For<IEpodProvider>().Use<EpodFtpProvider>();
#endif
                });
        }
    }
}
