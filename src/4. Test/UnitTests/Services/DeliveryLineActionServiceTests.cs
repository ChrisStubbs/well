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
        private Mock<IEnumerable<IDeliveryLinesAction>> actionHandlers;
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
            this.actionHandlers = new Mock<IEnumerable<IDeliveryLinesAction>>(MockBehavior.Strict);
            this.jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            this.jobDetailDamageRepository = new Mock<IJobDetailDamageRepository>(MockBehavior.Strict);
            this.jobService = new Mock<IJobService>(MockBehavior.Strict);
            this.eventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            logger = new Mock<ILogger>();

            this.service = new DeliveryLineActionService(this.adamRepository.Object,
                this.jobRepository.Object,
                this.eventRepository.Object,
                this.actionHandlers.Object,
                this.jobDetailRepository.Object,
                this.jobDetailDamageRepository.Object,
                this.jobService.Object,
                logger.Object);
        }

        public class CreditTransactionTests : DeliveryLineActionServiceTests
        {
            [Test]
            public void SuccessfulCreditMarksEventAsProcessed()
            {
                var creditTransaction = new CreditTransaction {JobId = 1, BranchId = 2};
                var eventId = 10;
                var settings = new AdamSettings {Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server"};
                this.adamRepository.Setup(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>())).Returns(AdamResponse.Success);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                this.service.CreditTransaction(creditTransaction, eventId, settings);
                this.adamRepository.Verify(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Once);
            }

            [Test]
            public void AdamDownResponseCreditMarksEventAsProcessed()
            {
                var creditTransaction = new CreditTransaction { JobId = 1, BranchId = 2 };
                var eventId = 10;
                var settings = new AdamSettings { Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server" };
                this.adamRepository.Setup(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>())).Returns(AdamResponse.AdamDown);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                this.service.CreditTransaction(creditTransaction, eventId, settings);
                this.adamRepository.Verify(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Once);
            }

            [Test]
            public void AdamUnknownResponseCreditMarksEventAsProcessed()
            {
                var creditTransaction = new CreditTransaction { JobId = 1, BranchId = 2 };
                var eventId = 10;
                var settings = new AdamSettings { Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server" };
                this.adamRepository.Setup(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>())).Returns(AdamResponse.Unknown);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                this.service.CreditTransaction(creditTransaction, eventId, settings);
                this.adamRepository.Verify(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Once);
            }

            [Test]
            public void AdamDownNoChangeResponseCreditDoesNotMarkEventAsProcessed()
            {
                var creditTransaction = new CreditTransaction { JobId = 1, BranchId = 2 };
                var eventId = 10;
                var settings = new AdamSettings { Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server" };
                this.adamRepository.Setup(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>())).Returns(AdamResponse.AdamDownNoChange);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                this.service.CreditTransaction(creditTransaction, eventId, settings);
                this.adamRepository.Verify(x => x.Credit(It.IsAny<CreditTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Never);
            }
        }

        public class PodTests : DeliveryLineActionServiceTests
        {
            [Test]
            public void NoPodCreatedForAdamWhenNoInvoiceNumberOnJob()
            {
                var job = new Job {Id = 1, InvoiceNumber = string.Empty };
                var pod = new PodEvent { Id = 1, BranchId = 2 };
                var eventId = 10;
                var settings = new AdamSettings { Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server" };

                this.jobRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(job);
                this.jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>()));
                this.adamRepository.Setup(x => x.Pod(It.IsAny<PodEvent>(), It.IsAny<AdamSettings>(), It.IsAny<Job>()));
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));
                
                this.service.Pod(pod, eventId, settings);

                this.adamRepository.Verify(x => x.Pod(It.IsAny<PodEvent>(), It.IsAny<AdamSettings>(), It.IsAny<Job>()), Times.Never());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Never);

            }

            [Test]
            public void PodCreatedForAdamWhenInvoiceNumberExistsAndItIsACleanDelivery()
            {
                var job = new Job { Id = 1, InvoiceNumber = "1234567" };
                var pod = new PodEvent { Id = 1, BranchId = 2 };
                var eventId = 10;
                var settings = new AdamSettings { Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server" };

                this.jobRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(job);
                this.jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
                this.jobDetailRepository.Setup(x => x.Update(It.IsAny<JobDetail>()));
                this.jobDetailDamageRepository.Setup(x => x.Update(It.IsAny<JobDetailDamage>()));
                this.jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>())).Returns(job);
                this.adamRepository.Setup(x => x.Pod(It.IsAny<PodEvent>(), It.IsAny<AdamSettings>(), It.IsAny<Job>())).Returns(AdamResponse.Success);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                this.service.Pod(pod, eventId, settings);

                this.adamRepository.Verify(x => x.Pod(It.IsAny<PodEvent>(), It.IsAny<AdamSettings>(), It.IsAny<Job>()), Times.Once());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Once);
                this.jobRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(2));
                this.jobRepository.Verify(x => x.Update(It.IsAny<Job>()), Times.Once);
                this.jobDetailRepository.Verify(x => x.Update(It.IsAny<JobDetail>()), Times.Never);
                this.jobDetailDamageRepository.Verify(x => x.Update(It.IsAny<JobDetailDamage>()), Times.Never());
                this.jobService.Verify(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>()), Times.Once());

            }
        }

        public class PodTransactionTests : DeliveryLineActionServiceTests
        {
            [Test]
            public void SuccessfulPodTransactionCompletesEvent()
            {
                var job = new Job { Id = 1, InvoiceNumber = "1234567" };
                var podTransaction = new PodTransaction {BranchId = 2, JobId = 1};
                var settings = new AdamSettings { Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server" };
                this.jobRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(job);
                this.jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
                this.jobDetailRepository.Setup(x => x.Update(It.IsAny<JobDetail>()));
                this.jobDetailDamageRepository.Setup(x => x.Update(It.IsAny<JobDetailDamage>()));
                this.jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>())).Returns(job);

                this.adamRepository.Setup(x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>())).Returns(AdamResponse.Success);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                service.PodTransaction(podTransaction, 1, settings);

                this.adamRepository.Verify(x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Once);

            }

            [Test]
            public void NoChangeToPodTransactionDoesNotCompletesEvent()
            {
                var job = new Job { Id = 1, InvoiceNumber = "1234567" };
                var podTransaction = new PodTransaction { BranchId = 2, JobId = 1 };
                var settings = new AdamSettings { Password = "pass", Port = 1, Rfs = "rfs", Username = "flp", Server = "Server" };
                this.jobRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(job);
                this.jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
                this.jobDetailRepository.Setup(x => x.Update(It.IsAny<JobDetail>()));
                this.jobDetailDamageRepository.Setup(x => x.Update(It.IsAny<JobDetailDamage>()));
                this.jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<Job>())).Returns(job);

                this.adamRepository.Setup(x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>())).Returns(AdamResponse.AdamDownNoChange);
                this.eventRepository.Setup(x => x.MarkEventAsProcessed(It.IsAny<int>()));

                service.PodTransaction(podTransaction, 1, settings);

                this.adamRepository.Verify(x => x.PodTransaction(It.IsAny<PodTransaction>(), It.IsAny<AdamSettings>()), Times.Once());
                this.eventRepository.Verify(x => x.MarkEventAsProcessed(It.IsAny<int>()), Times.Never);

            }
        }

        }
}

// TODO 
/*
namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Api.Controllers;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class DeliveryLineActionServiceTests
    {
        private Mock<IAdamRepository> adamRepository;
        private Mock<IExceptionEventRepository> exceptionEventRepository;
        private Mock<IJobRepository> jobRepository;
        private Mock<IUserRepository> userRepository;
        private Mock<ICreditTransactionFactory> creditTransactionFactory;
        private Mock<IUserThresholdService> userThresholdService;
        private Mock<IDeliverLineToDeliveryLineCreditMapper> mapper;
        private Mock<IDeliveryLinesAction> creditAction;
        private Mock<IDeliveryLinesAction> closeAction;
        private DeliveryLineActionService service;

        [SetUp]
        public void Setup()
        {
            this.adamRepository = new Mock<IAdamRepository>(MockBehavior.Strict);
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.creditTransactionFactory = new Mock<ICreditTransactionFactory>(MockBehavior.Strict);
            this.userThresholdService = new Mock<IUserThresholdService>(MockBehavior.Strict);
            this.mapper = new Mock<IDeliverLineToDeliveryLineCreditMapper>(MockBehavior.Strict);
            this.creditAction = new Mock<IDeliveryLinesAction>(MockBehavior.Strict);
            this.closeAction = new Mock<IDeliveryLinesAction>(MockBehavior.Strict);

            var actions = new List<IDeliveryLinesAction> { creditAction.Object, closeAction.Object };

            this.service = new DeliveryLineActionService(
                this.adamRepository.Object,
                this.jobRepository.Object,
                this.userRepository.Object,
                this.userThresholdService.Object, 
                this.exceptionEventRepository.Object,
                actions);
        }

        public class ProcessDeliveryActionResultTests : DeliveryLineActionServiceTests
        {
            [Test]
            public void SuccessfulCreditReturnsResultSuccess()
            {
                var creditLines = new List<DeliveryLine>()
                {
                    DeliveryLineFactory.New.With(x => x.ShortsActionId = (int) DeliveryAction.Credit).Build()
                };
                var credits = new List<DeliveryLineCredit>();

                var username = "fiona.pond";
                var creditTransaction = new CreditTransaction();
                var adamSettings = new AdamSettings();
                var job = new Job {Id = 202};
                int branchId = 2;

                var thresholdResponse = new ThresholdResponse();
                thresholdResponse.CanUserCredit = true;

                this.userThresholdService.Setup(x => x.CanUserCredit(username, 1015)).Returns(thresholdResponse);
                this.jobRepository.Setup(x => x.GetById(creditLines[0].JobId)).Returns(job);
                this.creditTransactionFactory.Setup(x => x.Build(credits, username, branchId))
                    .Returns(creditTransaction);
                this.adamRepository.Setup(x => x.Credit(creditTransaction, adamSettings, username))
                    .Returns(AdamResponse.Success);

                this.jobRepository.Setup(x => x.ResolveJobAndJobDetails(job.Id));
                this.userRepository.Setup(x => x.UnAssignJobToUser(job.Id));
                this.exceptionEventRepository.Setup(x => x.RemovedPendingCredit(job.Id));

                this.mapper.Setup(x => x.Map(creditLines)).Returns(credits);

                var response = this.service.ProcessDeliveryActions(creditLines, adamSettings, username, branchId);

                Assert.IsFalse(response.AdamIsDown);

                this.userThresholdService.Verify(x => x.CanUserCredit(username, 1015), Times.Once);
                this.jobRepository.Verify(x => x.GetById(creditLines[0].JobId), Times.Once);
                this.creditTransactionFactory.Verify(x => x.Build(credits, username, branchId), Times.Once);
                this.adamRepository.Verify(x => x.Credit(creditTransaction, adamSettings, username), Times.Once);

                this.jobRepository.Verify(x => x.ResolveJobAndJobDetails(job.Id), Times.Once);
                this.userRepository.Verify(x => x.UnAssignJobToUser(job.Id), Times.Once);
                this.exceptionEventRepository.Verify(x => x.RemovedPendingCredit(job.Id), Times.Once);

                this.mapper.Verify(x => x.Map(creditLines), Times.Once);
            }

            [Test]
            public void CantCreditAsThresholdToHighForUser()
            {
                var creditLines = new List<DeliveryLine>
                {
                    DeliveryLineFactory.New.With(x => x.ShortsActionId = (int) DeliveryAction.Credit).Build()
                };

                var username = "fiona.pond";
                var adamSettings = new AdamSettings();
                int branchId = 2;
                decimal threshold = 1015;

                var thresholdResponse = new ThresholdResponse {CanUserCredit = false};

                this.userThresholdService.Setup(x => x.CanUserCredit(username, threshold)).Returns(thresholdResponse);

                this.userThresholdService.Setup(
                    x => x.AssignPendingCredit(branchId, threshold, creditLines[0].JobId, username));

                var response = this.service.ProcessDeliveryActions(creditLines, adamSettings, username, branchId);

                Assert.IsTrue(response.CreditThresholdLimitReached);

                this.userThresholdService.Verify(x => x.CanUserCredit(username, threshold), Times.Once);

                this.userThresholdService.Verify(
                    x => x.AssignPendingCredit(branchId, threshold, creditLines[0].JobId, username), Times.Once);
            }

            [Test]
            public void NoCreditLinesFound()
            {
                var deliveryLines = new List<DeliveryLine>();
                deliveryLines.Add(
                    DeliveryLineFactory.New.With(x => x.ShortsActionId = (int) DeliveryAction.CreditAndReorder).Build());
                deliveryLines.Add(
                    DeliveryLineFactory.New.With(x => x.ShortsActionId = (int) DeliveryAction.Reject).Build());
                deliveryLines.Add(
                    DeliveryLineFactory.New.With(x => x.ShortsActionId = (int) DeliveryAction.ReplanInRoadnet).Build());

                var username = "fiona.pond";
                var adamSettings = new AdamSettings();
                int branchId = 2;
                decimal threshold = 1015;

                var response = this.service.CreditDeliveryLines(deliveryLines, adamSettings, username, branchId);

                Assert.IsFalse(response.CreditThresholdLimitReached);
                Assert.That(response.AdamResponse, Is.EqualTo(AdamResponse.Success));
            }

            [Test]
            public void AdamNotAvailableCreditReturnsResultAdamDown()
            {
                var creditLines = new List<DeliveryLine>
                {
                    DeliveryLineFactory.New.With(x => x.ShortsActionId = (int) DeliveryAction.Credit).Build()
                };
                var credits = new List<DeliveryLineCredit>();

                var username = "fiona.pond";
                var creditTransaction = new CreditTransaction();
                var adamSettings = new AdamSettings();
                var job = new Job {Id = 202};
                int branchId = 2;

                var thresholdResponse = new ThresholdResponse {CanUserCredit = true};

                this.userThresholdService.Setup(x => x.CanUserCredit(username, 1015)).Returns(thresholdResponse);
                this.jobRepository.Setup(x => x.GetById(creditLines[0].JobId)).Returns(job);
                this.creditTransactionFactory.Setup(x => x.Build(credits, username, branchId))
                    .Returns(creditTransaction);
                this.adamRepository.Setup(x => x.Credit(creditTransaction, adamSettings, username))
                    .Returns(AdamResponse.AdamDown);
                this.exceptionEventRepository.SetupSet(x => x.CurrentUser = username);
                this.jobRepository.Setup(x => x.SetJobToSubmittedStatus(job.Id));
                this.exceptionEventRepository.Setup(x => x.InsertCreditEventTransaction(creditTransaction));
                this.mapper.Setup(x => x.Map(creditLines)).Returns(credits);

                var response = this.service.CreditDeliveryLines(creditLines, adamSettings, username, branchId);

                Assert.That(response.AdamResponse, Is.EqualTo(AdamResponse.AdamDown));

                this.userThresholdService.Verify(x => x.CanUserCredit(username, 1015), Times.Once);
                this.jobRepository.Verify(x => x.GetById(creditLines[0].JobId), Times.Once);
                this.creditTransactionFactory.Verify(x => x.Build(credits, username, branchId), Times.Once);
                this.adamRepository.Verify(x => x.Credit(creditTransaction, adamSettings, username), Times.Once);
                this.jobRepository.Verify(x => x.SetJobToSubmittedStatus(job.Id), Times.Once);
                this.exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction), Times.Once);
                this.mapper.Verify(x => x.Map(creditLines), Times.Once);
            }

            [Test]
            public void SuccessfulGrnReturnsResultSuccess()
            {
                var eventId = 3;
                var adamSettings = new AdamSettings();
                var username = "fiona.pond";
                var grnEvent = new GrnEvent {Id = 1, BranchId = 55};
                this.adamRepository.Setup(x => x.Grn(grnEvent, adamSettings)).Returns(AdamResponse.Success);

                var response = this.service.Grn(grnEvent, adamSettings, username);

                Assert.That(response, Is.EqualTo(AdamResponse.Success));

                this.adamRepository.Verify(x => x.Grn(grnEvent, adamSettings), Times.Once);

            }

            [Test]
            public void SuccessfulPodReturnsResultSuccess()
            {
                var eventId = 3;
                var adamSettings = new AdamSettings();
                var username = "fiona.pond";
                var lines = new Dictionary<int, string>();

                lines.Add(1, "Thing 1");
                lines.Add(2, "Thing 2");

                var podTransaction = new PodTransaction() { BranchId  = 55, HeaderSql = "Header string", LineSql = lines };
                this.adamRepository.Setup(x => x.Pod(podTransaction, adamSettings)).Returns(AdamResponse.Success);

                this.service.Pod(podTransaction, eventId, adamSettings, username);

                this.adamRepository.Verify(x => x.Pod(podTransaction, adamSettings), Times.Once);

            }
        }
    }
}
*/
