namespace PH.Well.TranSend
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using StructureMap;
    using global::PH.Well.Domain;

    public class Import
    {
        public void Process(IContainer container, out string epodStatusMessage)
        {
            List<string> schemaErrors;
            var logger = container.GetInstance<ILogger>();
            ePodFtpProvider.ListFilesAndProcess(out schemaErrors);

            epodStatusMessage = schemaErrors.Any() ? $"Epod file import completed with the following errors:  {string.Join(",", schemaErrors)}" : "Epod file import complete with no errors";

            if (schemaErrors.Any())
            {
                logger.LogError(epodStatusMessage);
            }
            else
            {
                logger.LogDebug(epodStatusMessage);
            }

        }
    }
}
