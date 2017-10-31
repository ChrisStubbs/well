using System;
using PH.Well.Domain.Enums;

namespace PH.Well.UnitTests.ACL.AdamListener
{
    using System.IO;
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
        private Mock<IImportedFileRepository> importedFileRepository;
        private Mock<IAdamFileMonitorServiceConfig> adamFileMonitorServiceConfig;
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
            importedFileRepository = new Mock<IImportedFileRepository>();
            adamFileMonitorServiceConfig = new Mock<IAdamFileMonitorServiceConfig>();

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
                wellCleanUpService.Object,
                importedFileRepository.Object);
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

            var routeFileName = "ROUTE_PLY_170912_0915.xml";
            var routeFileInfo = new FileMonitorService.ImportFileInfo(routeFileName, routeFileName,
                routeFileModifiedTime, routeFileCreateTime);

            var orderFileModifiedTime = new DateTime(2017, 9, 11, 23, 36, 44);
            var orderFileCreateTime = new DateTime(2017, 9, 13, 11, 46, 35);
            var orderFileName = "ORDER_PLY_170912_2335.xml";
            var orderFileInfo = new FileMonitorService.ImportFileInfo(orderFileName, orderFileName,
                orderFileModifiedTime, orderFileCreateTime);

            // For epod files date is taken from file name
            var expectedEpodFileTime = new DateTime(2017, 9, 11, 14, 53, 16);
            var epodFileName = "ePOD__20170911_14531601151733.xml";
            var epodFileInfo = new FileMonitorService.ImportFileInfo(epodFileName, epodFileName,
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

        public class TheProcessMethod : FileMonitorServiceTests
        {
            [Test]
            public void ShouldNotProcessFileIfExists()
            {
                const string fullFileName = "C:\\ePOD__20171031_164300";
                const string fileName = "ePOD__20171031_164300";
                var fileInfo = new FileMonitorService.ImportFileInfo(fullFileName, fileName, DateTime.Now, DateTime.Now);
                importedFileRepository.Setup(x => x.HasFileAlreadyBeenImported(fileName)).Returns(true);
                fileModule.Setup(x => x.MoveFile(fullFileName, $"C:\\temp\\20171031\\Failures"));
                adamFileMonitorServiceConfig.Setup(x => x.ArchiveFolder).Returns("C:\\temp");

                fileMonitorService.Process(fileInfo, adamFileMonitorServiceConfig.Object);

                logger.Verify(x => x.LogDebug($"{fileName} ignored as already in system !"), Times.Once);
                fileModule.Verify(x => x.MoveFile(fullFileName, $"C:\\temp\\20171031\\Failures") , Times.Once);
            }
        }


    }
}