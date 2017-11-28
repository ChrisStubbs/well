namespace PH.Well.Clean
{
    using System;
    using System.IO;
    using Common.Contracts;

    public class TriggerCleanProcess : ITriggerCleanProcess
    {
        private readonly ILogger logger;

        public TriggerCleanProcess(ILogger logger)
        {
            this.logger = logger;
        }

        public void TriggerClean(string targetFolder)
        {
            try
            {
                var filename = Path.Combine(targetFolder, $"CLEAN__{DateTime.Now:yyyyMMdd_HHmmssff}.txt");
                logger.LogDebug($"Writing empty trigger file {filename}");
                File.Create(filename).Dispose();
            }
            catch (Exception ex)
            {
                logger.LogError("Error writing file", ex);
                throw;
            }
        }
    }
}