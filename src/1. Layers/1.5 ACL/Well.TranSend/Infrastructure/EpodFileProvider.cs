namespace PH.Well.TranSend.Infrastructure
{
    using System.Collections.Generic;
    using System.IO;
    using Contracts;
    using Common.Contracts;
    using Common.Extensions;
    using Well.Services.Contracts;

    public class EpodFileProvider : IEpodProvider
    {
        private readonly IEpodSchemaProvider epodSchemaProvider;
        private readonly IEpodDomainImportProvider epodDomainImportProvider;
        private readonly IEpodDomainImportService epodDomainImportService;
        private readonly ILogger logger;
        private readonly string correctExtension = ".xml";
        private readonly string assemblyName = "PH.Well.TranSend";
        private readonly IEpodImportConfiguration config;

        public EpodFileProvider(IEpodSchemaProvider epodSchemaProvider, ILogger logger, IEpodDomainImportProvider epodDomainImportProvider,
            IEpodDomainImportService epodDomainImportService, IEpodImportConfiguration config)
        {
            this.epodSchemaProvider = epodSchemaProvider;
            this.logger = logger;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
            this.config = config;
        }

        public void ListFilesAndProcess(List<string> schemaErrors)
        {
            var filePath = config.FilePath;

            var filesToRead = Directory.GetFiles(filePath);

            foreach (var fileToRead in filesToRead)
            {
                var filenameWithoutPath = fileToRead.GetFilenameWithoutPath();
                var errors = new List<string>();

                if (epodDomainImportService.IsFileXmlType(fileToRead))
                {
                    var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filenameWithoutPath);
                    var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);
                    var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);
                    var isFileValidBySchema = epodSchemaProvider.IsFileValid(fileToRead, schemaPath, ref errors);

                    if (!isFileValidBySchema)
                    {
                        logger.LogError($"file {fileToRead} failed schema validation");
                    }
                    else
                    {
                        var epodType = epodDomainImportService.GetEpodFileType(fileTypeIndentifier);
                        epodDomainImportProvider.ImportRouteHeader(fileToRead, epodType);
                    }
                }
            }
        }
    }
}
