using System.IO;

namespace PH.Well.TranSend.Infrastructure
{
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

        public EpodFileProvider(IEpodSchemaProvider epodSchemaProvider, ILogger logger, IEpodDomainImportProvider epodDomainImportProvider,
            IEpodDomainImportService epodDomainImportService)
        {
            this.epodSchemaProvider = epodSchemaProvider;
            this.logger = logger;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
        }

        public void ListFilesAndProcess()
        {
            var filePath = Configuration.FileLocation;

            var filesToRead = Directory.GetFiles(filePath);

            foreach (var fileToRead in filesToRead)
            {
                var filenameWithoutPath = fileToRead.GetFilenameWithoutPath();

                if (epodDomainImportService.IsFileXmlType(fileToRead))
                {
                    var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filenameWithoutPath);
                    var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);
                    var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);
                    var isFileValidBySchema = epodSchemaProvider.IsFileValid(fileToRead, schemaPath);

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
