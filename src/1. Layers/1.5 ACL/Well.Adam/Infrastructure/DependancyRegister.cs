namespace PH.Well.Adam.Infrastructure
{
    using Common;
    using Common.Contracts;
    using Contracts;
    using Repositories;
    using Repositories.Contracts;
    using Services;
    using Services.Contracts;
    using Services.EpodImport;
    using StructureMap;

    public static class DependancyRegister
    {
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<IEpodSchemaProvider>().Use<EpodSchemaProvider>();
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
                    x.For<IAdamRouteFileProvider>().Use<AdamRouteFileProvider>();
                    x.For<IAdamImportConfiguration>().Use<Configuration>();
                } );
        }
    }
}
