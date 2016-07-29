﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.BDD.Steps
{
    using System.IO;
    using Adam;
    using Adam.Contracts;
    using Framework.Context;
    using StructureMap;
    using TechTalk.SpecFlow;
    using Framework.Extensions;
    using NUnit.Framework;

    [Binding]
    public class AdamImportSteps
    {
        private readonly IContainer container;
        private string adamStatusMessage;
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
            var adamImport = new Import();
            
            adamImport.Process(container, ref adamStatusMessage);
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