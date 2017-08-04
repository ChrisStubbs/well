namespace PH.Well.UnitTests.Services
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Services;
    using Well.Services.Contracts;
    using Well.Services.EpodServices;

    [TestFixture]
    public class RouteImportServiceTests
    {
        private Mock<ILogger> logger;
        private Mock<IEventLogger> eventLogger;
        private Mock<IRouteHeaderRepository> routeHeaderRepository;
        private Mock<IImportService> importService;
        private Mock<IPostImportRepository> postImportRepository;
        private Mock<RouteImportService> mockRouteImportService;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger>();
            eventLogger = new Mock<IEventLogger>();
            routeHeaderRepository = new Mock<IRouteHeaderRepository>();
            importService = new Mock<IImportService>();
            postImportRepository = new Mock<IPostImportRepository>();

            mockRouteImportService = new Mock<RouteImportService>(
                logger.Object,
                eventLogger.Object,
                routeHeaderRepository.Object,
                importService.Object,
                postImportRepository.Object
            );
        }

        public class ImportRouteHeader : RouteImportServiceTests
        {
            [Test]
            public void ShouldGetExistingRouteHeaderWithRouteNumberDateAndOwnerId()
            {
                var routeId = 1;
                var routeNn = "001";
                var routeDate = DateTime.Today;
                var routeOwnerId = 5;
                var rh = new RouteHeader { RouteNumber = routeNn, RouteDate = routeDate };

                mockRouteImportService.Setup(x => x.GetRouteOwnerId(rh)).Returns(routeOwnerId);

                mockRouteImportService.Object.ImportRouteHeader(rh, routeId);

                routeHeaderRepository.Verify(x => x.GetByNumberDateBranch(
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<int>()), Times.Once);

                routeHeaderRepository.Verify(x => x.GetByNumberDateBranch(
                    routeNn,
                    routeDate,
                    routeOwnerId), Times.Once);

            }
        }
    }
}