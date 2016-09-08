namespace Well.Clean
{
    //using System.ComponentModel;
    using System;
    using PH.Well.Common.Contracts;
    using PH.Well.Services.Contracts;
    using StructureMap;


    public class CleanWell
    {
        public void Process(IContainer container, ref string epodStatusMessage)
        {
            var logger = container.GetInstance<ILogger>();
            var epodService = container.GetInstance<IEpodDomainImportService>();
            epodService.WellClearDate = CleanDate();
            epodService.WellClearMonths = CleanMonths();
            epodService.GetRouteHeadersForDelete(ref epodStatusMessage);

            if (string.IsNullOrWhiteSpace(epodStatusMessage))
                epodStatusMessage = "Well Clean process complete with no errors";

            logger.LogDebug("Well Clean process complete with no errors");

        }

        private DateTime CleanDate()
        {
            return !string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["OverrideCleanDate"])
                ? DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["OverrideCleanDate"])
                : DateTime.Now;
        }

        private int CleanMonths()
        {
            return !string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["ClearMonths"])
                ? int.Parse(System.Configuration.ConfigurationManager.AppSettings["ClearMonths"])
                : 3;
        }


    }
}
