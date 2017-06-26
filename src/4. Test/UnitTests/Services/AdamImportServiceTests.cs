namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Diagnostics;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.EpodServices;
    using PH.Well.UnitTests.Factories;
    using Well.Services;
    using Well.Services.Contracts;
    using static PH.Well.Domain.Mappers.AutoMapperConfig;

    [TestFixture]
    public class AdamImportServiceTests
    {
        private Mock<IRouteHeaderRepository> routeHeaderRepository;

        private Mock<IStopRepository> stopRepository;

        private Mock<IAccountRepository> accountRepository;

        private Mock<IJobRepository> jobRepository;

        private Mock<IJobDetailRepository> jobDetailRepository;

        private Mock<IJobDetailDamageRepository> jobDetailDamageRepository;

        private Mock<ILogger> logger;

        private Mock<IEventLogger> eventLogger;

        private Mock<IJobService> jobStatusService;

        private AdamImportService service;

        private Mock<IPostImportRepository> postImportRepository;

        [SetUp]
        public void Setup()
        {
            this.routeHeaderRepository = new Mock<IRouteHeaderRepository>(MockBehavior.Strict);
            this.stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);
            this.accountRepository = new Mock<IAccountRepository>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            this.jobDetailDamageRepository = new Mock<IJobDetailDamageRepository>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.eventLogger = new Mock<IEventLogger>(MockBehavior.Strict);
            this.jobStatusService = new Mock<IJobService>(MockBehavior.Strict);
            this.postImportRepository = new Mock<IPostImportRepository>(MockBehavior.Strict);

            this.service = new AdamImportService(this.routeHeaderRepository.Object,
                this.stopRepository.Object,
                this.accountRepository.Object,
                this.jobRepository.Object,
                this.jobDetailRepository.Object,
                this.jobDetailDamageRepository.Object,
                this.jobStatusService.Object,
                this.logger.Object,
                this.eventLogger.Object,
                this.postImportRepository.Object);
        }

        [Test]
        public void ShouldNotImportIfRouteHeaderExists()
        {
            var route = new RouteDelivery();

            var header = new RouteHeader { RouteNumber = "001", RouteDate = DateTime.Now, StartDepotCode = "21" };

            route.RouteHeaders.Add(header);

            this.routeHeaderRepository.Setup(x => x.GetByNumberDateBranch(header.RouteNumber, header.RouteDate.Value, header.StartDepot)).Returns(new RouteHeader());

            this.logger.Setup(
                x =>
                    x.LogDebug(
                        $"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.StartDepot})"));

            this.eventLogger.Setup(
                x =>
                    x.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.StartDepot})",
                        7776, EventLogEntryType.Error)).Returns(true);

            this.postImportRepository.Setup(x => x.PostImportUpdate());

            this.service.Import(route);

            this.routeHeaderRepository.Verify(x => x.GetByNumberDateBranch(header.RouteNumber, header.RouteDate.Value, header.StartDepot), Times.Once);

            this.logger.Verify(
                x =>
                    x.LogDebug(
                        $"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.StartDepot})"), Times.Once);

            this.eventLogger.Verify(
                x =>
                    x.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.StartDepot})",
                        7776, EventLogEntryType.Error), Times.Once);
        }

        [Test]
        public void ShouldImportCorrectly()
        {
            var route = RouteDeliveryFactory.New.Build();
            var routeHeader = RouteHeaderFactory.New.Build();
            var stop = StopFactory.New.Build();
            var job = JobFactory.New.Build();
            var jobDetail = JobDetailFactory.New.Build();

            jobDetail.JobDetailDamages.Add(new JobDetailDamage());

            job.JobDetails.Add(jobDetail);
            stop.Jobs.Add(job);
            routeHeader.Stops.Add(Mapper.Map<Stop, StopDTO>(stop));
            route.RouteHeaders.Add(routeHeader);

            this.routeHeaderRepository.Setup(x => x.GetByNumberDateBranch(routeHeader.RouteNumber, routeHeader.RouteDate.Value, routeHeader.StartDepot)).Returns((RouteHeader)null);

            this.routeHeaderRepository.Setup(x => x.Save(routeHeader));
            this.stopRepository.Setup(x => x.Save(It.IsAny<Stop>()));
            this.accountRepository.Setup(x => x.Save(It.IsAny<Account>()));
            this.jobRepository.Setup(x => x.Save(It.IsAny<Job>()));
            this.jobDetailRepository.Setup(x => x.Save(It.IsAny <JobDetail>()));
            this.jobDetailDamageRepository.Setup(x => x.Save(It.IsAny <JobDetailDamage>()));
            this.jobStatusService.Setup(x => x.SetInitialJobStatus(It.IsAny<Job>()));
            this.postImportRepository.Setup(x => x.PostImportUpdate());
            this.jobRepository.Setup(x => x.SetJobResolutionStatus(job.Id, It.IsAny<string>()));

            this.service.Import(route);

            this.routeHeaderRepository.Verify(x => x.Save(routeHeader), Times.Once);
            this.stopRepository.Verify(x => x.Save(It.IsAny<Stop>()), Times.Once);
            this.accountRepository.Verify(x => x.Save(It.IsAny<Account>()), Times.Once);
            this.jobRepository.Verify(x => x.Save(It.IsAny<Job>()), Times.Once);
            this.jobDetailRepository.Verify(x => x.Save(It.IsAny<JobDetail>()), Times.Once);
            this.jobDetailDamageRepository.Verify(x => x.Save(It.IsAny<JobDetailDamage>()), Times.Once);
            this.jobStatusService.Verify(x => x.SetInitialJobStatus(It.IsAny<Job>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostImportUpdate(), Times.Once);
            this.jobRepository.Verify(x => x.SetJobResolutionStatus(job.Id, It.IsAny<string>()), Times.Once);
        }
    }
}
