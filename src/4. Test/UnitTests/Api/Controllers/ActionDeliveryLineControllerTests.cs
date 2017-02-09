namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class ActionDeliveryLineControllerTests : BaseControllerTests<ActionDeliveryLinesController>
    {
        private Mock<IDeliveryReadRepository> deliveryReadRepository;

        private Mock<IDeliveryLinesToModelMapper> mapper;

        private Mock<IDeliveryLineActionService> deliveryLineActionService;

        private Mock<IJobRepository> jobRepository;

        private Mock<IBranchRepository> branchRepository;

        [SetUp]
        public void Setup()
        {
            this.deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);
            this.mapper = new Mock<IDeliveryLinesToModelMapper>(MockBehavior.Strict);
            this.deliveryLineActionService = new Mock<IDeliveryLineActionService>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);

            this.Controller = new ActionDeliveryLinesController(
                this.deliveryReadRepository.Object, 
                this.mapper.Object, 
                this.deliveryLineActionService.Object,
                this.jobRepository.Object,
                this.branchRepository.Object);

            this.SetupController();
        }

        public class TheDeliveryLineActionsMethod : ActionDeliveryLineControllerTests
        {
            [Test]
            public void ShouldReturnTheActionsTakenOnTheDeliveryLine()
            {
                var deliveryLines = new List<DeliveryLine>();
                var jobId = 44;

                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(jobId)).Returns(deliveryLines);
                this.mapper.Setup(x => x.Map(deliveryLines)).Returns(new List<DeliveryLineModel>());

                var response = this.Controller.DeliveryLineActions(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                this.deliveryReadRepository.Verify(x => x.GetDeliveryLinesByJobId(jobId), Times.Once);
                this.mapper.Verify(x => x.Map(deliveryLines), Times.Once);
            }
        }

        public class TheConfirmDeliveryLinesMethod : ActionDeliveryLineControllerTests
        {
            [Test]
            public void ShouldReturnMessageNoDeliveryLinesWhenJobIdCantFindAnyDeliveryLinesInTheDatabase()
            {
                var jobId = 101;

                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(jobId)).Returns(new List<DeliveryLine>());

                var response = this.Controller.ConfirmDeliveryLines(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.That(content, Is.EqualTo("{\"notAcceptable\":true,\"message\":\"No delivery lines found for job id (101)...\"}"));
            }

            [Test]
            public void ShouldReturnMessageNoJobFoundWhenJobDoesNotExistInDatabase()
            {
                var jobId = 101;

                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(jobId)).Returns(new List<DeliveryLine>() { new DeliveryLine() });

                this.jobRepository.Setup(x => x.GetById(jobId)).Returns((Job)null);

                var response = this.Controller.ConfirmDeliveryLines(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.That(content, Is.EqualTo("{\"notAcceptable\":true,\"message\":\"No job found for Id (101)...\"}"));
            }

            [Test]
            public void ShouldReturnMessageYourThresholdLevelIsntHighEnoughWhenUserCreditThresholdIsLowerThanTheDeliveriesCreditAmount()
            {
                var jobId = 101;
                var branchId = 2;

                var deliveryLines = new List<DeliveryLine> { DeliveryLineFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(jobId)).Returns(deliveryLines);

                this.jobRepository.Setup(x => x.GetById(jobId)).Returns(new Job());

                this.branchRepository.Setup(x => x.GetBranchIdForJob(jobId)).Returns(branchId);

                var processDeliveryActionResult = new ProcessDeliveryActionResult();
                processDeliveryActionResult.CreditThresholdLimitReached = true;

                this.deliveryLineActionService.Setup(
                    x => x.ProcessDeliveryActions(It.IsAny<List<DeliveryLine>>(), It.IsAny<AdamSettings>(), It.IsAny<string>(), branchId)).Returns(processDeliveryActionResult);

                var response = this.Controller.ConfirmDeliveryLines(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.That(content, Is.EqualTo("{\"notAcceptable\":true,\"message\":\"Your threshold level isn\'t high enough for the credit... It has been passed on for authorisation...\"}"));
            }

            [Test]
            public void ShouldReturnMessageAdamDownWhenAdamIsNotAvailable()
            {
                var jobId = 101;
                var branchId = 2;

                var deliveryLines = new List<DeliveryLine> { DeliveryLineFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(jobId)).Returns(deliveryLines);

                this.jobRepository.Setup(x => x.GetById(jobId)).Returns(new Job());

                this.branchRepository.Setup(x => x.GetBranchIdForJob(jobId)).Returns(branchId);

                var processDeliveryActionResult = new ProcessDeliveryActionResult();
                processDeliveryActionResult.AdamResponse = AdamResponse.AdamDown;

                this.deliveryLineActionService.Setup(
                    x => x.ProcessDeliveryActions(It.IsAny<List<DeliveryLine>>(), It.IsAny<AdamSettings>(), It.IsAny<string>(), branchId)).Returns(processDeliveryActionResult);

                var response = this.Controller.ConfirmDeliveryLines(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var content = response.Content.ReadAsStringAsync().Result;
                Assert.That(content, Is.EqualTo("{\"adamdown\":true}"));

            }

            [Test]
            public void ShouldReturnSuccessWhenAdamIsUpdated()
            {
                var jobId = 101;
                var branchId = 2;

                var deliveryLines = new List<DeliveryLine> { DeliveryLineFactory.New.Build() };

                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(jobId)).Returns(deliveryLines);

                this.jobRepository.Setup(x => x.GetById(jobId)).Returns(new Job());

                this.branchRepository.Setup(x => x.GetBranchIdForJob(jobId)).Returns(branchId);

                var processDeliveryActionResult = new ProcessDeliveryActionResult();
                processDeliveryActionResult.AdamResponse = AdamResponse.Success;

                this.deliveryLineActionService.Setup(
                    x => x.ProcessDeliveryActions(It.IsAny<List<DeliveryLine>>(), It.IsAny<AdamSettings>(), It.IsAny<string>(), branchId)).Returns(processDeliveryActionResult);

                var response = this.Controller.ConfirmDeliveryLines(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var content = response.Content.ReadAsStringAsync().Result;
                Assert.That(content, Is.EqualTo("{\"success\":true}"));

            }
        }
    }
}