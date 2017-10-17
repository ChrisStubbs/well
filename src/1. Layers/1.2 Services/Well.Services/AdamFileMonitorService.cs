namespace PH.Well.Services
{
    using System;
    using System.Diagnostics;
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
        private readonly IEpodFileProvider epodProvider;

        readonly Regex routeOrOrderRegEx = new Regex("^(ROUTE|ORDER|EPOD)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public AdamFileMonitorService(
            ILogger logger,
            IEventLogger eventLogger,
            IFileService fileService,
            IFileTypeService fileTypeService,
            IFileModule fileModule,
            IAdamImportService adamImportService,
            IAdamUpdateService adamUpdateService,
            IRouteHeaderRepository routeHeaderRepository,
            IEpodFileProvider epodProvider)
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
        }

        public void Monitor(IAdamFileMonitorServiceConfig config)
        {
            var directoryInfo = new DirectoryInfo(config.RootFolder);

            if (!directoryInfo.Exists)
            {
                return;
            }

            var files =
                directoryInfo.GetFiles().Where(f => IsRouteOrOrderFile(f.Name))
                    .Select(x => new ImportFileInfo(x))
                    .OrderBy(GetDateStampFromFile).ToList();

            var stopWatch = new Stopwatch();

            foreach (var file in files)
            {
                stopWatch.Restart();

                this.logger.LogDebug($"Start to process file {file.FullName}");
                this.fileService.WaitForFile(file.FullName);
                this.Process(file, config);

                var ts = stopWatch.Elapsed;

                var elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);

                this.logger.LogDebug($"File {file.FullName} took {elapsedTime} to process");
            }
        }

        public bool IsRouteOrOrderFile(string fileName)
        {
            return routeOrOrderRegEx.IsMatch(fileName);
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
                case EpodFileType.Epod:
                    var nameParts = fileInfo.Name.Split('_');
                    var timeString = nameParts[2] + nameParts[3].Substring(0, 6);
                    return DateTime.ParseExact(timeString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Process(ImportFileInfo importFile, IAdamFileMonitorServiceConfig config)
        {
            var filename = importFile.Name;
            var fileType = this.fileTypeService.DetermineFileType(filename);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            if (config.ProcessFiles)
            {
                switch (fileType)
                {
                    case EpodFileType.Route:
                        this.HandleRoute(importFile.FullName, filename);
                        break;

                    case EpodFileType.Order:
                        this.HandleOrder(importFile.FullName, filename);
                        break;

                    case EpodFileType.Epod:
                        this.HandleEpod(importFile.FullName, filename);
                        break;
                }
            }
            this.fileModule.MoveFile(importFile.FullName, GetArchivePath(importFile, config));
            this.logger.LogDebug($"{importFile.FullName} processed!");
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

        private string GetArchivePath(ImportFileInfo importFile,IAdamFileMonitorServiceConfig config)
        {
            return Path.Combine(config.RootFolder, "archive", GetDateStampFromFile(importFile).ToString("yyyyMMdd"));
        }

        public class ImportFileInfo
        {
            /// <summary>
            /// Full file path path
            /// </summary>
            public string FullName { get; set; }

            public string Name { get; }

            public DateTime ModificationTime { get; }

            public DateTime CreationTime { get; }

            public ImportFileInfo(string fullName, string fileName, DateTime modificationTime, DateTime creationTime)
            {
                FullName = fullName;
                Name = fileName;
                ModificationTime = modificationTime;
                CreationTime = creationTime;
            }

            public ImportFileInfo(FileInfo fileInfo):this(fileInfo.FullName, fileInfo.Name,fileInfo.LastWriteTime,fileInfo.CreationTime)
            {
                
            }
        }
    }
}
