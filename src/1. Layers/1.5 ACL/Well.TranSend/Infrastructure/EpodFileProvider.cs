namespace PH.Well.TranSend.Infrastructure
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Serialization;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.TranSend.Contracts;

    public class EpodFileProvider : IEpodProvider
    {
        private readonly IEpodUpdateService epodUpdateService;
        private readonly ILogger logger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IFileModule fileModule;

        public EpodFileProvider(IEpodUpdateService epodUpdateService, ILogger logger, IRouteHeaderRepository routeHeaderRepository,
            IFileModule fileModule)
        {
            this.epodUpdateService = epodUpdateService;
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.fileModule = fileModule;
        }

        public void Import()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
            var files = Directory.GetFiles(Configuration.DownloadFilePath).OrderBy(x => x);

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);

                var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));

                try
                {
                    using (var streamReader = new StreamReader(file))
                    {
                        var routes = (RouteDelivery)xmlSerializer.Deserialize(streamReader);

                        var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });

                        routes.RouteId = route.Id;

                        this.epodUpdateService.Update(routes, filename);
                    }

                    fileModule.MoveFile(file, Configuration.ArchiveLocation);
                    logger.LogDebug($"File {file} imported!");
                }
                catch (Exception exception)
                {
                    this.logger.LogError($"Epod update error in XML file {filename}!", exception);
                }
            }
        }
    }
}