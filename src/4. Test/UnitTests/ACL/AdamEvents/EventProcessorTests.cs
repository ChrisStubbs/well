namespace PH.Well.UnitTests.ACL.AdamEvents
{
    using System.Collections.Generic;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using PH.Well.Adam.Events;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    using StructureMap;

    [TestFixture]
    public class EventProcessorTests
    {
        private Mock<IExceptionEventRepository> exceptionEventRepository;

        private Mock<IExceptionEventService> exceptionEventService;

        private Mock<ILogger> logger;

        private Mock<IContainer> container;

        private EventProcessor processor;

        private string username;

        [SetUp]
        public void Setup()
        {
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            this.exceptionEventService = new Mock<IExceptionEventService>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.container = new Mock<IContainer>(MockBehavior.Strict);

            this.container.Setup(x => x.GetInstance<IExceptionEventRepository>())
                .Returns(this.exceptionEventRepository.Object);

            this.container.Setup(x => x.GetInstance<IExceptionEventService>())
                .Returns(this.exceptionEventService.Object);

            this.container.Setup(x => x.GetInstance<ILogger>()).Returns(this.logger.Object);

            this.processor = new EventProcessor(this.container.Object);

            this.username = "Event Processor";
        }

        public class TheProcessMethod : EventProcessorTests
        {
            [Test]
            public void Credit()
            {
                var creditEvent = new CreditEvent { BranchId = 22, InvoiceNumber = "20011.110" };

                var json = JsonConvert.SerializeObject(creditEvent);

                var exception = new ExceptionEvent
                {
                    Id = 501,
                    Event = json,
                    ExceptionActionId = (int)EventAction.Credit
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x => x.Credit(It.IsAny<CreditEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x => x.Credit(It.IsAny<CreditEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username),
                    Times.Once);
            }

            [Test]
            public void CreditAndReorder()
            {
                var creditReorderEvent = new CreditReorderEvent { BranchId = 22, InvoiceNumber = "20011.110" };

                var json = JsonConvert.SerializeObject(creditReorderEvent);

                var exception = new ExceptionEvent
                {
                    Event = json,
                    ExceptionActionId = (int)EventAction.CreditAndReorder
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x =>
                        x.CreditReorder(
                            It.IsAny<CreditReorderEvent>(),
                            exception.Id,
                            It.IsAny<AdamSettings>(),
                            this.username));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x =>
                        x.CreditReorder(
                            It.IsAny<CreditReorderEvent>(),
                            exception.Id,
                            It.IsAny<AdamSettings>(),
                            this.username),
                    Times.Once);
            }

            [Test]
            public void Reject()
            {
                var rejectEvent = new RejectEvent { BranchId = 22, InvoiceNumber = "20011.110" };

                var json = JsonConvert.SerializeObject(rejectEvent);

                var exception = new ExceptionEvent { Event = json, ExceptionActionId = (int)EventAction.Reject };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x => x.Reject(It.IsAny<RejectEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x => x.Reject(It.IsAny<RejectEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username),
                    Times.Once);
            }

            [Test]
            public void ReplanRoadnet()
            {
                var roadnetEvent = new RoadnetEvent { BranchId = 22, InvoiceNumber = "20011.110" };

                var json = JsonConvert.SerializeObject(roadnetEvent);

                var exception = new ExceptionEvent
                {
                    Event = json,
                    ExceptionActionId = (int)EventAction.ReplanInRoadnet
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x =>
                        x.ReplanRoadnet(It.IsAny<RoadnetEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x =>
                        x.ReplanRoadnet(It.IsAny<RoadnetEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username),
                    Times.Once);
            }

            [Test]
            public void ReplanTranscend()
            {
                var transcendEvent = new TranscendEvent { BranchId = 22, InvoiceNumber = "20011.110" };

                var json = JsonConvert.SerializeObject(transcendEvent);

                var exception = new ExceptionEvent
                {
                    Event = json,
                    ExceptionActionId = (int)EventAction.ReplanInTranSend
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x =>
                        x.ReplanTranscend(
                            It.IsAny<TranscendEvent>(),
                            exception.Id,
                            It.IsAny<AdamSettings>(),
                            this.username));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x =>
                        x.ReplanTranscend(
                            It.IsAny<TranscendEvent>(),
                            exception.Id,
                            It.IsAny<AdamSettings>(),
                            this.username),
                    Times.Once);
            }

            [Test]
            public void ReplanQueue()
            {
                var queueEvent = new QueueEvent { BranchId = 22, InvoiceNumber = "20011.110" };

                var json = JsonConvert.SerializeObject(queueEvent);

                var exception = new ExceptionEvent
                {
                    Event = json,
                    ExceptionActionId = (int)EventAction.ReplanInTheQueue
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x => x.ReplanQueue(It.IsAny<QueueEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x => x.ReplanQueue(It.IsAny<QueueEvent>(), exception.Id, It.IsAny<AdamSettings>(), this.username),
                    Times.Once);
            }
        }
    }
}