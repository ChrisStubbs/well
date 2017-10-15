using System;

namespace PH.Well.UnitTests.ACL.AdamListener
{
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class FileMonitorServiceTests
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
        private FileMonitorService fileMonitorService;
        private Mock<IEpodFileProvider> epodProvider;
        private Mock<IWellCleanUpService> wellCleanUpService;

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
            wellCleanUpService = new Mock<IWellCleanUpService>();

            fileMonitorService = new FileMonitorService(
                logger.Object,
                eventLogger.Object,
                fileService.Object,
                fileTypeService,
                fileModule.Object,
                adamImportService.Object,
                adamUpdateService.Object,
                routeHeaderRepository.Object,
                epodProvider.Object,
                wellCleanUpService.Object);
        }

        public class TheIsRouteOrOrderFileMethod : FileMonitorServiceTests
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
                return fileMonitorService.IsRecognisedFileName(fileName);
            }
        }

        [Test]
        public void GetDateStampFromFileTest()
        {
            var routeFileModifiedTime = new DateTime(2017, 9, 11, 9, 17, 8);
            var routeFileCreateTime = new DateTime(2017, 9, 13, 11, 46, 35);
            var routeFileInfo = new FileMonitorService.ImportFileInfo("ROUTE_PLY_170912_0915.xml",
                routeFileModifiedTime, routeFileCreateTime);

            var orderFileModifiedTime = new DateTime(2017, 9, 11, 23, 36, 44);
            var orderFileCreateTime = new DateTime(2017, 9, 13, 11, 46, 35);
            var orderFileInfo = new FileMonitorService.ImportFileInfo("ORDER_PLY_170912_2335.xml",
                orderFileModifiedTime, orderFileCreateTime);

            // For epod files date is taken from file name
            var expectedEpodFileTime = new DateTime(2017, 9, 11, 14, 53, 16);
            var epodFileInfo = new FileMonitorService.ImportFileInfo("ePOD__20170911_14531601151733.xml",
                DateTime.Now, DateTime.Now);

            var routeFileStamp = fileMonitorService.GetDateStampFromFile(routeFileInfo);
            var orderFileStamp = fileMonitorService.GetDateStampFromFile(orderFileInfo);
            var epodFileStamp = fileMonitorService.GetDateStampFromFile(epodFileInfo);

            // Expect modification time because its less than creation time
            Assert.AreEqual(routeFileModifiedTime, routeFileStamp);
            // Expect modification time because its less than creation time
            Assert.AreEqual(orderFileModifiedTime, orderFileStamp);

            Assert.AreEqual(expectedEpodFileTime, epodFileStamp);
        }


    }
}