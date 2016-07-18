namespace PH.Well.Adam.Infrastructure
{

    using System;
    using System.IO;
    using System.Net;
    using Contracts;
    using System.Configuration;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Domain.Enums;
    using Well.Services.Contracts;
    using static System.String;

    public class AdamRouteFileProvider : IAdamRouteFileProvider
    {
        private readonly IEpodSchemaProvider epodSchemaProvider;
        private readonly IEpodDomainImportProvider epodDomainImportProvider;
        private readonly IEpodDomainImportService epodDomainImportService;
        private readonly ILogger logger;
        private readonly string correctExtension = ".xml";
        private readonly string assemblyName = "PH.Well.TranSend";


        public AdamRouteFileProvider(IEpodSchemaProvider epodSchemaProvider, IEpodDomainImportProvider epodDomainImportProvider,
                               IEpodDomainImportService epodDomainImportService, ILogger logger)
        {
            this.epodSchemaProvider = epodSchemaProvider;
            this.logger = logger;
            this.epodDomainImportProvider = epodDomainImportProvider;
            this.epodDomainImportService = epodDomainImportService;
        }


        public void ListFilesAndProcess()
        {
            var filepath = ConfigurationManager.AppSettings["downloadFilePath"];

            var ePodFiles = Directory.GetFiles(filepath, "*.xml*", SearchOption.TopDirectoryOnly);

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
                    }
                }

            }
        }

    }
}
