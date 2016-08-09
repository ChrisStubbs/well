namespace PH.Well.BDD.Steps
{
    using System.IO;

    using Framework.Context;

    using PH.Well.BDD.Framework;
    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class AdamImportSteps
    {
        private readonly IContainer container;

        public AdamImportSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
        }

        [Given(@"I have loaded the Adam route data")]
        public void LoadAdamRouteData()
        {
            var logger = this.container.GetInstance<ILogger>();
            var fileService = this.container.GetInstance<IFileService>();
            var epodSchemaProvider = this.container.GetInstance<IEpodSchemaProvider>();
            var epodDomainImportProvider = this.container.GetInstance<IEpodDomainImportProvider>();
            var epodDomainImportService = this.container.GetInstance<IEpodDomainImportService>();

            var adamImport = new AdamFileMonitorService(logger, fileService, epodSchemaProvider, epodDomainImportProvider, epodDomainImportService);

            adamImport.Process(Configuration.AdamFile, false);
        }

        [Given(@"I have loaded the Adam route data that has 21 lines")]
        public void GivenIHaveLoadedTheAdamRouteDataThatHasLines()
        {
            for (int i = 0; i < 7; i++)
            {
                LoadAdamRouteData();
            }
        }

    }
}
