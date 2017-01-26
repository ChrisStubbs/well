namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    [TestFixture]
    public class ExceptionSubmissionControllerTests : BaseControllerTests<ExceptionSubmissionController>
    {
        private Mock<ILogger> logger;

        private Mock<IDeliveryReadRepository> deliveryReadRepository;

        private Mock<IDeliveryLinesToModelMapper> mapper;

        private Mock<IExceptionEventService> exceptionEventService;

        private Mock<IJobRepository> jobRepository;

        private Mock<IBranchRepository> branchRepository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);
            this.mapper = new Mock<IDeliveryLinesToModelMapper>(MockBehavior.Strict);
            this.exceptionEventService = new Mock<IExceptionEventService>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);

            this.Controller = new ExceptionSubmissionController(
                this.logger.Object, 
                this.deliveryReadRepository.Object, 
                this.mapper.Object, 
                this.exceptionEventService.Object,
                this.jobRepository.Object,
                this.branchRepository.Object);

            this.SetupController();
        }

        public class TheGetConfirmationDetailsMethod : ExceptionSubmissionControllerTests
        {
            [Test]
            public void ShouldReturnTheExceptionDeliveryConfirmationDetails()
            {
                var deliveryLines = new List<DeliveryLine>();
                var jobId = 44;

                this.deliveryReadRepository.Setup(x => x.GetDeliveryLinesByJobId(jobId)).Returns(deliveryLines);
                this.mapper.Setup(x => x.Map(deliveryLines)).Returns(new List<DeliveryLineModel>());

                var response = this.Controller.GetConfirmationDetails(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                this.deliveryReadRepository.Verify(x => x.GetDeliveryLinesByJobId(jobId), Times.Once);
                this.mapper.Verify(x => x.Map(deliveryLines), Times.Once);
            }
        }
    }
}