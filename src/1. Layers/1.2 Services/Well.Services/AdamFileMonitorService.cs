namespace PH.Well.Services
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class AdamFileMonitorService : IAdamFileMonitorService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IFileService fileService;
        private readonly IFileTypeService fileTypeService;
        private readonly IFileModule fileModule;
        private readonly IAdamImportService adamImportService;
        private readonly IAdamUpdateService adamUpdateService;
        private readonly IRouteHeaderRepository routeHeaderRepository;

        private string RootFolder { get; set; }

        public AdamFileMonitorService(
            ILogger logger,
            IEventLogger eventLogger,
            IFileService fileService,
            IFileTypeService fileTypeService,
            IFileModule fileModule,
            IAdamImportService adamImportService,
            IAdamUpdateService adamUpdateService,
            IRouteHeaderRepository routeHeaderRepository)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.fileService = fileService;
            this.fileTypeService = fileTypeService;
            this.fileModule = fileModule;
            this.adamImportService = adamImportService;
            this.adamUpdateService = adamUpdateService;
            this.routeHeaderRepository = routeHeaderRepository;
        }

        public void Monitor(string rootFolder)
        {
            this.RootFolder = rootFolder;

            var files = Directory.GetFiles(rootFolder);

            foreach (var file in files)
            {
                this.fileService.WaitForFile(file);

                this.Process(file);
            }
        }

        public void Process(string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var fileType = this.fileTypeService.DetermineFileType(filename);

            switch (fileType)
            {
                case EpodFileType.AdamInsert:
                    this.AdamInsert(filePath, filename);
                    break;
                case EpodFileType.AdamUpdate:
                    this.AdamUpdate(filePath, filename);
                    break;
                case EpodFileType.Unknown:
                    this.logger.LogDebug($"File ({filePath}) is not recognised!");
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"File ({filePath}) is not recognised!", 5049);
                    break;
            }

            this.fileModule.MoveFile(filePath, Configuration.ArchiveLocation);
            this.logger.LogDebug($"{filePath} processed!");
        }

        private void AdamInsert(string filePath, string filename)
        {
            var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var routes = (RouteDelivery)xmlSerializer.Deserialize(reader);

                    var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });

                    routes.RouteId = route.Id;

                    this.adamImportService.Import(routes);
                }
            }
            catch (Exception exception)
            {
                this.LogError(exception, filePath);
            }
        }

        private void AdamUpdate(string filePath, string filename)
        {
            var xmlSerializer = new XmlSerializer(typeof(RouteUpdates));
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var routes = (RouteUpdates)xmlSerializer.Deserialize(reader);

                    this.routeHeaderRepository.Create(new Routes { FileName = filename });

                    this.adamUpdateService.Update(routes);
                }
            }
            catch (Exception exception)
            {
                this.LogError(exception, filePath);
            }
        }

        private void LogError(Exception exception, string filePath)
        {
            this.logger.LogError($"Something went wrong with file {filePath}", exception);
            this.eventLogger.TryWriteToEventLog(
                EventSource.WellAdamXmlImport,
                $"Something went wrong with file {filePath}",
                3332);
        }
    }
}
