namespace PH.Well.Adam.Infrastructure
{
    using System.IO;
    using Contracts;
    using Common.Contracts;
    using Common.Extensions;
    using PH.Well.Services.Contracts;

    public class AdamRouteFileProvider : IAdamRouteFileProvider
    {
        private readonly IEpodSchemaProvider epodSchemaProvider;
        private readonly IEpodDomainImportProvider epodDomainImportProvider;
        private readonly IEpodDomainImportService epodDomainImportService;
        private readonly ILogger logger;
        private readonly string correctExtension = ".xml";
        private readonly string assemblyName = "PH.Well.TranSend";
        private string archiveLocation;


        public AdamRouteFileProvider(IEpodSchemaProvider epodSchemaProvider, IEpodDomainImportProvider epodDomainImportProvider,
                               IEpodDomainImportService epodDomainImportService, ILogger logger)
        {
            this.epodSchemaProvider = epodSchemaProvider;
            this.logger = logger;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
        }


        public void ListFilesAndProcess(IAdamImportConfiguration config)
        {
            var filepath = config.FilePath;
            this.archiveLocation = ConfigurationManager.AppSettings["archiveLocation"];

            var ePodFiles = Directory.GetFiles(filepath, config.SearchPattern, SearchOption.TopDirectoryOnly);

            foreach (var file in ePodFiles)
            {
                var filenameWithoutPath = file.GetFilenameWithoutPath();

                if (epodDomainImportService.IsFileXmlType(filenameWithoutPath))
                {
                    var fileTypeIndentifier = epodDomainImportService.GetFileTypeIdentifier(filenameWithoutPath);
                    var schemaName = epodDomainImportService.MatchFileNameToSchema(fileTypeIndentifier);
                    var schemaPath = epodDomainImportService.GetSchemaFilePath(schemaName);
                    var isFileValidBySchema = epodSchemaProvider.IsFileValid(file, schemaPath);

                    if (!isFileValidBySchema)
                    {
                        logger.LogError($"file {filenameWithoutPath} failed schema validation");
                    }
                    else
                    {
                        var epodType = epodDomainImportService.GetEpodFileType(fileTypeIndentifier);
                        epodDomainImportProvider.ImportRouteHeader(file, epodType); 
                        epodDomainImportService.CopyFileToArchive(file, filenameWithoutPath, archiveLocation);                    
                    }
                }

            }
        }



    }
}
