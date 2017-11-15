using PH.Well.Common.Contracts;

namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services.Contracts;
    using Well.Services.DeliveryActions;

    [TestFixture]
    public class DeliveryLineActionServiceTests
    {
        private Mock<IAdamRepository> adamRepository;
        private Mock<IJobRepository> jobRepository;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IJobDetailDamageRepository> jobDetailDamageRepository;
        private Mock<IJobService> jobService;
        private Mock<IExceptionEventRepository> eventRepository;
        private Mock<ILogger> logger;

        private DeliveryLineActionService service;

        [SetUp]
        public void Setup()
        {
            this.adamRepository = new Mock<IAdamRepository>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            this.jobDetailDamageRepository = new Mock<IJobDetailDamageRepository>(MockBehavior.Strict);
            this.jobService = new Mock<IJobService>(MockBehavior.Strict);
            this.eventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            logger = new Mock<ILogger>();

            this.service = new DeliveryLineActionService(this.adamRepository.Object,
                this.jobRepository.Object,
                this.eventRepository.Object,
                this.jobDetailRepository.Object,
                this.jobDetailDamageRepository.Object,
                this.jobService.Object,
                logger.Object);
        }

        public class CreditTransactionTests : DeliveryLineActionServiceTests
        {
            /// <summary>
            /// Test when event is marked as processed
            /// Should be marked as processed only on success
            /// </summary>
            [Test]
            [TestCase(AdamResponse.AdamDown, false)]
            [TestCase(AdamResponse.Unknown, false)]
            [TestCase(AdamResponse.Success, true)]
            public void HandleAdamResponse(AdamResponse response, bool shouldMarkedAsProcessed)
            {
                var creditTransaction = new CreditTransaction();
                var settings = new AdamSettings();
                this.adamRepository.Setup(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>()))
                    .Returns(response);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                this.service.CreditOrUpliftTransaction(creditTransaction, 0, settings);
                this.adamRepository.Verify(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>()),
                    Times.Once());

                var processedCalled = shouldMarkedAsProcessed ? Times.Once() : Times.Never();
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), processedCalled);
            }
        }

        public class PodTests : DeliveryLineActionServiceTests
        {
            [Test]
            public void NoPodCreatedForAdamWhenNoInvoiceNumberOnJob()
            {
                var job = new Job {Id = 1, InvoiceNumber = string.Empty};
                var pod = new PodEvent {Id = 1, BranchId = 2};
                var eventId = 10;
                var settings = new AdamSettings
                {
                    Password = "pass",
                    Port = 1,
                    Rfs = "rfs",
                    Username = "flp",
                    Server = "Server"
                };

                this.jobRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(job);
                this.jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>()));
                this.adamRepository.Setup(x => x.Pod(It.IsAny<PodEvent>(), It.IsAny<AdamSettings>(), It.IsAny<Job>()));
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                this.service.Pod(pod, eventId, settings);

                this.adamRepository.Verify(x => x.Pod(It.IsAny<PodEvent>(), It.IsAny<AdamSettings>(), It.IsAny<Job>()),
                    Times.Never());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Never);

            }

            public class PodTransactionTests : DeliveryLineActionServiceTests
            {
                [Test]
                public void SuccessfulPodTransactionCompletesEvent()
                {
                    var job = new Job {Id = 1, InvoiceNumber = "1234567"};
                    var podTransaction = new PodTransaction {BranchId = 2, JobId = 1};
                    var settings = new AdamSettings
                    {
                        Password = "pass",
                        Port = 1,
                        Rfs = "rfs",
                        Username = "flp",
                        Server = "Server"
                    };
                    this.jobRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(job);
                    this.jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
                    this.jobDetailRepository.Setup(x => x.Update(It.IsAny<JobDetail>()));
                    this.jobDetailDamageRepository.Setup(x => x.Update(It.IsAny<JobDetailDamage>()));
                    this.jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>())).Returns(job);

                    this.adamRepository
                        .Setup(x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>()))
                        .Returns(AdamResponse.Success);
                    this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                    service.PodTransaction(podTransaction, 1, settings);

                    this.adamRepository.Verify(
                        x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                    this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Once);
                }

                [Test]
                public void NoChangeToPodTransactionDoesNotCompletesEvent()
                {
                    var job = new Job {Id = 1, InvoiceNumber = "1234567"};
                    var podTransaction = new PodTransaction {BranchId = 2, JobId = 1};
                    var settings = new AdamSettings
                    {
                        Password = "pass",
                        Port = 1,
                        Rfs = "rfs",
                        Username = "flp",
                        Server = "Server"
                    };
                    this.jobRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(job);
                    this.jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
                    this.jobDetailRepository.Setup(x => x.Update(It.IsAny<JobDetail>()));
                    this.jobDetailDamageRepository.Setup(x => x.Update(It.IsAny<JobDetailDamage>()));
                    this.jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>())).Returns(job);

                    this.adamRepository
                        .Setup(x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>()))
                        .Returns(AdamResponse.AdamDown);
                    this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                    service.PodTransaction(podTransaction, 1, settings);

                    this.adamRepository.Verify(
                        x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                    this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Never);

                }
            }

        }
    }
}
