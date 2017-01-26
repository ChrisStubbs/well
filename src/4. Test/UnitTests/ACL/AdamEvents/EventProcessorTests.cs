﻿namespace PH.Well.UnitTests.ACL.AdamEvents
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
                var lineDictionary = new Dictionary<int, string>();
                var line = "jhgkjhgkj";
                lineDictionary.Add(1, line);
                var creditEvent = new CreditEventTransaction { BranchId = 22, HeaderSql = "20011.110", LineSql = lineDictionary};

                var json = JsonConvert.SerializeObject(creditEvent);

                var exception = new ExceptionEvent
                {
                    Id = 501,
                    Event = json,
                    ExceptionActionId = (int)EventAction.CreditTransaction
                };

                var events = new List<ExceptionEvent> { exception };

                this.exceptionEventRepository.Setup(x => x.GetAllUnprocessed()).Returns(events);
                this.exceptionEventService.Setup(
                    x => x.CreditEventTransaction(It.IsAny<CreditEventTransaction>(), exception.Id, It.IsAny<AdamSettings>(), this.username));

                this.logger.Setup(x => x.LogDebug("Starting Well Adam Events!"));
                this.logger.Setup(x => x.LogDebug("Finished Well Adam Events!"));

                this.processor.Process();

                this.exceptionEventRepository.Verify(x => x.GetAllUnprocessed(), Times.Once);
                this.exceptionEventService.Verify(
                    x => x.CreditEventTransaction(It.IsAny<CreditEventTransaction>(), exception.Id, It.IsAny<AdamSettings>(), this.username),
                    Times.Once);
            }
        }
    }
}