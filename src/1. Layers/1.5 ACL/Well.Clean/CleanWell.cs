namespace Well.Clean
{
    //using System.ComponentModel;
    using PH.Well.Common.Contracts;
    using PH.Well.Services.Contracts;
    using StructureMap;

    public class CleanWell
    {
        public void Process(IContainer container, ref string epodStatusMessage)
        {
            var logger = container.GetInstance<ILogger>();
            var epodService = container.GetInstance<IEpodDomainImportService>();
            epodService.GetRouteHeadersForDelete(ref epodStatusMessage);

            if (string.IsNullOrWhiteSpace(epodStatusMessage))
                epodStatusMessage = "Well Clean process complete with no errors";

            logger.LogDebug("Well Clean process complete with no errors");

        }
    }
}
