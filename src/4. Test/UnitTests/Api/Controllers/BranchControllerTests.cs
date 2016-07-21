namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class BranchControllerTests
    {
        private Mock<ILogger> logger;

        private Mock<IBranchRepository> branchRepository;

        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;

        private BranchController controller;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);
            this.serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);

            this.controller = new BranchController(
                this.logger.Object,
                this.branchRepository.Object,
                this.serverErrorResponseHandler.Object)
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        public class TheGetMethod : BranchControllerTests
        {
            [Test]
            public void ShouldReturnTheBranches()
            {
                var branches = new List<Branch> { BranchFactory.New.Build() };

                this.branchRepository.Setup(x => x.GetAll()).Returns(branches);

                var response = this.controller.Get();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedBranches = new List<Branch>();

                response.TryGetContentValue(out returnedBranches);

                Assert.That(returnedBranches.Count, Is.EqualTo(1));
                Assert.That(returnedBranches[0].Name, Is.EqualTo(branches[0].Name));
            }

            [Test]
            public void ShouldReturnNotFoundNoneReturned()
            {
                var branches = new List<Branch>();

                this.branchRepository.Setup(x => x.GetAll()).Returns(branches);

                var response = this.controller.Get();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldLogAnErrorWhenExceptionThrown()
            {
                var exception = new Exception();

                this.branchRepository.Setup(x => x.GetAll()).Throws(exception);
                this.logger.Setup(x => x.LogError("An error occcured when getting branches!", exception));

                this.serverErrorResponseHandler.Setup(x => x.HandleException(It.IsAny<HttpRequestMessage>(), exception))
                    .Returns(new HttpResponseMessage());
                
                this.controller.Get();

                this.logger.Verify(x => x.LogError("An error occcured when getting branches!", exception), Times.Once);
            }
        }
    }
}