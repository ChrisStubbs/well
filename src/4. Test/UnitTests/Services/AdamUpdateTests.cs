namespace PH.Well.UnitTests.Services
{
    using System;
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

        private Mock<IRouteMapper> mapper;

        private AdamUpdateService service;

        private Mock<IUserNameProvider> userNameProvider;

        private Mock<IJobStatusService> jobStatusService;

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
            this.mapper = new Mock<IRouteMapper>(MockBehavior.Strict);
            this.jobStatusService = new Mock<IJobStatusService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns(user);

            this.service = new AdamUpdateService(
                this.logger.Object,
                this.eventLogger.Object,
                this.routeHeaderRepository.Object,
                this.stopRepository.Object,
                this.jobRepository.Object,
                this.jobDetailRepository.Object,
                this.mapper.Object,
                this.jobStatusService.Object);
        }

        public class TheUpdateMethodOrderActionInsert : AdamUpdateTests
        {
            [Test]
            public void ShouldNotProcessIfRouteHeaderDoesNotExists()
            {
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
            
                this.service.Update(routeUpdate);

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
            }

            [Test]
            public void ShouldNotProcessIfStopAlreadyExists()
            {
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

                this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount))
                    .Returns(new Stop());

                this.service.Update(routeUpdate);

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

            }

            [Test]
            public void ShouldInsertCorrectly()
            {
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

                var jobUpdate = new JobUpdate { PhAccount = "12321", PickListRef = "42333", InvoiceNumber = "233232" };

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

                this.stopRepository.Setup(x => x.GetByJobDetails(jobUpdate.PickListRef, jobUpdate.PhAccount))
                    .Returns((Stop)null);

                this.mapper.Setup(x => x.Map(stopUpdate, It.IsAny<Stop>()));

                this.stopRepository.Setup(x => x.Save(It.IsAny<Stop>()));

                var existingJob = new Job();

                this.jobRepository.Setup(x => x.GetJobByRefDetails(jobUpdate.PhAccount, jobUpdate.PickListRef, 0))
                    .Returns(existingJob);

                this.mapper.Setup(x => x.Map(jobUpdate, It.IsAny<Job>()));

                this.jobRepository.Setup(x => x.Save(It.IsAny<Job>()));

                var existingJobDetail = new JobDetail();

                this.jobDetailRepository.Setup(x => x.GetByJobLine(0, jobDetailUpdate.LineNumber))
                    .Returns(existingJobDetail);

                this.mapper.Setup(x => x.Map(jobDetailUpdate, It.IsAny<JobDetail>()));

                this.jobDetailRepository.Setup(x => x.Save(It.IsAny<JobDetail>()));

                this.jobStatusService.Setup(x => x.SetInitialStatus(It.IsAny<Job>()));

                this.service.Update(routeUpdate);

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

                this.jobStatusService.Verify(x => x.SetInitialStatus(It.IsAny<Job>()), Times.Once);
            }
        }

        public class TheUpdateMethodOrderActionUpdate : AdamUpdateTests
        {
            [Test]
            public void ShouldNotProcessIfNoStopExists()
            {
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

                this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount))
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

                this.service.Update(routeUpdate);

                this.stopRepository.Verify(x => x.GetByJobDetails(job.PickListRef, job.PhAccount), Times.Once);

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
            }

            [Test]
            public void ShouldProcessCorrectly()
            {
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

                var job = new JobUpdate { PickListRef = "233333", PhAccount = "33222.222", InvoiceNumber = "343434" };

                stopUpdate.Jobs.Add(job);

                this.jobRepository.Setup(x => x.GetJobByRefDetails(job.PhAccount, job.PickListRef, stop.Id)).Returns((Job)null);

                this.stopRepository.Setup(x => x.GetByJobDetails(job.PickListRef, job.PhAccount)).Returns(stop);

                this.mapper.Setup(x => x.Map(stopUpdate, stop));

                this.stopRepository.Setup(x => x.Update(stop));

                this.service.Update(routeUpdate);
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

                this.stopRepository.Setup(x => x.GetByJobDetails(jobUpdate.PickListRef, jobUpdate.PhAccount)).Returns(stop);

                this.stopRepository.Setup(x => x.DeleteStopByTransportOrderReference(stop.TransportOrderReference));

                this.service.Update(routeUpdate);

                this.stopRepository.Verify(x => x.DeleteStopByTransportOrderReference(stopUpdate.TransportOrderRef), Times.Once);
            }
        }
    }
}
