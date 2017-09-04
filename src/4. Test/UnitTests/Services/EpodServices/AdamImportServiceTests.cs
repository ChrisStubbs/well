﻿namespace PH.Well.UnitTests.Services.EpodServices
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common;
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
        private Mock<IAdamImportMapper> importMapper;
        private Mock<IAdamFileImportCommands> importCommands;
        private Mock<IDeadlockRetryConfig> deadlockRetryConfig;
        private IDeadlockRetryHelper deadlockRetryHelper;

        private Mock<AdamImportService> mockRouteImportService;

        [SetUp]
        public void SetUp()
        {
            logger = new Mock<ILogger>();
            eventLogger = new Mock<IEventLogger>();
            routeHeaderRepository = new Mock<IRouteHeaderRepository>();
            importService = new Mock<IImportService>();
            importMapper = new Mock<IAdamImportMapper>();
            importCommands = new Mock<IAdamFileImportCommands>();
            deadlockRetryConfig = new Mock<IDeadlockRetryConfig>();
            deadlockRetryHelper = new DeadlockRetryHelper(logger.Object,deadlockRetryConfig.Object);


             mockRouteImportService = new Mock<AdamImportService>(
                logger.Object,
                eventLogger.Object,
                routeHeaderRepository.Object,
                importService.Object,
                importMapper.Object,
                importCommands.Object,
                deadlockRetryHelper
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