namespace PH.Well.UnitTests.ACL.AdamListener
{
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class AdamFileMonitorServiceTests
    {

        private Mock<ILogger> logger;
        private Mock<IEventLogger> eventLogger;
        private Mock<IFileService> fileService;
        //private Mock<IFileTypeService> fileTypeService;
        private FileTypeService fileTypeService;
        private Mock<IFileModule> fileModule;
        private Mock<IAdamImportService> adamImportService;
        private Mock<IAdamUpdateService> adamUpdateService;
        private Mock<IRouteHeaderRepository> routeHeaderRepository;
        private AdamFileMonitorService fileMonitorService;
        private Mock<IEpodFileProvider> epodProvider;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger>();
            eventLogger = new Mock<IEventLogger>();
            fileService = new Mock<IFileService>();
            fileTypeService = new FileTypeService();
            fileModule = new Mock<IFileModule>();
            adamImportService = new Mock<IAdamImportService>();
            adamUpdateService = new Mock<IAdamUpdateService>();
            routeHeaderRepository = new Mock<IRouteHeaderRepository>();
            epodProvider = new Mock<IEpodFileProvider>();

            fileMonitorService = new AdamFileMonitorService(
                logger.Object,
                eventLogger.Object,
                fileService.Object,
                fileTypeService,
                fileModule.Object,
                adamImportService.Object,
                adamUpdateService.Object,
                routeHeaderRepository.Object,
                epodProvider.Object);
        }

        public class TheIsRouteOrOrderFileMethod : AdamFileMonitorServiceTests
        {
            [Test]
            [TestCase("ORDER_123", ExpectedResult = true)]
            [TestCase("OrdeR_123", ExpectedResult = true)]
            [TestCase("ROUTE_123", ExpectedResult = true)]
            [TestCase("routE_123", ExpectedResult = true)]
            [TestCase("ePOD_", ExpectedResult = true)]
            [TestCase("Fiona_", ExpectedResult = false)]
            public bool ShouldOnlyReturnTrueIfPrefixedWithOrderOrRouteOrEpod(string fileName)
            {
                return fileMonitorService.IsRouteOrOrderFile(fileName);
            }
        }

        public class GetDateTimeStampFromFileName : AdamFileMonitorServiceTests
        {
            [Test]
            [TestCase("ORDER_PLY_170823_1008C.xml", ExpectedResult = "170823_100800")]
            [TestCase("ROUTE_PLY_170823_1008C.xml", ExpectedResult = "170823_100800")]
            [TestCase("ROUTE_PLY_170823_1008.xml", ExpectedResult = "170823_100800")]
            [TestCase("ORDER_PLY_170823_1008.xml", ExpectedResult = "170823_100800")]
            [TestCase("ePOD__20170823_1008123456.xml", ExpectedResult = "170823_100812")]
            public string ShouldReturnTheDateAndTimeStamp(string fileName)
            {
                return fileMonitorService.GetDateTimeStampFromFileName(fileName);
            }
        }


    }
}