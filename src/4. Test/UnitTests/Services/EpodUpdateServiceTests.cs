namespace PH.Well.UnitTests.Services
{
    using System.Diagnostics;
    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.Services.EpodServices;
    using PH.Well.UnitTests.Factories;
    using Well.Common;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services;

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

        private Mock<IAdamImportService> adamImportService;

        private EpodUpdateService service;

        private Mock<IExceptionEventRepository> exceptionEventRepository;

        private Mock<IPodTransactionFactory> podTransactionFactory;
        private Mock<IUserNameProvider> userNameProvider;

        private Mock<IJobStatusService> deliveryStatusService;

        private Mock<IPostImportRepository> postImportRepository;

        private Mock<IJobResolutionStatus> jobResolutionStatus;

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
            this.adamImportService = new Mock<IAdamImportService>(MockBehavior.Strict);
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            this.podTransactionFactory = new Mock<IPodTransactionFactory>(MockBehavior.Strict);
            this.deliveryStatusService = new Mock<IJobStatusService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns(user);
            this.postImportRepository = new Mock<IPostImportRepository>(MockBehavior.Strict);
            this.jobResolutionStatus = new Mock<IJobResolutionStatus>(MockBehavior.Strict);


            this.service = new EpodUpdateService(this.logger.Object,
                this.eventLogger.Object,
                this.routeHeaderRepository.Object,
                this.stopRepository.Object,
                this.jobRepository.Object,
                this.jobDetailRepository.Object,
                this.jobDetailDamageRepository.Object,
                this.exceptionEventRepository.Object,
                this.mapper.Object,
                this.adamImportService.Object,
                this.podTransactionFactory.Object,
                this.deliveryStatusService.Object,
                this.userNameProvider.Object,
                this.postImportRepository.Object,
                this.jobResolutionStatus.Object);
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
            this.adamImportService.Setup(x => x.ImportRouteHeader(routeHeader, route.RouteId));
            this.eventLogger.Setup(x => x.TryWriteToEventLog(It.IsAny<EventSource>(), It.IsAny<string>(), It.IsAny<int>(), EventLogEntryType.Error)).Returns(true);

            const string filename = "epod_file.xml";

            this.postImportRepository.Setup(x => x.PostImportUpdate());
            this.postImportRepository.Setup(x => x.PostTranSendImport());

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            this.adamImportService.Verify(x => x.ImportRouteHeader(routeHeader, route.RouteId), Times.Never);
            var logError = $"RouteDelivery Ignored could not find matching RouteHeader," +
                            $"Branch: {branchId} " +
                            $"RouteNumber: {routeHeader.RouteNumber.Substring(2)} " +
                            $"RouteDate: {routeHeader.RouteDate} " +
                            $"FileName: {filename}";

            this.logger.Verify(x => x.LogDebug(logError), Times.Once);

            this.eventLogger.Verify(x => x.TryWriteToEventLog(EventSource.WellAdamXmlImport, logError, 9682, EventLogEntryType.Error), Times.Once);

            this.postImportRepository.Verify(x => x.PostImportUpdate(), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImport(), Times.Once);
        }

        [Test]
        [TestCase(20)]
        public void ShouldProcessCorrectly(int branchId)
        {
            //ARRANGE
            var route = new RouteDelivery();

            var routeHeader = RouteHeaderFactory.New.Build();

            var existingRouteHeader = RouteHeaderFactory.New.With(x => x.StartDepotCode = branchId.ToString()).Build();

            var stop = StopFactoryDTO.New.Build();

            routeHeader.Stops.Add(stop);

            var existingStop = new Stop();

            route.RouteHeaders.Add(routeHeader);

            var job = JobFactoryDTO.New.Build();

            stop.Jobs.Add(job);

            var existingJob = new Job();

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate)).Returns(existingRouteHeader);

            this.mapper.Setup(x => x.Map(routeHeader, existingRouteHeader));

            this.routeHeaderRepository.Setup(x => x.Update(existingRouteHeader));

            this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount))
                .Returns(existingStop);

            this.mapper.Setup(x => x.Map(stop, existingStop));

            this.stopRepository.Setup(x => x.Update(existingStop));

            this.jobRepository.Setup(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0))
                .Returns(existingJob);

            this.mapper.Setup(x => x.Map(job, existingJob));

            this.jobRepository.Setup(x => x.Update(existingJob));

            // HACK: DIJ TOTAL HACK FOR NOW!!!
            //(i would like to know how long will this for now will become)

            this.deliveryStatusService.Setup(x => x.DetermineStatus(existingJob, branchId)).Returns(existingJob);
            const string filename = "epod_file.xml";

            this.postImportRepository.Setup(x => x.PostImportUpdate());
            this.postImportRepository.Setup(x => x.PostTranSendImport());
            this.jobResolutionStatus.Setup(x => x.StepForward(existingJob)).Returns(ResolutionStatus.DriverCompleted);
            this.jobRepository.Setup(x => x.Save(existingJob));
            this.jobRepository.Setup(x => x.SetJobResolutionStatus(existingJob.Id, It.IsAny<string>()));

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            this.mapper.Verify(x => x.Map(routeHeader, existingRouteHeader), Times.Once);

            this.routeHeaderRepository.Verify(x => x.Update(existingRouteHeader), Times.Once);

            this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount), Times.Once);

            this.mapper.Verify(x => x.Map(stop, existingStop), Times.Once);

            this.stopRepository.Verify(x => x.Update(existingStop), Times.Once);

            this.jobRepository.Verify(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0), Times.Once);

            this.mapper.Verify(x => x.Map(job, existingJob), Times.Once);

            this.jobRepository.Verify(x => x.Update(existingJob), Times.Once);

            this.postImportRepository.Verify(x => x.PostImportUpdate(), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImport(), Times.Once);

            this.jobResolutionStatus.Verify(x => x.StepForward(existingJob), Times.Once);
            this.jobRepository.Verify(x => x.Save(existingJob), Times.Once);
            this.jobRepository.Verify(x => x.SetJobResolutionStatus(existingJob.Id, It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void ShouldProcessPodCorrectly()
        {

            var route = new RouteDelivery();

            var routeHeader = RouteHeaderFactory.New.Build();
            var branchId = 0;
            routeHeader.TryParseBranchIdFromRouteNumber(out branchId);

            var existingRouteHeader = RouteHeaderFactory.New.With(x => x.StartDepotCode = branchId.ToString()).Build();

            var stop = StopFactoryDTO.New.Build();

            routeHeader.Stops.Add(stop);

            var existingStop = new Stop();

            route.RouteHeaders.Add(routeHeader);

            var job = JobFactoryDTO.New.Build();

            stop.Jobs.Add(job);

            var existingJob = new Job { ProofOfDelivery = (int)ProofOfDelivery.CocaCola };

            this.exceptionEventRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>()));

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate)).Returns(existingRouteHeader);

            this.mapper.Setup(x => x.Map(routeHeader, existingRouteHeader));

            this.routeHeaderRepository.Setup(x => x.Update(existingRouteHeader));

            this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount))
                .Returns(existingStop);

            this.mapper.Setup(x => x.Map(stop, existingStop));

            this.stopRepository.Setup(x => x.Update(existingStop));

            this.jobRepository.Setup(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0))
                .Returns(existingJob);

            this.mapper.Setup(x => x.Map(job, existingJob));

            this.jobRepository.Setup(x => x.Update(existingJob));



            // HACK: DIJ TOTAL HACK FOR NOW!!!

            this.deliveryStatusService.Setup(x => x.DetermineStatus(existingJob, branchId)).Returns(existingJob);
            const string filename = "epod_file.xml";

            this.postImportRepository.Setup(x => x.PostImportUpdate());
            this.postImportRepository.Setup(x => x.PostTranSendImport());

            this.jobResolutionStatus.Setup(x => x.StepForward(existingJob)).Returns(ResolutionStatus.DriverCompleted);
            this.jobRepository.Setup(x => x.Save(existingJob));
            this.jobRepository.Setup(x => x.SetJobResolutionStatus(existingJob.Id, It.IsAny<string>()));

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            this.mapper.Verify(x => x.Map(routeHeader, existingRouteHeader), Times.Once);

            this.routeHeaderRepository.Verify(x => x.Update(existingRouteHeader), Times.Once);

            this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount), Times.Once);

            this.mapper.Verify(x => x.Map(stop, existingStop), Times.Once);

            this.stopRepository.Verify(x => x.Update(existingStop), Times.Once);

            this.jobRepository.Verify(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0), Times.Once);

            this.mapper.Verify(x => x.Map(job, existingJob), Times.Once);

            this.jobRepository.Verify(x => x.Update(existingJob), Times.Once);

            this.exceptionEventRepository.Verify(x => x.InsertPodEvent(It.IsAny<PodEvent>()), Times.Once);

            this.postImportRepository.Verify(x => x.PostImportUpdate(),Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImport(), Times.Once);

            this.jobResolutionStatus.Verify(x => x.StepForward(existingJob), Times.Once);
            this.jobRepository.Verify(x => x.Save(existingJob), Times.Once);
            this.jobRepository.Verify(x => x.SetJobResolutionStatus(existingJob.Id, It.IsAny<string>()), Times.Exactly(2));

        }

        [Test]
        public void ShouldNotProcessCompletedOnPaperPod()
        {
            //ARRANGE
            var route = new RouteDelivery();
            var routeHeader = RouteHeaderFactory.New.Build();
            var branchId = 20;
            var existingRouteHeader = RouteHeaderFactory.New.With(x => x.StartDepotCode = branchId.ToString()).Build();
            var stop = StopFactoryDTO.New.Build();
            var job = JobFactoryDTO.New.Build();
            var existingStop = new Stop();

            routeHeader.Stops.Add(stop);
            route.RouteHeaders.Add(routeHeader);
            stop.Jobs.Add(job);

            var existingJob = new Job { ProofOfDelivery = (int)ProofOfDelivery.CocaCola, JobStatus = JobStatus.CompletedOnPaper };

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate)).Returns(existingRouteHeader);

            this.mapper.Setup(x => x.Map(routeHeader, existingRouteHeader));

            this.routeHeaderRepository.Setup(x => x.Update(existingRouteHeader));

            this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount))
                .Returns(existingStop);

            this.mapper.Setup(x => x.Map(stop, existingStop));

            this.stopRepository.Setup(x => x.Update(existingStop));

            this.jobRepository.Setup(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0))
                .Returns(existingJob);

            this.mapper.Setup(x => x.Map(job, existingJob));

            this.jobRepository.Setup(x => x.Update(existingJob));

            // HACK: DIJ TOTAL HACK FOR NOW!!!

            this.deliveryStatusService.Setup(x => x.DetermineStatus(existingJob, branchId)).Returns(existingJob);
            const string filename = "epod_file.xml";

            this.postImportRepository.Setup(x => x.PostImportUpdate());

            this.postImportRepository.Setup(x => x.PostTranSendImport());
            this.jobResolutionStatus.Setup(x => x.StepForward(existingJob)).Returns(ResolutionStatus.DriverCompleted);
            this.jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));
            this.jobRepository.Setup(x => x.Save(existingJob));

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            this.mapper.Verify(x => x.Map(routeHeader, existingRouteHeader), Times.Once);
            this.routeHeaderRepository.Verify(x => x.Update(existingRouteHeader), Times.Once);
            this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount), Times.Once);
            this.mapper.Verify(x => x.Map(stop, existingStop), Times.Once);
            this.stopRepository.Verify(x => x.Update(existingStop), Times.Once);
            this.jobRepository.Verify(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0), Times.Once);
            this.mapper.Verify(x => x.Map(job, existingJob), Times.Once);
            this.jobRepository.Verify(x => x.Update(existingJob), Times.Once);
            this.postImportRepository.Verify(x => x.PostImportUpdate(), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImport(), Times.Once);
            this.jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));
            this.jobResolutionStatus.Verify(x => x.StepForward(existingJob), Times.Once);
            this.jobRepository.Verify(x => x.Save(existingJob), Times.Once);
        }
    }
}