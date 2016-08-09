namespace PH.Well.Adam
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;

    public class Import
    {
        /*public void Process(IContainer container)
        {
            var schemaErrors = new List<string>();

            var adamRouteFileProvider = container.GetInstance<IAdamRouteFileProvider>();
            var logger = container.GetInstance<ILogger>();
            adamRouteFileProvider.ListFilesAndProcess(container.GetInstance<IAdamImportConfiguration>(), schemaErrors);

            var adamStatusMessage = schemaErrors.Any()
                ? $"Adam file import completed with the following errors: {string.Join(",", schemaErrors)}"
                : "Adam file import completed with no errors";

            logger.LogDebug(adamStatusMessage);
        }*/
    }
}
