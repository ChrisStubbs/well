namespace PH.Well.TranSend
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using StructureMap;

    public class Import
    {
        public void Process(IContainer container, ref string epodStatusMessage)
        {
            List<string> schemaErrors = new List<string>();
            var logger = container.GetInstance<ILogger>();
            var ePodProvider = container.GetInstance<IEpodProvider>();
            var webClient = container.GetInstance<IWebClient>();
            var configuration = container.GetInstance<IEpodImportConfiguration>();
            ePodProvider.ListFilesAndProcess(schemaErrors);

            epodStatusMessage = schemaErrors.Any() ? $"Epod file import completed with the following errors:  {string.Join(",", schemaErrors)}" : "Epod file import complete with no errors";

            if (schemaErrors.Any())
            {
                logger.LogError(epodStatusMessage);
            }
            else
            {
                logger.LogDebug(epodStatusMessage);
            }

            //webClient.DownloadString(configuration.DashboardRefreshEndpoint);
        }
    }
}
