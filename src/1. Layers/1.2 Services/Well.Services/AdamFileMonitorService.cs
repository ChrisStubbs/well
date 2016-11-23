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

        private readonly IEpodImportProvider epodImportProvider;

        private readonly IEpodImportService epodImportService;

        private string RootFolder { get; set; }

        public AdamFileMonitorService(
            ILogger logger, 
            IFileService fileService, 
            IEpodSchemaValidator epodSchemaValidator, 
            IEpodImportProvider epodImportProvider,
            IEpodImportService epodImportService)
        {
            this.logger = logger;
            this.fileService = fileService;
            this.epodSchemaValidator = epodSchemaValidator;
            this.epodImportProvider = epodImportProvider;
            this.epodImportService = epodImportService;
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

            this.fileService.WaitForFile(args.FullPath);

            this.Process(args.FullPath);
        }

        public void Process(string filePath)
        {
            var filenameWithoutPath = Path.GetFileName(filePath);

            if (filenameWithoutPath.EndsWith("xml"))
            {
                var isFileValidBySchema = this.epodSchemaValidator.IsFileValid(filePath);

                if (isFileValidBySchema)
                {
                    this.epodImportProvider.ImportRouteHeader(filePath);
                }
            }
        }
    }
}
