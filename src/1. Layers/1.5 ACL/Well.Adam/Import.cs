﻿namespace PH.Well.Adam
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using StructureMap;

    public class Import
    {
        public void Process(IContainer container, out string adamStatusMessage)
        {
            List<string> schemaErrors;
            var adamRouteFileProvider = container.GetInstance<IAdamRouteFileProvider>();
            var logger = container.GetInstance<ILogger>();
            adamRouteFileProvider.ListFilesAndProcess(container.GetInstance<IAdamImportConfiguration>(), out schemaErrors);

            adamStatusMessage = schemaErrors.Any()
                ? $"Adam file import completed with the following errors: {string.Join(",", schemaErrors)}"
                : "Adam file import completed with no errors";

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
