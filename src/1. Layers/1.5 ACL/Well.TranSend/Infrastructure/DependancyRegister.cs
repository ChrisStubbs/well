﻿namespace PH.Well.TranSend.Infrastructure
{
    using Common;
    using Common.Contracts;
    using Contracts;
    using Repositories;
    using Repositories.Contracts;
    using StructureMap;
    using Well.Services.Contracts;
    using Well.Services;
    using Well.Services.EpodImport;

    public static class DependancyRegister
    {
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<IEpodSchemaValidator>().Use<EpodSchemaValidator>();
                    x.For<IEpodProvider>().Use<EpodFtpProvider>();
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IEpodDomainImportProvider>().Use<EpodDomainImportProvider>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IEpodDomainImportService>().Use<EpodDomainImportService>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IAccountRepository>().Use<AccountRepository>();
                    x.For<IWebClient>().Use<WebClient>();
                    x.For<IFtpClient>().Use<FtpClient>();
                } );
        }
    }
}
