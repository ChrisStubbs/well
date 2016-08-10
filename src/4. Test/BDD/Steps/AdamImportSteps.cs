namespace PH.Well.BDD.Steps
{
    using System.IO;
    using System;

    using Framework.Context;

    using PH.Well.BDD.Framework;
    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    using StructureMap;
    using TechTalk.SpecFlow;
    using Framework.Extensions;
    using NUnit.Framework;
    using TechTalk.SpecFlow.Assist.ValueRetrievers;

    [Binding]
    public class AdamImportSteps
    {
        private readonly IContainer container;
        const string currentAdamRouteFile = "PH_ROUTES_30062016_02.xml";
        const string currentEpodRouteFile = "ePOD__20160701_10452212189454.xml";
        private const string ParentNode = "RouteHeader";

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

            adamImport.Process(container, ref adamStatusMessage);
            logger.LogDebug("Calling file monitor service");
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

        [Given(@"I have an invalid ADAM route file '(.*)' with a '(.*)' node at position '(.*)' with the '(.*)' node missing")]
        public void GivenIHaveAnInvalidADAMRouteFileWithANodeAtPositionWithTheNodeMissing(string resultFile, string parentNode, int nodePosition, string nodeToRemove)
        {
            var fileFolder = "xml";
            ProcessImportFile(resultFile, parentNode, nodePosition, nodeToRemove, fileFolder, currentAdamRouteFile);
        }

        [Given(@"I have an invalid Epod route file '(.*)' with a '(.*)' node at position '(.*)' with the '(.*)' node missing")]
        public void GivenIHaveAnInvalidEPodRouteFileWithANodeAtPositionWithTheNodeMissing(string resultFile, string parentNode, int nodePosition, string nodeToRemove)
        {
            var fileFolder = "Epod";
            ProcessImportFile(resultFile, parentNode, nodePosition, nodeToRemove, fileFolder, currentEpodRouteFile);
        }


        [When(@"I import the route file '(.*)' into the well")]
        public void WhenIImportTheRouteFileIntoTheWell(string routeFile)
        {
            var schemaErrors = new List<string>();

            var adamContainer = container.GetInstance<IAdamRouteFileProvider>();
            var configContainer = container.GetInstance<IAdamImportConfiguration>();

            configContainer.FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RouteFiles");
            configContainer.SearchPattern = routeFile.StartsWith("ePOD") ? "Epod_*" : configContainer.SearchPattern;

            adamContainer.ListFilesAndProcess(configContainer, ref schemaErrors); 

            if(schemaErrors.Count > 0)
                ScenarioContext.Current.Add("schemaErrors", schemaErrors[0]);

        }

        [Then(@"The schema validation error should be ""(.*)""")]
        public void ThenTheSchemaValidationErrorShouldBe(string expectedSchemaErrorMessage)
        {
            var actualSchemaResult = ScenarioContext.Current["schemaErrors"];
            Assert.That(actualSchemaResult, Is.EqualTo(expectedSchemaErrorMessage));
             
        }

        [Given(@"I have an invalid ADAM route file '(.*)' with a '(.*)' node at position '(.*)' with a '(.*)' node added with a value of '(.*)'")]
        public void GivenIHaveAnInvalidADAMRouteFileWithANodeAtPositionWithANodeAddedWithAValueOf(string resultFile, string parentNode, int nodePosition, string nodeToAdd, string nodeValue)
        {
            var fileFolder = "xml";
            ProcessImportFileWithNodeAdded(resultFile, parentNode, nodePosition, nodeToAdd, nodeValue,  fileFolder, currentAdamRouteFile);
        }

        [Given(@"I have an invalid Epod route file '(.*)' with a '(.*)' node at position '(.*)' with a '(.*)' node added with a value of '(.*)'")]
        public void GivenIHaveAnInvalidEpodRouteFileWithANodeAtPositionWithANodeAddedWithAValueOf(string resultFile, string parentNode, int nodePosition, string nodeToAdd, string nodeValue)
        {
            var fileFolder = "Epod";
            ProcessImportFileWithNodeAdded(resultFile, parentNode, nodePosition, nodeToAdd, nodeValue, fileFolder, currentEpodRouteFile);
        }



        private void ProcessImportFileWithNodeAdded(string resultFile, string parentNode, int nodePosition, string nodeToAdd, string nodeToAddValue,  string routeFileFolder, string currentRouteFile)
        {
            var sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, routeFileFolder + @"\" + currentRouteFile);
            var importRouteFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RouteFiles") + @"\" + resultFile;

            RouteFileExtensions.DeleteTestRouteFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RouteFiles"));

            var isChildCollectionNode = parentNode != ParentNode;

            RouteFileExtensions.AddElementsToRouteFile(sourceFile, parentNode, nodePosition, nodeToAdd, nodeToAddValue, importRouteFile);
            ScenarioContext.Current.Add("currentRouteTestFile", importRouteFile);
        }


        private void ProcessImportFile(string resultFile, string parentNode, int nodePosition, string nodeToRemove, string routeFileFolder, string currentRouteFile)
        {
            var sourceFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, routeFileFolder + @"\" + currentRouteFile);
            var importRouteFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RouteFiles") + @"\" + resultFile;

            RouteFileExtensions.DeleteTestRouteFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RouteFiles"));

            var isChildCollectionNode = parentNode != ParentNode;

            RouteFileExtensions.RemoveElementsFromRouteFile(sourceFile, parentNode, nodePosition, nodeToRemove, importRouteFile, isChildCollectionNode);
            ScenarioContext.Current.Add("currentRouteTestFile", importRouteFile);
        }



    }
}
