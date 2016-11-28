namespace PH.Well.TranSend.Infrastructure
{
    using System.IO;
    using Contracts;
    using Well.Services.Contracts;

    public class EpodFileProvider : IEpodProvider
    {
        private readonly IEpodImportProvider epodImportProvider;

        public EpodFileProvider(IEpodImportProvider epodImportProvider)
        {
            this.epodImportProvider = epodImportProvider;
        }

        public void Import()
        {
            var filePath = Configuration.FilePath;

            var filesToRead = Directory.GetFiles(filePath);

            foreach (var fileToRead in filesToRead)
            {
                this.epodImportProvider.ImportRouteHeader(fileToRead);
            }
        }
    }
}
