namespace PH.Well.Adam
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using StructureMap;

    public class Import
    {
        public void Process(IContainer container)
        {
            List<string> schemaErrors;
            var adamRouteFileProvider = container.GetInstance<IAdamRouteFileProvider>();
            var logger = container.GetInstance<ILogger>();
            adamRouteFileProvider.ListFilesAndProcess(container.GetInstance<IAdamImportConfiguration>(), out schemaErrors);

            var adamStatusMessage = schemaErrors.Any()
                ? $"Import completed with the following errors: {string.Join(",", schemaErrors)}"
                : "Import complete with no errors";

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
