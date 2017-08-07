namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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

        private EpodUpdateService service;

        private Mock<IExceptionEventRepository> exceptionEventRepository;

        private Mock<IJobService> jobService;

        private Mock<IPostImportRepository> postImportRepository;

        private Mock<IGetJobResolutionStatus> getJobResolutionStatus;

        private Mock<IDateThresholdService> dateThresholdService;

        [SetUp]
        public virtual void SetUp()
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
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            this.jobService = new Mock<IJobService>(MockBehavior.Strict);
            this.postImportRepository = new Mock<IPostImportRepository>(MockBehavior.Strict);
            this.getJobResolutionStatus = new Mock<IGetJobResolutionStatus>(MockBehavior.Strict);
            dateThresholdService = new Mock<IDateThresholdService>();

            this.service = new EpodUpdateService(this.logger.Object,
                this.eventLogger.Object,
                this.routeHeaderRepository.Object,
                this.stopRepository.Object,
                this.jobRepository.Object,
                this.jobDetailRepository.Object,
                this.jobDetailDamageRepository.Object,
                this.exceptionEventRepository.Object,
                this.mapper.Object,
                this.jobService.Object,
                this.postImportRepository.Object,
                this.getJobResolutionStatus.Object,
                dateThresholdService.Object
              );


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
            //this.adamImportService.Setup(x => x.ImportRouteHeader(routeHeader, route.RouteId));
            this.eventLogger.Setup(x => x.TryWriteToEventLog(It.IsAny<EventSource>(), It.IsAny<string>(), It.IsAny<int>(), EventLogEntryType.Error)).Returns(true);

            const string filename = "epod_file.xml";

            //this.postImportRepository.Setup(x => x.PostImportUpdate());
            //this.postImportRepository.Setup(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()));
            //this.postImportRepository.Setup(x => x.PostTranSendImport(It.IsAny<List<int>>()));
            //this.postImportRepository.Setup(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()));

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            //this.adamImportService.Verify(x => x.ImportRouteHeader(routeHeader, route.RouteId), Times.Never);
            var logError = $"RouteDelivery Ignored could not find matching RouteHeader," +
                            $"Branch: {branchId} " +
                            $"RouteNumber: {routeHeader.RouteNumber.Substring(2)} " +
                            $"RouteDate: {routeHeader.RouteDate} " +
                            $"FileName: {filename}";

            this.logger.Verify(x => x.LogDebug(logError), Times.Once);

            this.eventLogger.Verify(x => x.TryWriteToEventLog(EventSource.WellAdamXmlImport, logError, 9682, EventLogEntryType.Error), Times.Once);

            //this.postImportRepository.Verify(x => x.PostImportUpdate(), Times.Once);
            //this.postImportRepository.Verify(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()), Times.Never);
            //this.postImportRepository.Verify(x => x.PostTranSendImport(It.IsAny<List<int>>()), Times.Never);
            //this.postImportRepository.Verify(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()), Times.Never);
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

            var updateJob = JobFactory.New.Build();
            var updateJobs = new List<Job> { updateJob };
            var lineItem = new LineItem { Id = 1, JobId = 1 };
            var lineItems = new List<LineItem> { lineItem };
            var jobRoute = new JobRoute { JobId = 1, BranchId = 55, RouteDate = DateTime.Now };
            var jobRoutes = new List<JobRoute> { jobRoute };

            var existingJob = new Job { ResolutionStatus = ResolutionStatus.Imported};

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate)).Returns(existingRouteHeader);

            this.mapper.Setup(x => x.Map(routeHeader, existingRouteHeader));

            this.routeHeaderRepository.Setup(x => x.Update(existingRouteHeader));

            this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, branchId))
                .Returns(existingStop);

            this.mapper.Setup(x => x.Map(stop, existingStop));

            this.stopRepository.Setup(x => x.Update(existingStop));

            this.jobRepository.Setup(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0))
                .Returns(existingJob);

            this.mapper.Setup(x => x.Map(job, existingJob));

            this.jobRepository.Setup(x => x.Update(existingJob));

            // HACK: DIJ TOTAL HACK FOR NOW!!!
            //(i would like to know how long will this for now will become)

            this.jobService.Setup(x => x.DetermineStatus(existingJob, branchId)).Returns(existingJob);
            const string filename = "epod_file.xml";

            this.postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImport(It.IsAny<List<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()));
            this.jobRepository.Setup(x => x.GetJobsWithLineItemActions(It.IsAny<List<int>>())).Returns(It.IsAny<IEnumerable<int>>());
            this.jobRepository.Setup(x => x.GetByIds(It.IsAny<List<int>>())).Returns(updateJobs);
            this.jobService.Setup(x => x.PopulateLineItemsAndRoute(updateJobs)).Returns(updateJobs);
            this.jobRepository.Setup(x => x.GetJobsRoute(It.IsAny<IEnumerable<int>>())).Returns(jobRoutes);

            this.getJobResolutionStatus.Setup(x => x.GetNextResolutionStatus(updateJobs.FirstOrDefault())).Returns(ResolutionStatus.DriverCompleted);
            this.jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));
            this.jobRepository.Setup(x => x.Update(updateJobs.FirstOrDefault()));

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            this.mapper.Verify(x => x.Map(routeHeader, existingRouteHeader), Times.Once);

            this.routeHeaderRepository.Verify(x => x.Update(existingRouteHeader), Times.Once);

            this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, branchId), Times.Once);

            this.mapper.Verify(x => x.Map(stop, existingStop), Times.Once);

            this.stopRepository.Verify(x => x.Update(existingStop), Times.Once);

            this.jobRepository.Verify(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0), Times.Once);

            this.mapper.Verify(x => x.Map(job, existingJob), Times.Once);

            this.jobRepository.Verify(x => x.Update(existingJob), Times.Once);

            this.postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImport(It.IsAny<List<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()), Times.Once);
            this.jobRepository.Verify(x => x.GetJobsWithLineItemActions(It.IsAny<List<int>>()), Times.Once);
            this.jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));
            this.getJobResolutionStatus.Verify(x => x.GetNextResolutionStatus(updateJobs.FirstOrDefault()), Times.Once);
            this.jobRepository.Verify(x => x.Update(updateJobs.FirstOrDefault()), Times.Once);
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
            var updateJob = JobFactory.New.Build();
            var updateJobs = new List<Job> { updateJob };
            var lineItem = new LineItem { Id = 1, JobId = 1 };
            var lineItems = new List<LineItem> { lineItem };
            var jobRoute = new JobRoute { JobId = 1, BranchId = 55, RouteDate = DateTime.Now };
            var jobRoutes = new List<JobRoute> { jobRoute };

            route.RouteHeaders.Add(routeHeader);

            var job = JobFactoryDTO.New.Build();

            stop.Jobs.Add(job);

            var existingJob = new Job { ProofOfDelivery = (int)ProofOfDelivery.CocaCola, ResolutionStatus = ResolutionStatus.Imported};

            this.exceptionEventRepository.Setup(x => x.InsertPodEvent(It.IsAny<PodEvent>()));

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate)).Returns(existingRouteHeader);

            this.mapper.Setup(x => x.Map(routeHeader, existingRouteHeader));

            this.routeHeaderRepository.Setup(x => x.Update(existingRouteHeader));

            this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, branchId))
                .Returns(existingStop);

            this.mapper.Setup(x => x.Map(stop, existingStop));

            this.stopRepository.Setup(x => x.Update(existingStop));

            this.jobRepository.Setup(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0))
                .Returns(existingJob);

            this.mapper.Setup(x => x.Map(job, existingJob));

            this.jobRepository.Setup(x => x.Update(existingJob));



            // HACK: DIJ TOTAL HACK FOR NOW!!!

            this.jobService.Setup(x => x.DetermineStatus(existingJob, branchId)).Returns(existingJob);
            const string filename = "epod_file.xml";

            this.postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImport(It.IsAny<List<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()));

            this.jobRepository.Setup(x => x.GetJobsWithLineItemActions(It.IsAny<IEnumerable<int>>())).Returns(It.IsAny<IEnumerable<int>>());
            this.jobRepository.Setup(x => x.GetByIds(It.IsAny<List<int>>())).Returns(updateJobs);
            this.jobService.Setup(x => x.PopulateLineItemsAndRoute(updateJobs)).Returns(updateJobs);
            this.jobRepository.Setup(x => x.GetJobsRoute(It.IsAny<IEnumerable<int>>())).Returns(jobRoutes);

            this.getJobResolutionStatus.Setup(x => x.GetNextResolutionStatus(updateJobs.FirstOrDefault())).Returns(ResolutionStatus.DriverCompleted);
            this.jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));
            this.jobRepository.Setup(x => x.Update(updateJobs.FirstOrDefault()));

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            this.mapper.Verify(x => x.Map(routeHeader, existingRouteHeader), Times.Once);

            this.routeHeaderRepository.Verify(x => x.Update(existingRouteHeader), Times.Once);

            this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, branchId), Times.Once);

            this.mapper.Verify(x => x.Map(stop, existingStop), Times.Once);

            this.stopRepository.Verify(x => x.Update(existingStop), Times.Once);

            this.jobRepository.Verify(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0), Times.Once);

            this.mapper.Verify(x => x.Map(job, existingJob), Times.Once);

            this.jobRepository.Verify(x => x.Update(existingJob), Times.Once);

            this.exceptionEventRepository.Verify(x => x.InsertPodEvent(It.IsAny<PodEvent>()), Times.Once);

            this.exceptionEventRepository.Verify(x => x.InsertPodEvent(It.IsAny<PodEvent>()), Times.Once);

            this.postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()),Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImport(It.IsAny<List<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()), Times.Once);
            this.jobRepository.Verify(x => x.GetJobsWithLineItemActions(It.IsAny<IEnumerable<int>>()), Times.Once);
            this.jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));
            this.getJobResolutionStatus.Verify(x => x.GetNextResolutionStatus(updateJobs.FirstOrDefault()), Times.Once);
            this.jobRepository.Verify(x => x.Update(updateJobs.FirstOrDefault()), Times.Once);

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
            var updateJob = JobFactory.New.Build();
            var updateJobs = new List<Job> {updateJob};
            var lineItem = new LineItem {Id = 1, JobId = 1};
            var lineItems = new List<LineItem> {lineItem};
            var jobRoute = new JobRoute {JobId = 1, BranchId = 55, RouteDate = DateTime.Now};
            var jobRoutes = new List<JobRoute> {jobRoute};


            routeHeader.Stops.Add(stop);
            route.RouteHeaders.Add(routeHeader);
            stop.Jobs.Add(job);

            var existingJob = new Job { ProofOfDelivery = (int)ProofOfDelivery.CocaCola, JobStatus = JobStatus.CompletedOnPaper , ResolutionStatus = ResolutionStatus.Imported};

            this.routeHeaderRepository.Setup(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate)).Returns(existingRouteHeader);

            this.mapper.Setup(x => x.Map(routeHeader, existingRouteHeader));

            this.routeHeaderRepository.Setup(x => x.Update(existingRouteHeader));

            this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, branchId))
                .Returns(existingStop);

            this.mapper.Setup(x => x.Map(stop, existingStop));

            this.stopRepository.Setup(x => x.Update(existingStop));

            this.jobRepository.Setup(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0))
                .Returns(existingJob);

            this.mapper.Setup(x => x.Map(job, existingJob));

            this.jobRepository.Setup(x => x.Update(existingJob));

            // HACK: DIJ TOTAL HACK FOR NOW!!!

            this.jobService.Setup(x => x.DetermineStatus(existingJob, branchId)).Returns(existingJob);
            const string filename = "epod_file.xml";

            this.postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));

            this.postImportRepository.Setup(x => x.PostTranSendImport(It.IsAny<List<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()));
            this.postImportRepository.Setup(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()));
            this.jobRepository.Setup(x => x.GetJobsWithLineItemActions(It.IsAny<IEnumerable<int>>())).Returns(It.IsAny<IEnumerable<int>>());
            this.jobRepository.Setup(x => x.GetByIds(It.IsAny<List<int>>())).Returns(updateJobs);
           
            this.jobService.Setup(x => x.PopulateLineItemsAndRoute(updateJobs)).Returns(updateJobs);
            this.jobRepository.Setup(x => x.GetJobsRoute(It.IsAny<IEnumerable<int>>())).Returns(jobRoutes);

            this.getJobResolutionStatus.Setup(x => x.GetNextResolutionStatus(updateJobs.FirstOrDefault())).Returns(ResolutionStatus.DriverCompleted);
            this.jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));
            this.jobRepository.Setup(x => x.Update(updateJobs.FirstOrDefault()));

            //ACT
            this.service.Update(route, filename);

            //ASSERT
            this.routeHeaderRepository.Verify(
                x => x.GetRouteHeaderByRoute(branchId, routeHeader.RouteNumber.Substring(2), routeHeader.RouteDate), Times.Once);

            this.mapper.Verify(x => x.Map(routeHeader, existingRouteHeader), Times.Once);
            this.routeHeaderRepository.Verify(x => x.Update(existingRouteHeader), Times.Once);
            this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, branchId), Times.Once);
            this.mapper.Verify(x => x.Map(stop, existingStop), Times.Once);
            this.stopRepository.Verify(x => x.Update(existingStop), Times.Once);
            this.jobRepository.Verify(x => x.GetJobByRefDetails(job.JobTypeCodeTransend, job.PhAccount, job.PickListRef, 0), Times.Once);
            this.mapper.Verify(x => x.Map(job, existingJob), Times.Once);
            this.jobRepository.Verify(x => x.Update(existingJob), Times.Once);
            this.postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImport(It.IsAny<List<int>>()), Times.Once);
            this.postImportRepository.Verify(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()), Times.Once);
            this.jobRepository.Verify(x => x.GetJobsWithLineItemActions(It.IsAny<IEnumerable<int>>()), Times.Once);
            this.jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));
            this.getJobResolutionStatus.Verify(x => x.GetNextResolutionStatus(updateJobs.FirstOrDefault()), Times.Once);
            this.jobRepository.Verify(x => x.Update(updateJobs.FirstOrDefault()), Times.Once);
        }

        public class TheRunPostInvoicedProcessingMethod : EpodUpdateServiceTests
        {
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                postImportRepository.Setup(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()));
                postImportRepository.Setup(x => x.PostTranSendImport(It.IsAny<List<int>>()));
                postImportRepository.Setup(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()));
                jobRepository.Setup(x => x.GetJobsWithLineItemActions(It.IsAny<List<int>>())).Returns(It.IsAny<IEnumerable<int>>());
                jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(It.IsAny<IEnumerable<Job>>()); ;
                jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>())).Returns(new List<Job> { new Job() });
                getJobResolutionStatus.Setup(x => x.GetNextResolutionStatus(It.IsAny<Job>())).Returns(ResolutionStatus.Imported);
                jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
                jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));
            }

            [Test]
            public void ShouldNotRunPostTransendImportIfNoJobIds()
            {
               
                var updatedJobIds = new List<int>();

                service.RunPostInvoicedProcessing(updatedJobIds);

                postImportRepository.Verify(x => x.PostTranSendImportForTobacco(It.IsAny<List<int>>()),Times.Never);
                postImportRepository.Verify(x => x.PostTranSendImport(It.IsAny<List<int>>()),Times.Never);
                postImportRepository.Verify(x => x.PostTranSendImportShortsTba(It.IsAny<List<int>>()), Times.Never);
                jobRepository.Verify(x => x.GetJobsWithLineItemActions(It.IsAny<List<int>>()),Times.Never);
                jobRepository.Verify(x => x.GetByIds(It.IsAny<IEnumerable<int>>()), Times.Never);
                jobService.Verify(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>()), Times.Never);
                getJobResolutionStatus.Verify(x => x.GetNextResolutionStatus(It.IsAny<Job>()), Times.Never);
                jobRepository.Verify(x => x.Update(It.IsAny<Job>()),Times.Never);
                jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()),Times.Never);
            }

            [Test]
            public void ShouldRunPostTransendImportIfMoreThanOneJobId()
            {
                var updatedJobIds = new List<int>{1,2};

                postImportRepository.Setup(x => x.PostTranSendImportForTobacco(updatedJobIds));
                postImportRepository.Setup(x => x.PostTranSendImport(updatedJobIds));
                postImportRepository.Setup(x => x.PostTranSendImportShortsTba(updatedJobIds));

                service.RunPostInvoicedProcessing(updatedJobIds);

                postImportRepository.Verify(x => x.PostTranSendImportForTobacco(updatedJobIds), Times.Once);
                postImportRepository.Verify(x => x.PostTranSendImport(updatedJobIds), Times.Once);
                postImportRepository.Verify(x => x.PostTranSendImportShortsTba(updatedJobIds), Times.Once);
            }


            [Test]
            public void ShouldUpdateAndSaveJobStatusOnceForEachJob()
            {
                var updatedJobIds = new List<int> { 1, 2 };
                var job1 = JobFactory.New.With(x => x.Id = 1).Build();
                var job2 = JobFactory.New.With(x => x.Id = 2).Build();

                jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>())).Returns(new List<Job> { job1, job2 });

                getJobResolutionStatus.Setup(x => x.GetNextResolutionStatus(job1)).Returns(ResolutionStatus.Imported);
                getJobResolutionStatus.Setup(x => x.GetNextResolutionStatus(job2)).Returns(ResolutionStatus.Imported);
                jobRepository.Setup(x => x.Update(job1));
                jobRepository.Setup(x => x.Update(job2));

                jobRepository.Setup(x => x.SetJobResolutionStatus(1, ResolutionStatus.Imported.Description));
                jobRepository.Setup(x => x.SetJobResolutionStatus(2, ResolutionStatus.Imported.Description));

                service.RunPostInvoicedProcessing(updatedJobIds);

                getJobResolutionStatus.Verify(x => x.GetNextResolutionStatus(It.IsAny<Job>()), Times.Exactly(2));
                jobRepository.Verify(x => x.Update(It.IsAny<Job>()), Times.Exactly(2));
                jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Exactly(2));
         
            }
        }
    }
}