namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Net;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    [TestFixture]
    public class ExceptionEventControllerTests : BaseControllerTests<ExceptionEventController>
    {
        private Mock<ILogger> logger;

        private Mock<IExceptionEventService> exceptionEventService;

        private Mock<IUserThresholdService> userThresholdService;

        private Mock<IExceptionEventRepository> exceptionEventRepository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.exceptionEventService = new Mock<IExceptionEventService>(MockBehavior.Strict);
            this.userThresholdService = new Mock<IUserThresholdService>(MockBehavior.Strict);
            this.exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);

            this.Controller = new ExceptionEventController(
                this.logger.Object, 
                this.exceptionEventService.Object, 
                this.userThresholdService.Object, 
                this.exceptionEventRepository.Object);

            this.SetupController();
        }

        public class TheCreditMethod : ExceptionEventControllerTests
        {
            [Test]
            public void ShouldCreditDelivery()
            {
                var creditEvent = new CreditEvent
                {
                    InvoiceNumber = "12345.221",
                    TotalCreditValueForThreshold = 10M,
                    Action = "Accept",
                    BranchId = 22,
                    Id = 101
                };

                this.userThresholdService.Setup(x => x.CanUserCredit("", creditEvent.TotalCreditValueForThreshold))
                    .Returns(true);

                this.exceptionEventService.Setup(x => x.Credit(creditEvent, It.IsAny<AdamSettings>(), ""))
                    .Returns(AdamResponse.Success);

                this.exceptionEventRepository.Setup(x => x.RemovedPendingCredit(creditEvent.InvoiceNumber));

                var response = this.Controller.Credit(creditEvent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ShouldAssignCreditForAuthorisation()
            {
                var creditEvent = new CreditEvent
                {
                    InvoiceNumber = "12345.221",
                    TotalCreditValueForThreshold = 10M,
                    Action = "Accept",
                    BranchId = 22,
                    Id = 101
                };

                this.userThresholdService.Setup(x => x.CanUserCredit("", creditEvent.TotalCreditValueForThreshold))
                    .Returns(false);

                this.userThresholdService.Setup(x => x.AssignPendingCredit(creditEvent, ""));

                var response = this.Controller.Credit(creditEvent);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }
        }
    }
}