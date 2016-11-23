namespace PH.Well.TranSend.Infrastructure
{
    using System.IO;
    using Contracts;
    using Well.Services.Contracts;

    public class EpodFileProvider : IEpodProvider
    {
        private readonly IEpodSchemaValidator epodSchemaValidator;
        private readonly IEpodImportProvider epodImportProvider;

        public EpodFileProvider(IEpodSchemaValidator epodSchemaValidator, IEpodImportProvider epodImportProvider)
        {
            this.epodSchemaValidator = epodSchemaValidator;
            this.epodImportProvider = epodImportProvider;
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
