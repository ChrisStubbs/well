namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;

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

            this.Controller = new ExceptionEventController(
                this.logger.Object, 
                this.exceptionEventService.Object, 
                this.userThresholdService.Object);

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

        public class TheBulkCreditMethod : ExceptionEventControllerTests
        {
            [Test]
            public void ShouldBulkCredit()
            {
                var creditEvents = new List<CreditEvent> { CreditEventFactory.New.Build() };

                var username = string.Empty;

                this.userThresholdService.Setup(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username))
                    .Returns(new Dictionary<int, string>());

                this.exceptionEventService.Setup(x => x.BulkCredit(creditEvents, username))
                    .Returns(AdamResponse.Success);

                var response = this.Controller.BulkCredit(creditEvents);

                this.userThresholdService.Verify(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username), Times.Once);

                this.exceptionEventService.Verify(x => x.BulkCredit(creditEvents, username), Times.Once);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var result = response.Content.ReadAsStringAsync().Result;

                Assert.That(result, Is.EqualTo("{\"success\":true}"));
            }

            [Test]
            public void ShouldBulkCreditEvenWhenAdamIsDown()
            {
                var creditEvents = new List<CreditEvent> { CreditEventFactory.New.Build() };

                var username = string.Empty;

                this.userThresholdService.Setup(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username))
                    .Returns(new Dictionary<int, string>());

                this.exceptionEventService.Setup(x => x.BulkCredit(creditEvents, username))
                    .Returns(AdamResponse.AdamDown);

                var response = this.Controller.BulkCredit(creditEvents);

                this.userThresholdService.Verify(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username), Times.Once);

                this.exceptionEventService.Verify(x => x.BulkCredit(creditEvents, username), Times.Once);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var result = response.Content.ReadAsStringAsync().Result;

                Assert.That(result, Is.EqualTo("{\"adamdown\":true,\"errors\":{}}"));
            }

            [Test]
            public void ShouldBulkCreditEvenWhenThresholdErrors()
            {
                var creditEvents = new List<CreditEvent> { CreditEventFactory.New.Build() };

                var username = string.Empty;

                var errors = new Dictionary<int, string> { { 1, "Some error" } };
                
                this.userThresholdService.Setup(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username))
                    .Returns(errors);

                this.exceptionEventService.Setup(x => x.BulkCredit(creditEvents, username))
                    .Returns(AdamResponse.Success);

                var response = this.Controller.BulkCredit(creditEvents);

                this.userThresholdService.Verify(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username), Times.Once);

                this.exceptionEventService.Verify(x => x.BulkCredit(creditEvents, username), Times.Once);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var result = response.Content.ReadAsStringAsync().Result;

                Assert.That(result, Is.EqualTo("{\"errors\":{\"1\":\"Some error\"}}"));
            }

            [Test]
            public void ShouldLogError()
            {
                var creditEvents = new List<CreditEvent> { CreditEventFactory.New.Build() };

                var username = string.Empty;

                this.userThresholdService.Setup(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username))
                    .Throws(new Exception());

                this.logger.Setup(x => x.LogError("Error on bulk credit", It.IsAny<Exception>()));

                var response = this.Controller.BulkCredit(creditEvents);

                this.userThresholdService.Verify(x => x.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username), Times.Once);

                this.logger.Verify(x => x.LogError("Error on bulk credit", It.IsAny<Exception>()), Times.Once);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
            }
        }
    }
}