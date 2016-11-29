﻿namespace PH.Well.UnitTests.Services
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

            this.routeHeaderRepository.SetupSet(x => x.CurrentUser = user);
            this.stopRepository.SetupSet(x => x.CurrentUser = user);
            this.jobRepository.SetupSet(x => x.CurrentUser = user);
            this.jobDetailRepository.SetupSet(x => x.CurrentUser = user);

            this.service = new AdamUpdateService(
                this.logger.Object, this.eventLogger.Object,
                this.routeHeaderRepository.Object, this.stopRepository.Object,
                this.jobRepository.Object, this.jobDetailRepository.Object, this.mapper.Object);
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

                this.stopRepository.Setup(x => x.GetByTransportOrderReference(stopUpdate.TransportOrderRef))
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

                var jobUpdate = new JobUpdate { PhAccount = "12321", PickListRef = "42333" };

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

                this.stopRepository.Setup(x => x.GetByTransportOrderReference(stopUpdate.TransportOrderRef))
                    .Returns((Stop)null);

                this.mapper.Setup(x => x.Map(stopUpdate, It.IsAny<Stop>()));

                this.stopRepository.Setup(x => x.Save(It.IsAny<Stop>()));

                var existingJob = new Job();

                this.jobRepository.Setup(x => x.JobGetByRefDetails(jobUpdate.PhAccount, jobUpdate.PickListRef, 0))
                    .Returns(existingJob);

                this.mapper.Setup(x => x.Map(jobUpdate, It.IsAny<Job>()));

                this.jobRepository.Setup(x => x.Save(It.IsAny<Job>()));

                var existingJobDetail = new JobDetail();

                this.jobDetailRepository.Setup(x => x.GetByJobLine(0, jobDetailUpdate.LineNumber))
                    .Returns(existingJobDetail);

                this.mapper.Setup(x => x.Map(jobDetailUpdate, It.IsAny<JobDetail>()));

                this.jobDetailRepository.Setup(x => x.Save(It.IsAny<JobDetail>()));

                this.service.Update(routeUpdate);

                this.routeHeaderRepository.Verify(
                    x =>
                        x.GetByNumberDateBranch(
                            stopUpdate.RouteNumber,
                            stopUpdate.DeliveryDate.Value,
                            int.Parse(stopUpdate.StartDepotCode)), Times.Once);

                this.stopRepository.Verify(x => x.GetByTransportOrderReference(stopUpdate.TransportOrderRef), Times.Once);

                this.mapper.Verify(x => x.Map(stopUpdate, It.IsAny<Stop>()), Times.Once);

                this.stopRepository.Verify(x => x.Save(It.IsAny<Stop>()), Times.Once);


                this.mapper.Verify(x => x.Map(jobUpdate, It.IsAny<Job>()), Times.Once);

                this.jobRepository.Verify(x => x.Save(It.IsAny<Job>()), Times.Once);

                this.mapper.Verify(x => x.Map(jobDetailUpdate, It.IsAny<JobDetail>()), Times.Once);

                this.jobDetailRepository.Verify(x => x.Save(It.IsAny<JobDetail>()), Times.Once);
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
                    ActionIndicator = "A",
                    TransportOrderRef = "BIR-000001"
                };

                routeUpdate.Stops.Add(stopUpdate);

                this.stopRepository.Setup(x => x.GetByTransportOrderReference(stopUpdate.TransportOrderRef))
                    .Returns((Stop)null);

                this.logger.Setup(
                    x =>
                        x.LogDebug(
                            $"Existing stop not found for transport order reference ({stopUpdate.TransportOrderRef})"));
                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Existing stop not found for transport order reference ({stopUpdate.TransportOrderRef})",
                            7222, EventLogEntryType.Error)).Returns(true);

                this.service.Update(routeUpdate);

                this.stopRepository.Verify(x => x.GetByTransportOrderReference(stopUpdate.TransportOrderRef), Times.Once);

                this.logger.Verify(
                    x =>
                        x.LogDebug(
                            $"Existing stop not found for transport order reference ({stopUpdate.TransportOrderRef})"), Times.Once);

                this.eventLogger.Verify(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellAdamXmlImport,
                            $"Existing stop not found for transport order reference ({stopUpdate.TransportOrderRef})",
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
                    ActionIndicator = "A",
                    TransportOrderRef = "BIR-000001"
                };

                routeUpdate.Stops.Add(stopUpdate);

                var stop = StopFactory.New.Build();

                this.stopRepository.Setup(x => x.GetByTransportOrderReference(stopUpdate.TransportOrderRef)).Returns(stop);

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

                routeUpdate.Stops.Add(stopUpdate);

                this.stopRepository.Setup(x => x.DeleteStopByTransportOrderReference(stopUpdate.TransportOrderRef));

                this.service.Update(routeUpdate);

                this.stopRepository.Verify(x => x.DeleteStopByTransportOrderReference(stopUpdate.TransportOrderRef), Times.Once);
            }
        }
    }
}
