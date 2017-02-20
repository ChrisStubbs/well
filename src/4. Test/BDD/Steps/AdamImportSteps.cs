namespace PH.Well.BDD.Steps
{
    using System.IO;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using Framework.Context;

    using PH.Well.BDD.Framework;
    using PH.Well.Common.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    using StructureMap;
    using TechTalk.SpecFlow;
    using NUnit.Framework;

    using PH.Well.BDD.XmlFileHelper;
    using PH.Well.Domain;

    using Repositories.Contracts;

    [Binding]
    public class AdamImportSteps
    {
        private readonly IContainer container;
        const string currentAdamRouteFile = "ROUTES_30062016_02.xml";
        const string currentEpodRouteFile = "ePOD__20160701_10452212189454.xml";
        private const string ParentNode = "RouteHeader";
        private ILogger logger;
        private IEventLogger eventLogger;
        private IFileService fileService;
        private IFileModule fileModule;
        private IAdamImportService adamImportService;
        private IAdamUpdateService adamUpdateService;
        private IFileTypeService fileTypeService;
        private IRouteHeaderRepository routeHeaderRepository;
        private IEpodUpdateService epodUpdateService;
        private AdamFileMonitorService adamFileMonitorService;
        private readonly string currentUser = "epodBDDUser";

        public AdamImportSteps()
        {
            this.container = FeatureContextWrapper.GetContextObject<IContainer>(ContextDescriptors.StructureMapContainer);
            this.FileMonitorSetup();
        }

        private void FileMonitorSetup()
        {
            this.logger = this.container.GetInstance<ILogger>();
            this.fileService = this.container.GetInstance<IFileService>();

            this.eventLogger = this.container.GetInstance<IEventLogger>();
            this.fileModule = this.container.GetInstance<IFileModule>();
            this.fileTypeService = this.container.GetInstance<IFileTypeService>();
            this.adamImportService = this.container.GetInstance<IAdamImportService>();
            this.adamUpdateService = this.container.GetInstance<IAdamUpdateService>();
            this.epodUpdateService = this.container.GetInstance<IEpodUpdateService>();
            this.routeHeaderRepository = this.container.GetInstance<IRouteHeaderRepository>();
            

            this.routeHeaderRepository.CurrentUser = this.currentUser;

            this.logger.LogDebug("Calling file monitor service");
            adamFileMonitorService = new AdamFileMonitorService(logger, this.eventLogger, fileService, this.fileTypeService, this.fileModule, this.adamImportService, this.adamUpdateService, this.routeHeaderRepository);
        }

        [Given(@"I have loaded the Adam route data")]
        public void LoadAdamRouteData()
        {
            var importFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "xml\\ROUTE_30062016_02.xml"));

            this.adamFileMonitorService.Process(importFilePath);
        }
        [Given(@"I have loaded the MultiDate Adam route data")]
        public void LoadAdamRouteDataMultiDate()
        {
            var importFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "xml\\ROUTE_30062016_04.xml"));

            this.adamFileMonitorService.Process(importFilePath);
        }

        [Given(@"I have loaded the Adam route data to check data to ADAM")]
        public void LoadAdamRouteDataToCheck()
        {
            var importFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "xml\\ROUTE_30062016_82.xml"));

            this.adamFileMonitorService.Process(importFilePath);
        }

        [Given(@"I have loaded the Adam order data to check data to ADAM")]
        public void LoadAdamOrderDataToCheck()
        {
            var importFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "xml\\ORDER_HAY_170207_2333.xml"));

            this.adamFileMonitorService.Process(importFilePath);
        }

        [Given(@"I have loaded the Adam route data that has 21 lines")]
        public void GivenIHaveLoadedTheAdamRouteDataThatHasLines()
        {
            var importFilePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
               "xml\\ROUTE_30062016_03.xml"));

            this.adamFileMonitorService.Process(importFilePath);
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
            var adamContainer = container.GetInstance<IAdamFileMonitorService>();
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RouteFiles");

            adamFileMonitorService.Process(Path.Combine(filePath, routeFile));
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

        [Given(@"I have imported a valid Epod update file named '(.*)' with (.*) clean and (.*) exceptions")]
        public void GivenIHaveImportedAValidEpodUpdateFileNamedWithCleanAndExceptions(string epodFile, int cleanLines, int exceptionLines)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Epod");

            adamFileMonitorService.Process(Path.Combine(filePath, epodFile));

            ScenarioContext.Current.Add("cleanLines", cleanLines);
            ScenarioContext.Current.Add("exceptionLines", exceptionLines);
        }

        [Given(@"I have a database with Adam/Epod data")]
        public void GivenIHaveADatabaseWithAdamEpodData()
        {
            Hooks.SetupDatabase();
            LoadAdamRouteData();

        }

        [Given(@"I have imported a valid Epod update file named '(.*)'")]
        public void GivenIHaveImportedAValidEpodUpdateFile(string epodFile)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Epod");

            var xmlSerializer = new XmlSerializer(typeof(RouteDelivery));

            using (var reader = new StreamReader(Path.Combine(filePath, epodFile)))
            {
                var routes = (RouteDelivery)xmlSerializer.Deserialize(reader);
                this.epodUpdateService.Update(routes);
            }
        }

        [Given(@"I have an exception royalty of (.*) days for royalty (.*)")]
        public void GivenIHaveAnExceptionRoyaltyOfDaysForRoyalty(int exceptionDays, int royaltyCode)
        {
            var jobRepository = container.GetInstance<IJobRepository>();
            var customerRoyalty = jobRepository.GetCustomerRoyaltyExceptionsByRoyalty(royaltyCode);
            customerRoyalty.ExceptionDays = exceptionDays;
            jobRepository.CurrentUser = this.currentUser;
            jobRepository.UpdateCustomerRoyaltyException(customerRoyalty);
        }

        [Then(@"there should be (.*) exception lines left for a Job with an Id of (.*)")]
        public void ThenThereShouldBeExceptionLinesLeftForAJobWithAndIdOr(int exceptionLines, int jobId)
        {
            var jobDetailrepositoryContainer = container.GetInstance<IJobDetailRepository>();
            var jobDetailCount = jobDetailrepositoryContainer.GetByJobId(jobId).Count(x => !x.IsDeleted);
            Assert.AreEqual(exceptionLines, jobDetailCount);
        }

        [Then(@"there should be (.*) lines left for a Job with an Id of (.*)")]
        public void ThenThereShouldBeLinesLeftForAJobWithAnIdOf(int exceptionLines, int jobId)
        {
            var jobDetailrepositoryContainer = container.GetInstance<IJobDetailRepository>();
            var jobDetailCount = jobDetailrepositoryContainer.GetByJobId(jobId).Count();
            Assert.AreEqual(exceptionLines, jobDetailCount);
        }

        [Given(@"I resolve one of the exceptions with a JobId of (.*)")]
        public void GivenIResolveOneOfTheExceptionsWithAJobIdOf(int jobId)
        {
            var jobDetailrepositoryContainer = container.GetInstance<IJobDetailRepository>();
            var jobDetailToResolve = jobDetailrepositoryContainer.GetByJobId(jobId).FirstOrDefault(x => x.JobDetailStatusId == 2);
            jobDetailToResolve.JobDetailStatusId = 1;
            jobDetailrepositoryContainer.CurrentUser = currentUser;
            jobDetailrepositoryContainer.Update(jobDetailToResolve);
        }

        [Given(@"I resolve all of the exceptions with a JobId of (.*)")]
        public void GivenIResolveAllOfTheExceptionsWithAJobIdOf(int jobId)
        {
            var jobDetailrepositoryContainer = container.GetInstance<IJobDetailRepository>();
            var jobDetailToResolve = jobDetailrepositoryContainer.GetByJobId(jobId).Where(x => x.JobDetailStatusId == 2);
            jobDetailrepositoryContainer.CurrentUser = currentUser;

            foreach (var jobDetail in jobDetailToResolve)
            {
                jobDetail.JobDetailStatusId = 1;
                jobDetailrepositoryContainer.Update(jobDetail);
            }        
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
