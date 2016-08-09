namespace PH.Well.BDD.Steps
{
    using System.IO;

    using Adam;
    using Framework.Context;

    using PH.Well.Adam.Contracts;
    using PH.Well.Common.Extensions;

    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class AdamImportSteps
    {
        private readonly IContainer container;
        private readonly IAdamImportConfiguration config;

        public AdamImportSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.config = this.container.GetInstance<IAdamImportConfiguration>();
        }

        [Given(@"I have loaded the Adam route data")]
        public void LoadAdamRouteData()
        {
            var adamImport = new Import();
            
            //adamImport.Process(container);

            //var archiveLocation = this.config.ArchiveLocation;
            //var originalLocation = this.config.FilePath;

            string[] fileList = Directory.GetFiles(this.config.ArchiveLocation, "*.xml*");

            foreach (var file in fileList)
            {
                var filenameWithoutPath = file.GetFilenameWithoutPath();
                File.Move(file, Path.Combine(this.config.FilePath, filenameWithoutPath));
            }
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
