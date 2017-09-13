namespace PH.Well.UnitTests.Services.EpodServices
{
    using System.Diagnostics;
    using Factories;
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
    public class EpodImportServiceTests
    {
        private Mock<ILogger> logger;
        private Mock<IEventLogger> eventLogger;
        private Mock<IRouteHeaderRepository> routeHeaderRepository;
        private Mock<IImportService> importService;
        private Mock<IEpodImportMapper> epodImportMapper;
        private Mock<IEpodFileImportCommands> importCommands;
        private Mock<IDeadlockRetryConfig> deadlockRetryConfig;
        private IDeadlockRetryHelper deadlockRetryHelper;
        private EpodImportService epodImportService;
        private Mock<IRouteService> routeService;

        [SetUp]
        public virtual void SetUp()
        {
            logger = new Mock<ILogger>();
            eventLogger = new Mock<IEventLogger>();
            routeHeaderRepository = new Mock<IRouteHeaderRepository>();
            importService = new Mock<IImportService>();
            epodImportMapper = new Mock<IEpodImportMapper>();
            importCommands = new Mock<IEpodFileImportCommands>();
            deadlockRetryConfig = new Mock<IDeadlockRetryConfig>();
            routeService = new Mock<IRouteService>();

            deadlockRetryHelper = new DeadlockRetryHelper(logger.Object, deadlockRetryConfig.Object);

            epodImportService = new EpodImportService(
                logger.Object,
                eventLogger.Object,
                routeHeaderRepository.Object,
                importService.Object,
                epodImportMapper.Object,
                importCommands.Object,
                deadlockRetryHelper,
                routeService.Object);
        }

        [Test]
        public void ShouldNotUpdateRouteHeaderAndLogIfHeaderDoesNotExist()
        {
            //ARRANGE
            var route = new RouteDelivery();

            var routeHeader = RouteHeaderFactory.New.Build();
            var branchId = 0;
            routeHeader.TryParseBranchIdFromRouteNumber(out branchId);
            route.RouteHeaders.Add(routeHeader);

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(branchId,
                    routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate)).Returns((RouteHeader)null);

            this.logger.Setup(x => x.LogDebug(It.IsAny<string>()));

            this.eventLogger.Setup(x => x.TryWriteToEventLog(It.IsAny<EventSource>(), It.IsAny<string>(), It.IsAny<int>(), EventLogEntryType.Error)).Returns(true);

            const string filename = "epod_file.xml";
            //ACT
            epodImportService.Import(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            var logError = $"RouteDelivery Ignored could not find matching RouteHeader," +
                           $"Branch: {branchId} " +
                           $"RouteNumber: {routeHeader.RouteNumber.Substring(2)} " +
                           $"RouteDate: {routeHeader.RouteDate} " +
                           $"FileName: {filename}";

            this.logger.Verify(x => x.LogDebug(logError), Times.Once);

            this.eventLogger.Verify(x => x.TryWriteToEventLog(EventSource.WellAdamXmlImport, logError, 9682, EventLogEntryType.Error), Times.Once);

        }
    }
}