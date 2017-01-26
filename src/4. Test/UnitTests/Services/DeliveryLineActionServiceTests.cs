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

            this.service = new DeliveryLineActionService(this.adamRepository.Object, this.exceptionEventRepository.Object,
                this.jobRepository.Object, this.userRepository.Object,this.creditTransactionFactory.Object, this.userThresholdService.Object);

            
        }

        public class ProcessDeliveryActionResultTests : DeliveryLineActionServiceTests
        {
            [Test]
            public void SuccessfulCreditReturnsResultSuccess()
            {
                var creditLines = new List<DeliveryLine> { DeliveryLineFactory.New.With(x => x.ShortsActionId = (int)DeliveryAction.Credit).Build() };

                var username = "fiona.pond";
                var creditTransaction = new CreditTransaction();
                var adamSettings = new AdamSettings();
                var job = new Job { Id = 202 };
                int branchId = 2;

                var thresholdResponse = new ThresholdResponse();
                thresholdResponse.CanUserCredit = true;

                this.userThresholdService.Setup(x => x.CanUserCredit(username, 1015)).Returns(thresholdResponse);
                    //ErrorMessage = String.Empty});
                this.jobRepository.Setup(x => x.GetById(creditLines[0].JobId)).Returns(job);
                this.creditTransactionFactory.Setup(x => x.BuildCreditEventTransaction(creditLines, username))
                    .Returns(creditTransaction);
                this.adamRepository.Setup(x => x.Credit(creditTransaction, adamSettings, username))
                    .Returns(AdamResponse.Success);

                this.jobRepository.Setup(x => x.ResolveJobAndJobDetails(job.Id));
                this.userRepository.Setup(x => x.UnAssignJobToUser(job.Id));
                this.exceptionEventRepository.Setup(x => x.RemovedPendingCredit(job.Id));

                var response = this.service.CreditDeliveryLines(creditLines, adamSettings, username, branchId);

                Assert.That(response.AdamResponse, Is.EqualTo(AdamResponse.Success));

                this.userThresholdService.Verify(x => x.CanUserCredit(username, 1015), Times.Once);
                this.jobRepository.Verify(x => x.GetById(creditLines[0].JobId), Times.Once);
                this.creditTransactionFactory.Verify(x => x.BuildCreditEventTransaction(creditLines, username), Times.Once);
                this.adamRepository.Verify(x => x.Credit(creditTransaction, adamSettings, username), Times.Once);

                this.jobRepository.Verify(x => x.ResolveJobAndJobDetails(job.Id), Times.Once);
                this.userRepository.Verify(x => x.UnAssignJobToUser(job.Id), Times.Once);
                this.exceptionEventRepository.Verify(x => x.RemovedPendingCredit(job.Id), Times.Once);
            }

            [Test]
            public void CantCreditAsThresholdToHighForUser()
            {
                var creditLines = new List<DeliveryLine> { DeliveryLineFactory.New.With(x => x.ShortsActionId = (int)DeliveryAction.Credit).Build() };

                var username = "fiona.pond";
                var adamSettings = new AdamSettings();
                int branchId = 2;
                decimal threshold = 1015;
                
                var thresholdResponse = new ThresholdResponse();
                thresholdResponse.CanUserCredit = false;

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
                deliveryLines.Add(DeliveryLineFactory.New.With(x => x.ShortsActionId = (int)DeliveryAction.CreditAndReorder).Build());
                deliveryLines.Add(DeliveryLineFactory.New.With(x => x.ShortsActionId = (int)DeliveryAction.Reject).Build());
                deliveryLines.Add(DeliveryLineFactory.New.With(x => x.ShortsActionId = (int)DeliveryAction.ReplanInRoadnet).Build());
                
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
                var creditLines = new List<DeliveryLine> { DeliveryLineFactory.New.With(x => x.ShortsActionId = (int)DeliveryAction.Credit).Build() };

                var username = "fiona.pond";
                var creditTransaction = new CreditTransaction();
                var adamSettings = new AdamSettings();
                var job = new Job { Id = 202 };
                int branchId = 2;

                var thresholdResponse = new ThresholdResponse();
                thresholdResponse.CanUserCredit = true;

                this.userThresholdService.Setup(x => x.CanUserCredit(username, 1015)).Returns(thresholdResponse);
                this.jobRepository.Setup(x => x.GetById(creditLines[0].JobId)).Returns(job);
                this.creditTransactionFactory.Setup(x => x.BuildCreditEventTransaction(creditLines, username))
                    .Returns(creditTransaction);
                this.adamRepository.Setup(x => x.Credit(creditTransaction, adamSettings, username))
                    .Returns(AdamResponse.AdamDown);
                this.exceptionEventRepository.SetupSet(x => x.CurrentUser = username);

                this.exceptionEventRepository.Setup(x => x.InsertCreditEventTransaction(creditTransaction));

                var response = this.service.CreditDeliveryLines(creditLines, adamSettings, username, branchId);

                Assert.That(response.AdamResponse, Is.EqualTo(AdamResponse.AdamDown));

                this.userThresholdService.Verify(x => x.CanUserCredit(username, 1015), Times.Once);
                this.jobRepository.Verify(x => x.GetById(creditLines[0].JobId), Times.Once);
                this.creditTransactionFactory.Verify(x => x.BuildCreditEventTransaction(creditLines, username), Times.Once);
                this.adamRepository.Verify(x => x.Credit(creditTransaction, adamSettings, username), Times.Once);

                this.exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction), Times.Once);
            }
        }
    }
}
