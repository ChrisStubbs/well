namespace PH.Well.TranSend.Infrastructure
{
    using System.IO;
    using System.Xml;

    using Contracts;
    using Common.Contracts;
    using Common.Extensions;
    using Well.Services.Contracts;

    public class EpodFileProvider : IEpodProvider
    {
        private readonly IEpodSchemaValidator epodSchemaValidator;
        private readonly IEpodImportProvider epodImportProvider;
        private readonly IEpodImportService epodImportService;
        private readonly ILogger logger;
        private readonly string correctExtension = ".xml";
        private readonly string assemblyName = "PH.Well.TranSend";

        public EpodFileProvider(IEpodSchemaValidator epodSchemaValidator, ILogger logger, IEpodImportProvider epodImportProvider,
            IEpodImportService epodImportService)
        {
            this.epodSchemaValidator = epodSchemaValidator;
            this.logger = logger;
            this.epodImportProvider = epodImportProvider;
            this.epodImportService = epodImportService;
        }

        public void Import()
        {
            var filePath = Configuration.FilePath;

            var filesToRead = Directory.GetFiles(filePath);

            foreach (var fileToRead in filesToRead)
            {
                var isFileValidBySchema = this.epodSchemaValidator.IsFileValid(fileToRead);

                if (isFileValidBySchema)
                {
                    this.epodImportProvider.ImportRouteHeader(fileToRead);
                }
            }
        }
    }
}
