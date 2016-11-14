namespace PH.Well.Services
{
    using System.IO;
    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;
    using PH.Well.Services.Contracts;

    public class AdamFileMonitorService : IAdamFileMonitorService
    {
        private readonly ILogger logger;

        private readonly IFileService fileService;

        private readonly IEpodSchemaValidator epodSchemaValidator;

        private readonly IEpodDomainImportProvider epodDomainImportProvider;

        private readonly IEpodDomainImportService epodDomainImportService;

        private string RootFolder { get; set; }

        public AdamFileMonitorService(
            ILogger logger, 
            IFileService fileService, 
            IEpodSchemaValidator epodSchemaValidator, 
            IEpodDomainImportProvider epodDomainImportProvider,
            IEpodDomainImportService epodDomainImportService)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.epodSchemaValidator = epodSchemaValidator;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
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

        public void Process(string filePath)
        {
            var filenameWithoutPath = filePath.GetFilenameWithoutPath();

            if (filenameWithoutPath.EndsWith("xml"))
            {
                var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filenameWithoutPath);

                var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);

                var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);

                var isFileValidBySchema = this.epodSchemaValidator.IsFileValid(filePath, schemaPath);

                if (isFileValidBySchema)
                {
                    var epodType = epodDomainImportService.GetEpodFileType(fileTypeIndentifier);

                    epodDomainImportProvider.ImportRouteHeader(filePath, epodType);

                    epodDomainImportService.CopyFileToArchive(
                    filePath,
                    filenameWithoutPath,
                    this.RootFolder + "\\archive");
                }
            }
        }
    }
}
