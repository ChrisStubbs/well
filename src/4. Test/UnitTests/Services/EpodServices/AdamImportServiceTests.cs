namespace PH.Well.UnitTests.Services.EpodServices
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
        private Mock<IDbConfiguration> dbConfiguration;

        private Mock<AdamImportService> mockRouteImportService;
        private Mock<IRouteService> routeService;

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
            dbConfiguration = new Mock<IDbConfiguration>();
            routeService = new Mock<IRouteService>();

            deadlockRetryHelper = new DeadlockRetryHelper(logger.Object, deadlockRetryConfig.Object);

            mockRouteImportService = new Mock<AdamImportService>(
                logger.Object,
                eventLogger.Object,
                routeHeaderRepository.Object,
                importService.Object,
                importMapper.Object,
                importCommands.Object,
                deadlockRetryHelper,
                dbConfiguration.Object,
                routeService.Object
            );
        }
    }
}