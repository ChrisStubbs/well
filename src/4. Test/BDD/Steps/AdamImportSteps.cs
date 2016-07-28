using System;
using System.IO;
using PH.Well.Adam.Contracts;
using PH.Well.Common.Extensions;

namespace PH.Well.BDD.Steps
{
    using Adam;
    using Framework.Context;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class AdamImportSteps
    {
        private readonly IContainer container;
        private string adamStatusMessage;
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
            
            adamImport.Process(container, out adamStatusMessage);

            var archiveLocation = @"D:\wellEpodArchive";
            //var originalLocation = @"D:\_dev\well\src\4. Test\BDD\bin\Debug\Xml";
            // string[] fileList = Directory.GetFiles(this.config.ArchiveLocation, "*.xml*");

            string[] fileList = Directory.GetFiles(archiveLocation, "*.xml*");

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
