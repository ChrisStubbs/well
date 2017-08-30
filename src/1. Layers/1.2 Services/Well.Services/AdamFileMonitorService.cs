namespace PH.Well.Services
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
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
        readonly Regex routeOrOrderRegEx = new Regex("^(ROUTE|ORDER)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
            var directoryInfo = new DirectoryInfo(rootFolder);
            
            var files =
                directoryInfo.GetFiles().Where(f => IsRouteOrOrderFile(f.Name))
                        .OrderBy(f => GetDateTimeStampFromFileName(f.Name));

            foreach (var file in files)
            {
                this.fileService.WaitForFile(file.FullName);
                this.Process(file.FullName);
            }
        }

        public bool IsRouteOrOrderFile(string fileName)
        {
            return routeOrOrderRegEx.IsMatch(fileName);
        }

        public string GetDateTimeStampFromFileName(string fileName)
        {
            var dateTimeStamp = fileName.Substring(10, 11);
            return dateTimeStamp;
        }

        public void Process(string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var fileType = this.fileTypeService.DetermineFileType(filename);
            var adicionalDataPath = string.Empty;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            switch (fileType)
            {
                case EpodFileType.AdamInsert:
                    this.AdamImport(filePath, filename);
                    break;

                case EpodFileType.AdamUpdate:
                    this.AdamUpdate(filePath, filename);
                    break;

                case EpodFileType.Unknown:
                    this.logger.LogDebug($"File ({filePath}) is not recognised!");
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, $"File ({filePath}) is not recognised!", 5049);
                    adicionalDataPath = "UnknownFiles";
                    break;
            }

            this.fileModule.MoveFile(
                filePath,
                Path.Combine(Configuration.ArchiveLocation, DateTime.Now.ToString("yyyyMMdd"), adicionalDataPath));

            this.logger.LogDebug($"{filePath} processed!");
        }

        private void AdamImport(string filePath, string filename)
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
