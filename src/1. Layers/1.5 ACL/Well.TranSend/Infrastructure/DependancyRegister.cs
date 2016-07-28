using System;
namespace PH.Well.TranSend.Infrastructure
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
                    x.For<IEpodSchemaProvider>().Use<EpodSchemaProvider>();
                    x.For<IEpodProvider>().Use<EpodFileProvider>();
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
                    x.For<IEpodImportConfiguration>().Use<Configuration>();

                } );
        }
    }
}
