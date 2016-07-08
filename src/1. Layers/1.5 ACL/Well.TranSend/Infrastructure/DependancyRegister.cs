using System;
namespace PH.Well.TranSend.Infrastructure
{
    using Common;
    using Common.Contracts;
    using Contracts;
    using Repositories;
    using Repositories.Contracts;
    using Services;
    using StructureMap;

    public static class DependancyRegister
    {
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<IEpodSchemaProvider>().Use<EpodSchemaProvider>();
                    x.For<IEpodFtpProvider>().Use<EpodFtpProvider>();
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IEpodDomainImportProvider>().Use<EpodDomainImportProvider>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IStopRepository>().Use<StopRepository>();
                    x.For<IEpodDomainImportService>().Use<EpodDomainImportService>();

                } );
        }
    }
}
