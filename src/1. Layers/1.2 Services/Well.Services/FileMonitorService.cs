namespace PH.Well.Services
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class FileMonitorService : IFileMonitorService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IFileService fileService;
        private readonly IFileTypeService fileTypeService;
        private readonly IFileModule fileModule;
        private readonly IAdamImportService adamImportService;
        private readonly IAdamUpdateService adamUpdateService;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IEpodFileProvider epodProvider;
        private readonly IWellCleanUpService wellCleanUpService;

        readonly Regex fileNameRegEx = new Regex("^(ROUTE|ORDER|EPOD|CLEAN)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public FileMonitorService(
            ILogger logger,
            IEventLogger eventLogger,
            IFileService fileService,
            IFileTypeService fileTypeService,
            IFileModule fileModule,
            IAdamImportService adamImportService,
            IAdamUpdateService adamUpdateService,
            IRouteHeaderRepository routeHeaderRepository,
            IEpodFileProvider epodProvider,
            IWellCleanUpService wellCleanUpService)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.fileService = fileService;
            this.fileTypeService = fileTypeService;
            this.fileModule = fileModule;
            this.adamImportService = adamImportService;
            this.adamUpdateService = adamUpdateService;
            this.routeHeaderRepository = routeHeaderRepository;
            this.epodProvider = epodProvider;
            this.wellCleanUpService = wellCleanUpService;
        }

        public void Monitor(string rootFolder)
        {
            var directoryInfo = new DirectoryInfo(rootFolder);

            var files =
                directoryInfo.GetFiles().Where(f => IsRecognisedFileName(f.Name))
                    .OrderBy(f => GetDateStampFromFile(new ImportFileInfo(f)));
            var stopWatch = new Stopwatch();

            foreach (var file in files)
            {
                stopWatch.Restart();

                this.logger.LogDebug($"Start to process file {file.FullName}");
                this.fileService.WaitForFile(file.FullName);
                this.Process(file.FullName);

                var ts = stopWatch.Elapsed;

                var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";

                this.logger.LogDebug($"File {file.FullName} took {elapsedTime} to process");
            }
        }

        public bool IsRecognisedFileName(string fileName)
        {
            return fileNameRegEx.IsMatch(fileName);
        }

        public DateTime GetDateStampFromFile(ImportFileInfo fileInfo)
        {
            var fileType = this.fileTypeService.DetermineFileType(fileInfo.Name);
            switch (fileType)
            {
                case EpodFileType.Route:
                case EpodFileType.Order:
                    return new[]
                    {
                        fileInfo.ModificationTime,
                        fileInfo.CreationTime
                    }.Min();
                case EpodFileType.Clean:
                case EpodFileType.Epod:
                    var nameParts = fileInfo.Name.Split('_');
                    var timeString = nameParts[2] + nameParts[3].Substring(0, 6);
                    return DateTime.ParseExact(timeString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Process(string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var fileType = this.fileTypeService.DetermineFileType(filename);
            var adicionalDataPath = string.Empty;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            switch (fileType)
            {
                case EpodFileType.Route:
                    this.HandleRoute(filePath, filename);
                    break;

                case EpodFileType.Order:
                    this.HandleOrder(filePath, filename);
                    break;

                case EpodFileType.Epod:
                    this.HandleEpod(filePath, filename);
                    break;
                case EpodFileType.Clean:
                    this.HandleClean(filePath);
                    break;
            }

            this.fileModule.MoveFile(
                filePath,
                Path.Combine(Configuration.ArchiveLocation, DateTime.Now.ToString("yyyyMMdd"), adicionalDataPath));

            this.logger.LogDebug($"{filePath} processed!");
        }

        private void HandleClean(string filePath)
        {
            try
            {
                wellCleanUpService.Clean().Wait();
            }
            catch (Exception exception)
            {
                this.LogError(exception, filePath);
            }
        }

        private void HandleRoute(string filePath, string filename)
        {
            var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var routes = (RouteDelivery)xmlSerializer.Deserialize(reader);

                    var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });

                    routes.RouteId = route.Id;

                    this.adamImportService.Import(routes, filename);
                }
            }
            catch (Exception exception)
            {
                this.LogError(exception, filePath);
            }
        }

        private void HandleOrder(string filePath, string filename)
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

        private void HandleEpod(string filePath, string filename)
        {
            try
            {
                this.epodProvider.Import(filePath, filename);
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

        public class ImportFileInfo
        {
            public string Name { get; }

            public DateTime ModificationTime { get; }

            public DateTime CreationTime { get; }

            public ImportFileInfo(string fileName, DateTime modificationTime, DateTime creationTime)
            {
                Name = fileName;
                ModificationTime = modificationTime;
                CreationTime = creationTime;
            }

            public ImportFileInfo(FileInfo fileInfo):this(fileInfo.Name,fileInfo.LastWriteTime,fileInfo.CreationTime)
            {
                
            }
        }
    }
}
