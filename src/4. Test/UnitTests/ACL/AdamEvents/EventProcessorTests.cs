namespace PH.Well.UnitTests.ACL.AdamEvents
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Moq;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using PH.Well.Adam.Events;
    using PH.Well.Common;
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

        private Mock<IDeliveryLineActionService> exceptionEventService;

        private Mock<IEventLogger> eventLogger;

        private Mock<ILogger> logger;
        private Mock<IAdamRepository> _adamRepository;
        private Mock<IContainer> container;

        private EventProcessor processor;

        private string username;
      

        [SetUp]
        public void Setup()
        {
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            this.exceptionEventService = new Mock<IDeliveryLineActionService>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.eventLogger = new Mock<IEventLogger>(MockBehavior.Strict);
            _adamRepository = new Mock<IAdamRepository>();

            this.processor = new EventProcessor(exceptionEventRepository.Object,
                exceptionEventService.Object,
                logger.Object,
                eventLogger.Object,
                _adamRepository.Object
                );

            this.username = "Event Processor";
        }

        public class TheProcessMethod : EventProcessorTests
        {
            [Test]
            public void Credit()
            {
                var lineDictionary = new Dictionary<int, string>();
                var line = "jhgkjhgkj";
                lineDictionary.Add(1, line);
                var creditEvent = new CreditTransaction { BranchId = 22, HeaderSql = "20011.110", LineSql = lineDictionary};

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
                    x => x.CreditTransaction(It.IsAny<CreditTransaction>(), exception.Id, It.IsAny<AdamSettings>()));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellTaskRunner,
                            "Processing ADAM tasks...",
                            5655,
                            EventLogEntryType.Information)).Returns(true);

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x => x.CreditTransaction(It.IsAny<CreditTransaction>(), exception.Id, It.IsAny<AdamSettings>()),
                    Times.Once);
            }

            [Test]
            public void Grn()
            {
                var grnEvent = new GrnEvent() { Id = 1, BranchId = 22};

                var json = JsonConvert.SerializeObject(grnEvent);

                var exception = new ExceptionEvent
                {
                    Id = 501,
                    Event = json,
                    ExceptionActionId = (int)EventAction.Grn
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x => x.Grn(It.IsAny<GrnEvent>(), exception.Id, It.IsAny<AdamSettings>()));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellTaskRunner,
                            "Processing ADAM tasks...",
                            5655,
                            EventLogEntryType.Information)).Returns(true);

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x => x.Grn(It.IsAny<GrnEvent>(), exception.Id, It.IsAny<AdamSettings>()),
                    Times.Once);
            }

            [Test]
            public void Pod()
            {
               /* var lineDictionary = new Dictionary<int, string>();
                var line = "poddy pod pod";
                lineDictionary.Add(1, line);
                var podTransaction = new PodTransaction { BranchId = 22, HeaderSql = "20011.110", LineSql = lineDictionary };
                */

                var podEvent = new PodEvent {BranchId = 22, Id = 10};

                // var json = JsonConvert.SerializeObject(podTransaction);

                var json = JsonConvert.SerializeObject(podEvent);

                var exception = new ExceptionEvent
                {
                    Id = 501,
                    Event = json,
                    ExceptionActionId = (int)EventAction.Pod
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x => x.Pod(It.IsAny<PodEvent>(), exception.Id, It.IsAny<AdamSettings>()));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellTaskRunner,
                            "Processing ADAM tasks...",
                            5655,
                            EventLogEntryType.Information)).Returns(true);

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x => x.Pod(It.IsAny<PodEvent>(), exception.Id, It.IsAny<AdamSettings>()),
                    Times.Once);
            }

            [Test]
            public void PodTransaction()
            {
                var lineDictionary = new Dictionary<int, string>();
                var line = "poddy pod pod";
                lineDictionary.Add(1, line);
                var podTransaction = new PodTransaction { BranchId = 22, HeaderSql = "20011.110", LineSql = lineDictionary };

                var json = JsonConvert.SerializeObject(podTransaction);

                var exception = new ExceptionEvent
                {
                    Id = 501,
                    Event = json,
                    ExceptionActionId = (int)EventAction.PodTransaction
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x => x.PodTransaction(It.IsAny<PodTransaction>(), exception.Id, It.IsAny<AdamSettings>()));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.eventLogger.Setup(
                    x =>
                        x.TryWriteToEventLog(
                            EventSource.WellTaskRunner,
                            "Processing ADAM tasks...",
                            5655,
                            EventLogEntryType.Information)).Returns(true);

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                exceptionEventService.Verify(
                    x => x.PodTransaction(It.IsAny<PodTransaction>(), exception.Id, It.IsAny<AdamSettings>()),
                    Times.Once);
            }
        }
    }
}