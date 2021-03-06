﻿namespace PH.Well.Services
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Xml.Serialization;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
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
        private readonly IImportedFileRepository importedFileRepository;

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
            IWellCleanUpService wellCleanUpService,
            IImportedFileRepository importedFileRepository)
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
            this.importedFileRepository = importedFileRepository;
        }

        public void Monitor(IAdamFileMonitorServiceConfig config)
        {
            var directoryInfo = new DirectoryInfo(config.RootFolder);

            var files =
                directoryInfo.GetFiles().Where(f => IsRecognisedFileName(f.Name))
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

                var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
                this.logger.LogDebug($"File {file.FullName} took {elapsedTime} to process");

                // Abort if a file called stop.txt exists in exe folder
                if (File.Exists("stop.txt"))
                {
                    File.Delete("stop.txt");
                    return;
                }
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

        public void Process(ImportFileInfo importFile, IAdamFileMonitorServiceConfig config)
        {
            if (importedFileRepository.HasFileAlreadyBeenImported(importFile.Name))
            {
                this.logger.LogDebug($"{importFile.Name} ignored as already in system !");
                this.fileModule.MoveFile(importFile.FullName, GetArchivePath(importFile, config, false));
                return;
            }

            var filename = importFile.Name;
            var fileType = this.fileTypeService.DetermineFileType(filename);

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

            bool isSuccess = false;
            switch (fileType)
            {
                case EpodFileType.Route:
                    isSuccess = TryHandleRoute(importFile.FullName, filename, config);
                    break;

                case EpodFileType.Order:
                    isSuccess = TryHandleOrder(importFile.FullName, filename, config);
                    break;

                case EpodFileType.Epod:
                    try
                    {
                        isSuccess = epodProvider.TryImport(importFile.FullName, filename, config);
                    }
                    catch (Exception exception)
                    {
                        this.LogError(exception, importFile.FullName);
                        isSuccess = false;
                    }
                    break;

                case EpodFileType.Clean:
                    try
                    {
                        wellCleanUpService.Clean().Wait();
                        isSuccess = true;
                    }
                    catch (Exception exception)
                    {
                        this.LogError(exception, importFile.FullName);
                        isSuccess = false;
                    }
                    break;
            }

            this.fileModule.MoveFile(importFile.FullName, GetArchivePath(importFile, config, isSuccess));
            this.logger.LogDebug($"{importFile.FullName} processed!");
        }

        private bool TryHandleRoute(string filePath, string filename, IImportConfig config)
        {
            var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var routes = (RouteDelivery)xmlSerializer.Deserialize(reader);

                    var route = this.routeHeaderRepository.Create(new Routes { FileName = filename });

                    routes.RouteId = route.Id;
                    bool hasErrors;
                    this.adamImportService.Import(routes, filename, config, out hasErrors);
                    return !hasErrors;
                }
            }
            catch (Exception exception)
            {
                this.LogError(exception, filePath);
                return false;
            }
        }

        private bool TryHandleOrder(string filePath, string filename, IImportConfig config)
        {
            var xmlSerializer = new XmlSerializer(typeof(RouteUpdates));
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var routes = (RouteUpdates)xmlSerializer.Deserialize(reader);

                    this.routeHeaderRepository.Create(new Routes { FileName = filename });

                    this.adamUpdateService.Update(routes, config);
                    return true;

                }
            }
            catch (Exception exception)
            {
                this.LogError(exception, filePath);
                return false;
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

        private string GetArchivePath(ImportFileInfo importFile, IAdamFileMonitorServiceConfig config, bool isSuccessful)
        {
            var pathString = Path.Combine(config.ArchiveFolder, GetDateStampFromFile(importFile).ToString("yyyyMMdd"));
            
            if (!isSuccessful)
            {
                pathString = Path.Combine(pathString, "Failures");
            }

            return pathString;
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

            public ImportFileInfo(FileInfo fileInfo) : this(fileInfo.FullName, fileInfo.Name, fileInfo.LastWriteTime, fileInfo.CreationTime)
            {

            }
        }
    }
}
