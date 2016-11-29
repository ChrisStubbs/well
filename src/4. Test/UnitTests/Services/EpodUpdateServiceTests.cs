namespace PH.Well.UnitTests.Services
{
    using System.Diagnostics;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.Services.EpodServices;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class EpodUpdateServiceTests
    {
        private Mock<ILogger> logger;

        private Mock<IEventLogger> eventLogger;

        private Mock<IRouteHeaderRepository> routeHeaderRepository;

        private Mock<IStopRepository> stopRepository;

        private Mock<IJobRepository> jobRepository;

        private Mock<IJobDetailRepository> jobDetailRepository;

        private Mock<IJobDetailDamageRepository> jobDetailDamageRepository;

        private Mock<IRouteMapper> mapper;

        private EpodUpdateService service;

        [SetUp]
        public void Setup()
        {
            var user = "EpodUpdate";

            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.eventLogger = new Mock<IEventLogger>(MockBehavior.Strict);
            this.routeHeaderRepository = new Mock<IRouteHeaderRepository>(MockBehavior.Strict);
            this.stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            this.jobDetailDamageRepository = new Mock<IJobDetailDamageRepository>(MockBehavior.Strict);
            this.mapper = new Mock<IRouteMapper>(MockBehavior.Strict);

            this.routeHeaderRepository.SetupSet(x => x.CurrentUser = user);
            this.stopRepository.SetupSet(x => x.CurrentUser = user);
            this.jobRepository.SetupSet(x => x.CurrentUser = user);
            this.jobDetailRepository.SetupSet(x => x.CurrentUser = user);
            this.jobDetailDamageRepository.SetupSet(x => x.CurrentUser = user);

            this.service = new EpodUpdateService(this.logger.Object, this.eventLogger.Object, this.routeHeaderRepository.Object,
                this.stopRepository.Object, this.jobRepository.Object, this.jobDetailRepository.Object, this.jobDetailDamageRepository.Object,
                this.mapper.Object);
        }

        [Test]
        public void ShouldNotProcessWhenNoHeader()
        {
            var route = new RouteDelivery();

            var routeHeader = RouteHeaderFactory.New.Build();

            route.RouteHeaders.Add(routeHeader);

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(routeHeader.RouteNumber, routeHeader.RouteDate)).Returns((RouteHeader)null);

            this.logger.Setup(
                x =>
                    x.LogDebug(
                        $"No data found for Epod route: {routeHeader.RouteNumber} on date: {routeHeader.RouteDate}"));

            this.eventLogger.Setup(
                x =>
                    x.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"No data found for Epod route: {routeHeader.RouteNumber} on date: {routeHeader.RouteDate}",
                        7450,
                        EventLogEntryType.Error)).Returns(true);

            this.service.Update(route);

            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(routeHeader.RouteNumber, routeHeader.RouteDate), Times.Once);

            this.logger.Verify(
                x =>
                    x.LogDebug(
                        $"No data found for Epod route: {routeHeader.RouteNumber} on date: {routeHeader.RouteDate}"), Times.Once);

            this.eventLogger.Verify(
                x =>
                    x.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"No data found for Epod route: {routeHeader.RouteNumber} on date: {routeHeader.RouteDate}",
                        7450,
                        EventLogEntryType.Error), Times.Once);
        }

        [Test]
        public void ShouldProcessCorrectly()
        {
            var route = new RouteDelivery();

            var routeHeader = RouteHeaderFactory.New.Build();

            var existingRouteHeader = RouteHeaderFactory.New.Build();

            var stop = StopFactory.New.Build();

            routeHeader.Stops.Add(stop);

            var existingStop = new Stop();

            route.RouteHeaders.Add(routeHeader);

            var job = JobFactory.New.Build();

            stop.Jobs.Add(job);

            var existingJob = new Job();

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(routeHeader.RouteNumber, routeHeader.RouteDate)).Returns(existingRouteHeader);

            this.mapper.Setup(x => x.Map(routeHeader, existingRouteHeader));

            this.routeHeaderRepository.Setup(x => x.Update(existingRouteHeader));

            this.stopRepository.Setup(x => x.GetByTransportOrderReference(stop.TransportOrderReference))
                .Returns(existingStop);

            this.mapper.Setup(x => x.Map(stop, existingStop));

            this.stopRepository.Setup(x => x.Update(existingStop));

            this.jobRepository.Setup(x => x.GetByAccountPicklistAndStopId(job.PhAccount, job.PickListRef, 0))
                .Returns(existingJob);

            this.mapper.Setup(x => x.Map(job, existingJob));

            this.jobRepository.Setup(x => x.Update(existingJob));

            this.service.Update(route);

            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(routeHeader.RouteNumber, routeHeader.RouteDate), Times.Once);

            this.mapper.Verify(x => x.Map(routeHeader, existingRouteHeader), Times.Once);

            this.routeHeaderRepository.Verify(x => x.Update(existingRouteHeader), Times.Once);

            this.stopRepository.Verify(x => x.GetByTransportOrderReference(stop.TransportOrderReference), Times.Once);

            this.mapper.Verify(x => x.Map(stop, existingStop), Times.Once);

            this.stopRepository.Verify(x => x.Update(existingStop), Times.Once);

            this.jobRepository.Verify(x => x.GetByAccountPicklistAndStopId(job.PhAccount, job.PickListRef, 0), Times.Once);

            this.mapper.Verify(x => x.Map(job, existingJob), Times.Once);

            this.jobRepository.Verify(x => x.Update(existingJob), Times.Once);
        }
    }
}