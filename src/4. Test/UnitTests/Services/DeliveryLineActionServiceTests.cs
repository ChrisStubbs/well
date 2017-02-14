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
        private DeliveryLineActionService service;
        private Mock<IPodTransactionFactory> podTransactionFactory;

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
            this.podTransactionFactory = new Mock<IPodTransactionFactory>(MockBehavior.Loose);

            this.service = new DeliveryLineActionService(
                this.adamRepository.Object, 
                this.exceptionEventRepository.Object,
                this.jobRepository.Object, 
                this.userRepository.Object,
                this.creditTransactionFactory.Object, 
                this.userThresholdService.Object,
                this.mapper.Object,
                this.podTransactionFactory.Object);
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

                var response = this.service.CreditDeliveryLines(creditLines, adamSettings, username, branchId);

                Assert.That(response.AdamResponse, Is.EqualTo(AdamResponse.Success));

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

                var response = this.service.CreditDeliveryLines(creditLines, adamSettings, username, branchId);

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

               // this.exceptionEventRepository.Setup(x => x.CurrentUser, "fiona.pond");
                var podTransaction = new PodTransaction() { BranchId  = 55, HeaderSql = "Header string", LineSql = lines };
                this.adamRepository.Setup(x => x.Pod(podTransaction, adamSettings)).Returns(AdamResponse.Success);

                this.service.Pod(podTransaction, eventId, adamSettings, username);

                this.adamRepository.Verify(x => x.Pod(podTransaction, adamSettings), Times.Once);

            }
        }
    }
}
