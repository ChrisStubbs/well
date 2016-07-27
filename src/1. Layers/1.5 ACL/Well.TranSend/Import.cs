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
        public void Process(IContainer container)
        {
            List<string> schemaErrors;
            var ePodFtpProvider = container.GetInstance<IEpodFtpProvider>();
            var logger = container.GetInstance<ILogger>();
            ePodFtpProvider.ListFilesAndProcess(out schemaErrors);

            var adamStatusMessage = schemaErrors.Any() ? $"Import completed with the following errors:  {string.Join(",", schemaErrors)}" : "Import complete with no errors";

            if (schemaErrors.Any())
            {
                logger.LogError(adamStatusMessage);
            }
            else
            {
                logger.LogDebug(adamStatusMessage);
            }

        }
    }
}
