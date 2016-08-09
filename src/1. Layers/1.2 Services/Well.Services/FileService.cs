namespace PH.Well.Services
{
    using System.IO;
    using System.Threading;

    using PH.Well.Common.Contracts;
    using PH.Well.Repositories;
    using PH.Well.Services.Contracts;

    public class FileService : IFileService
    {
        private readonly ILogger logger;

        private readonly IFileModule fileModule;

        public FileService(ILogger logger, IFileModule fileModule)
        {
            this.logger = logger;
            this.fileModule = fileModule;
        }

        public void Reject(string filePath)
        {
            var rejectFilePath = filePath.Insert(filePath.LastIndexOf('\\') + 1, "rejected\\");

            this.fileModule.Move(filePath, rejectFilePath);
        }

        public void Archive(string filePath)
        {
            var archiveFilePath = filePath.Insert(filePath.LastIndexOf('\\') + 1, "archive\\");

            this.fileModule.Move(filePath, archiveFilePath);
        }

        public void WaitForFile(string filePath)
        {
            try
            {
                using (var stream = this.fileModule.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    this.logger.LogDebug($"File ready for processing ({filePath})");
                }
            }
            catch (IOException)
            {
                Thread.Sleep(Configuration.WaitTimeInMillisecondsForFileToBeCopied);

                WaitForFile(filePath);
            }
        }

    }
}
