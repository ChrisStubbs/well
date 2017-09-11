namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Moq;

    using NUnit.Framework;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.Services.EpodServices;
    using PH.Well.UnitTests.Factories;
    using Well.Domain.Enums;

    [TestFixture]
    public class AdamUpdateTests
    {
        private Mock<ILogger> logger;

        private Mock<IEventLogger> eventLogger;

        private Mock<IRouteHeaderRepository> routeHeaderRepository;

        private Mock<IStopRepository> stopRepository;

        private Mock<IJobRepository> jobRepository;

        private Mock<IJobDetailRepository> jobDetailRepository;

        private Mock<IOrderImportMapper> mapper;

        private AdamUpdateService service;

        private Mock<IUserNameProvider> userNameProvider;

        private Mock<IJobService> jobStatusService;

        private Mock<IPostImportRepository> postImportRepository;

        [SetUp]
        public void Setup()
        {
            var user = "AdamUpdate";

            this.routeHeaderRepository = new Mock<IRouteHeaderRepository>(MockBehavior.Strict);
            this.stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.eventLogger = new Mock<IEventLogger>(MockBehavior.Strict);
            this.mapper = new Mock<IOrderImportMapper>(MockBehavior.Strict);
            this.jobStatusService = new Mock<IJobService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns(user);
            this.postImportRepository = new Mock<IPostImportRepository>(MockBehavior.Strict);


            this.service = new AdamUpdateService(
                this.logger.Object,
                this.eventLogger.Object,
                this.routeHeaderRepository.Object,
                this.stopRepository.Object,
                this.jobRepository.Object,
                this.jobDetailRepository.Object,
                this.mapper.Object,
                this.jobStatusService.Object,
                this.postImportRepository.Object);
        }

        public class TheUpdateMethodOrderActionInsert : AdamUpdateTests
        {
            [Test]
            public void ShouldNotProcessIfRouteHeaderDoesNotExists()
            {
                //ARRANGE
                var stopUpdate = new StopUpdate
                {
                    RouteNumberAndDropNumber = "001 02",
                    DeliveryDate = DateTime.Now,
                    StartDepotCode = "22",
                    ActionIndicator = "I"
                };

                var routeUpdate = new RouteUpdates();

                routeUpdate.Stops.Add(stopUpdate);

                this.routeHeaderRepository.Setup(
                    x =>
                        x.GetByNumberDateBranch(
                            stopUpdate.RouteNumber,
                            stopUpdate.DeliveryDate.Value,
                            int.Parse(stopUpdate.StartDepotCode))).Returns((RouteHeader)null);

                this.logger.Setup(
                    x =>
                        x.LogDebug(
                            $"Existing route header not found for route number ({stopUpdate.RouteNumber}), delivery date ({stopUpdate.DeliveryDate})!"));

                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Existing route header not found for route number ({stopUpdate.RouteNumber}), delivery date ({stopUpdate.DeliveryDate})!",
                            3215,
                            EventLogEntryType.Error)).Returns(true);


                //postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));

                //ACT
                this.service.Update(routeUpdate);

                //ASSERT
                this.routeHeaderRepository.Verify(
                    x =>
                        x.GetByNumberDateBranch(
                            stopUpdate.RouteNumber,
                            stopUpdate.DeliveryDate.Value,
                            int.Parse(stopUpdate.StartDepotCode)), Times.Once);

                this.logger.Verify(
                    x =>
                        x.LogDebug(
                            $"Existing route header not found for route number ({stopUpdate.RouteNumber}), delivery date ({stopUpdate.DeliveryDate})!"), Times.Once);

                this.eventLogger.Verify(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Existing route header not found for route number ({stopUpdate.RouteNumber}), delivery date ({stopUpdate.DeliveryDate})!",
                            3215,
                            EventLogEntryType.Error), Times.Once);

                //postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()), Times.Never);
            }

            [Test]
            public void ShouldNotProcessIfStopAlreadyExists()
            {
                //ARRANGE
                var stopUpdate = new StopUpdate
                {
                    RouteNumberAndDropNumber = "001 02",
                    DeliveryDate = DateTime.Now,
                    StartDepotCode = "22",
                    ActionIndicator = "I",
                    TransportOrderRef = "BIR-000001"
                };

                var job = new JobUpdate { PickListRef = "12221", PhAccount = "55444.333", InvoiceNumber = "54444444" };

                stopUpdate.Jobs.Add(job);

                var routeUpdate = new RouteUpdates();

                this.logger.Setup(
                    x =>
                        x.LogDebug(
                            $"Stop already exists for ({stopUpdate.TransportOrderRef}) when doing adam insert to existing route header!"));

                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Stop already exists for ({stopUpdate.TransportOrderRef}) when doing adam insert to existing route header!",
                            3232,
                            EventLogEntryType.Error)).Returns(true);

                routeUpdate.Stops.Add(stopUpdate);

                var routeHeader = RouteHeaderFactory.New.Build();

                this.routeHeaderRepository.Setup(
                    x =>
                        x.GetByNumberDateBranch(
                            stopUpdate.RouteNumber,
                            stopUpdate.DeliveryDate.Value,
                            int.Parse(stopUpdate.StartDepotCode))).Returns(routeHeader);

                this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, int.Parse(stopUpdate.StartDepotCode)))
                    .Returns(new Stop());


                //postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));

                //ACT
                this.service.Update(routeUpdate);

                //ASSERT
                this.routeHeaderRepository.Verify(
                    x =>
                        x.GetByNumberDateBranch(
                            stopUpdate.RouteNumber,
                            stopUpdate.DeliveryDate.Value,
                            int.Parse(stopUpdate.StartDepotCode)), Times.Once);

                this.logger.Verify(
                    x =>
                        x.LogDebug(
                            $"Stop already exists for ({stopUpdate.TransportOrderRef}) when doing adam insert to existing route header!"), Times.Once);

                this.eventLogger.Verify(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Stop already exists for ({stopUpdate.TransportOrderRef}) when doing adam insert to existing route header!",
                            3232,
                            EventLogEntryType.Error), Times.Once);


                postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()), Times.Never);
            }

            [Test]
            public void ShouldInsertCorrectly()
            {

                //ARRANGE
                var routeUpdate = new RouteUpdates();

                var stopUpdate = new StopUpdate
                {
                    RouteNumberAndDropNumber = "001 02",
                    DeliveryDate = DateTime.Now,
                    StartDepotCode = "22",
                    ActionIndicator = "I",
                    TransportOrderRef = "BIR-000001"
                };

                routeUpdate.Stops.Add(stopUpdate);

                var jobUpdate = new JobUpdate { JobTypeCode = "DEL-TOB", PhAccount = "12321", PickListRef = "42333", InvoiceNumber = "233232" };

                var jobDetailUpdate = new JobDetailUpdate { LineNumber = 1 };

                jobUpdate.JobDetails.Add(jobDetailUpdate);

                stopUpdate.Jobs.Add(jobUpdate);

                var routeHeader = RouteHeaderFactory.New.Build();

                this.routeHeaderRepository.Setup(
                    x =>
                        x.GetByNumberDateBranch(
                            stopUpdate.RouteNumber,
                            stopUpdate.DeliveryDate.Value,
                            int.Parse(stopUpdate.StartDepotCode))).Returns(routeHeader);

                this.stopRepository.Setup(x => x.GetByJobDetails(jobUpdate.PickListRef, jobUpdate.PhAccount, routeHeader.RouteOwnerId))
                    .Returns((Stop)null);

                this.mapper.Setup(x => x.Map(stopUpdate, It.IsAny<Stop>()));

                this.stopRepository.Setup(x => x.Save(It.IsAny<Stop>()));

                var existingJob = new Job();

                this.jobRepository.Setup(x => x.GetJobByRefDetails(jobUpdate.JobTypeCode, jobUpdate.PhAccount, jobUpdate.PickListRef, 0))
                    .Returns(existingJob);

                this.mapper.Setup(x => x.Map(jobUpdate, It.IsAny<Job>()));

                this.jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));

                this.jobRepository.Setup(x => x.Save(It.IsAny<Job>()));

                var existingJobDetail = new JobDetail();

                this.jobDetailRepository.Setup(x => x.GetByJobLine(0, jobDetailUpdate.LineNumber))
                    .Returns(existingJobDetail);

                this.mapper.Setup(x => x.Map(jobDetailUpdate, It.IsAny<JobDetail>()));

                this.jobDetailRepository.Setup(x => x.Save(It.IsAny<JobDetail>()));

                this.jobStatusService.Setup(x => x.SetInitialJobStatus(It.IsAny<Job>()));

                this.postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));

                //ACT
                this.service.Update(routeUpdate);


                //Assert
                this.routeHeaderRepository.Verify(
                    x =>
                        x.GetByNumberDateBranch(
                            stopUpdate.RouteNumber,
                            stopUpdate.DeliveryDate.Value,
                            int.Parse(stopUpdate.StartDepotCode)), Times.Once);

                this.mapper.Verify(x => x.Map(stopUpdate, It.IsAny<Stop>()), Times.Once);

                this.stopRepository.Verify(x => x.Save(It.IsAny<Stop>()), Times.Once);

                this.mapper.Verify(x => x.Map(jobUpdate, It.IsAny<Job>()), Times.Once);

                this.jobRepository.Verify(x => x.Save(It.IsAny<Job>()), Times.Once);

                this.mapper.Verify(x => x.Map(jobDetailUpdate, It.IsAny<JobDetail>()), Times.Once);

                this.jobDetailRepository.Verify(x => x.Save(It.IsAny<JobDetail>()), Times.Once);

                this.jobStatusService.Verify(x => x.SetInitialJobStatus(It.IsAny<Job>()), Times.Once);

                this.jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Once);

                this.postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()), Times.Once);
            }
        }

        public class TheUpdateMethodOrderActionUpdate : AdamUpdateTests
        {
            [Test]
            public void ShouldNotProcessIfNoStopExists()
            {
                //ARRANGE
                var routeUpdate = new RouteUpdates();

                var stopUpdate = new StopUpdate
                {
                    RouteNumberAndDropNumber = "001 02",
                    DeliveryDate = DateTime.Now,
                    StartDepotCode = "22",
                    ActionIndicator = "U",
                    TransportOrderRef = "BIR-000001"
                };

                routeUpdate.Stops.Add(stopUpdate);

                var job = new JobUpdate { PickListRef = "32222", PhAccount = "344333.222" };

                stopUpdate.Jobs.Add(job);

                this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, int.Parse(stopUpdate.StartDepotCode)))
                    .Returns((Stop)null);

                this.logger.Setup(
                    x =>
                        x.LogDebug(
                            $"Existing stop not found for picklist ({job.PickListRef}), account ({job.PhAccount})"));

                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Existing stop not found for picklist ({job.PickListRef}), account ({job.PhAccount})",
                            7222, EventLogEntryType.Error)).Returns(true);

                postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));

                //ACT
                this.service.Update(routeUpdate);

                //ASSERT
                this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, int.Parse(stopUpdate.StartDepotCode)), Times.Once);

                this.logger.Verify(
                    x =>
                        x.LogDebug(
                            $"Existing stop not found for picklist ({job.PickListRef}), account ({job.PhAccount})"), Times.Once);

                this.eventLogger.Verify(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Existing stop not found for picklist ({job.PickListRef}), account ({job.PhAccount})",
                            7222, EventLogEntryType.Error), Times.Once);

                postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()), Times.Never);
            }

            [Test]
            public void ShouldProcessCorrectly()
            {
                //ARRANGE
                var routeUpdate = new RouteUpdates();

                var stopUpdate = new StopUpdate
                {
                    RouteNumberAndDropNumber = "001 02",
                    DeliveryDate = DateTime.Now,
                    StartDepotCode = "22",
                    ActionIndicator = "U",
                    TransportOrderRef = "BIR-000001"
                };

                routeUpdate.Stops.Add(stopUpdate);

                var stop = StopFactory.New.Build();

                var job = new JobUpdate { JobTypeCode = "DEL-FRZ", PickListRef = "233333", PhAccount = "33222.222", InvoiceNumber = "343434" };

                stopUpdate.Jobs.Add(job);

                this.jobRepository.Setup(x => x.GetJobByRefDetails(job.JobTypeCode, job.PhAccount, job.PickListRef, stop.Id)).Returns((Job)null);

                this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount, int.Parse(stopUpdate.StartDepotCode))).Returns(stop);

                this.mapper.Setup(x => x.Map(stopUpdate, stop));

                this.stopRepository.Setup(x => x.Update(stop));

                this.mapper.Setup(x => x.Map(It.IsAny<JobUpdate>(), It.IsAny<Job>()));

                this.jobStatusService.Setup(x => x.SetIncompleteJobStatus(It.IsAny<Job>()));

                this. jobRepository.Setup(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()));

                this.jobRepository.Setup(x => x.Save(It.IsAny<Job>()));

                this.postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));

                //ACT
                this.service.Update(routeUpdate);

                //ASSERT
                this.jobRepository.Verify(x => x.SetJobResolutionStatus(It.IsAny<int>(), It.IsAny<string>()), Times.Once);

                postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()),Times.Once);
            }
        }

        public class TheUpdateMethodOrderActionDelete : AdamUpdateTests
        {
            [Test]
            public void ShouldDeleteTheRoute()
            {
                var routeUpdate = new RouteUpdates();

                var stopUpdate = new StopUpdate
                {
                    RouteNumberAndDropNumber = "001 02",
                    DeliveryDate = DateTime.Now,
                    StartDepotCode = "22",
                    ActionIndicator = "D",
                    TransportOrderRef = "BIR-000001"
                };

                var stop = new Stop() { TransportOrderReference = "BIR-000001" };

                var jobUpdate = new JobUpdate() { PickListRef = "322222", PhAccount = "4332.222" };

                stopUpdate.Jobs.Add(jobUpdate);

                routeUpdate.Stops.Add(stopUpdate);

                stopRepository.Setup(x => x.GetByJobDetails(jobUpdate.PickListRef, jobUpdate.PhAccount, int.Parse(stopUpdate.StartDepotCode))).Returns(stop);

                stopRepository.Setup(x => x.DeleteStopByTransportOrderReference(stop.TransportOrderReference));

                postImportRepository.Setup(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()));

                service.Update(routeUpdate);

                stopRepository.Verify(x => x.DeleteStopByTransportOrderReference(stopUpdate.TransportOrderRef), Times.Once);

                postImportRepository.Verify(x => x.PostImportUpdate(It.IsAny<IEnumerable<int>>()), Times.Never);
            }
        }
    }
}
