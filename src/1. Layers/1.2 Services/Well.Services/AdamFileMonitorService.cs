namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;
    using PH.Well.Services.Contracts;

    public class AdamFileMonitorService : IAdamFileMonitorService
    {
        public bool SchemaValidationEnabled { get; set; }

        private readonly ILogger logger;

        private readonly IFileService fileService;

        private readonly IEpodSchemaProvider epodSchemaProvider;

        private readonly IEpodDomainImportProvider epodDomainImportProvider;

        private readonly IEpodDomainImportService epodDomainImportService;

        private string RootFolder { get; set; }

        public AdamFileMonitorService(
            ILogger logger, 
            IFileService fileService, 
            IEpodSchemaProvider epodSchemaProvider, 
            IEpodDomainImportProvider epodDomainImportProvider,
            IEpodDomainImportService epodDomainImportService)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.epodSchemaProvider = epodSchemaProvider;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
            SchemaValidationEnabled = true;
        }

        public void Monitor(string rootFolder)
        {
            this.RootFolder = rootFolder;

            var watcher = new FileSystemWatcher
            {
                Path = rootFolder,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.LastAccess,
                IncludeSubdirectories = true
            };

            watcher.Created += this.ProcessCreated;
            // watcher.Renamed += this.ProcessRenamed;
            // watcher.Deleted += this.ProcessDeleted;

            watcher.EnableRaisingEvents = true;
        }

        private void ProcessCreated(object o, FileSystemEventArgs args)
        {
            // ignore archive and rejected folders
            if (args.FullPath.Contains("archive") || args.FullPath.Contains("rejected")) return;

            this.logger.LogDebug($"File created ({args.FullPath})");

            this.fileService.WaitForFile(args.FullPath);

            this.Process(args.FullPath);
        }

        public List<string> Process(string filePath, bool archive = true)
        {
            var schemaErrors = new List<string>();

            var filenameWithoutPath = filePath.GetFilenameWithoutPath();

            if (epodDomainImportService.IsFileXmlType(filenameWithoutPath))
            {
                var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filenameWithoutPath);

                if (SchemaValidationEnabled)
                {
                    var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);

                    var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);

                    var validationErrors = new List<string>();
                    var isFileValidBySchema = epodSchemaProvider.IsFileValid(filePath, schemaPath, validationErrors);

                    if (!isFileValidBySchema && validationErrors.Count > 0)
                    {
                        var validationError =
                            $"file {filenameWithoutPath} failed schema validation with the following: {string.Join(",", validationErrors)}";

                        schemaErrors.Add(validationError);
                        logger.LogError(validationError);
                        return schemaErrors;
                    }
                }

                var epodType = epodDomainImportService.GetEpodFileType(fileTypeIndentifier);

                epodDomainImportProvider.ImportRouteHeader(filePath, epodType);

                if (archive)
                {
                    epodDomainImportService.CopyFileToArchive(
                    filePath,
                    filenameWithoutPath,
                    this.RootFolder + "\\archive");
                }
            }

            return schemaErrors;
        }

        /*private void ProcessDeleted(object o, FileSystemEventArgs args)
        {
            this.logger.LogDebug($"File deleted ({args.FullPath})");
        }

        private void ProcessRenamed(object o, RenamedEventArgs args)
        {
            this.logger.LogDebug($"File renamed ({args.FullPath})");
        }*/
    }
}
