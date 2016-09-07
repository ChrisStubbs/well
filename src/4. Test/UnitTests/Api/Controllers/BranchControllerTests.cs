﻿namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Api.Mapper;
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class BranchControllerTests : BaseControllerTests<BranchController>
    {
        private Mock<ILogger> logger;

        private Mock<IBranchRepository> branchRepository;

        private Mock<IServerErrorResponseHandler> serverErrorResponseHandler;

        private Mock<IBranchService> branchService;

        private Mock<IBranchModelMapper> branchModelMapper;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);
            this.serverErrorResponseHandler = new Mock<IServerErrorResponseHandler>(MockBehavior.Strict);
            this.branchService = new Mock<IBranchService>(MockBehavior.Strict);
            this.branchModelMapper = new Mock<IBranchModelMapper>(MockBehavior.Strict);

            this.Controller = new BranchController(this.logger.Object,
                this.branchRepository.Object,
                this.serverErrorResponseHandler.Object,
                this.branchService.Object,
                this.branchModelMapper.Object);
            SetupController();
        }

        public class TheGetMethod : BranchControllerTests
        {
            [Test]
            public void ShouldReturnTheBranches()
            {
                var branches = new List<Branch> { BranchFactory.New.Build() };

                this.branchRepository.Setup(x => x.GetAll()).Returns(branches);

                this.branchRepository.Setup(x => x.GetBranchesForUser("")).Returns((List<Branch>)null);

                this.branchModelMapper.Setup(x => x.Map(branches, (List<Branch>)null))
                    .Returns(new List<BranchModel> { new BranchModel { Name = BranchFactory.New.Build().Name } });

                var response = this.Controller.Get();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedBranches = new List<BranchModel>();

                response.TryGetContentValue(out returnedBranches);

                Assert.That(returnedBranches.Count, Is.EqualTo(1));
                Assert.That(returnedBranches[0].Name, Is.EqualTo(branches[0].Name));
            }

            [Test]
            public void ShouldReturnNotFoundNoneReturned()
            {
                var branches = new List<Branch>();

                this.branchRepository.Setup(x => x.GetAll()).Returns(branches);

                var response = this.Controller.Get();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            }

            [Test]
            public void ShouldLogAnErrorWhenExceptionThrown()
            {
                var exception = new Exception();

                this.branchRepository.Setup(x => x.GetAll()).Throws(exception);

                this.serverErrorResponseHandler.Setup(x => x.HandleException(It.IsAny<HttpRequestMessage>(), exception, "An error occcured when getting branches!"))
                    .Returns(new HttpResponseMessage());
                
                this.Controller.Get();
            }
        }

        public class ThePostMethod : BranchControllerTests
        {
            [Test]
            public void ShouldSaveTheBranchesForTheLoggedInUser()
            {
                var branches = new Branch[] { BranchFactory.New.Build(), BranchFactory.New.Build() };
                this.branchService.Setup(x => x.SaveBranchesForUser(branches, ""));

                var response = this.Controller.Post(branches);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("success"));
            }

            [Test]
            public void ShouldReturnNotAcceptableIfNoBranchesPassedToThePostMethod()
            {
                var branches = new Branch[0];
                this.branchService.Setup(x => x.SaveBranchesForUser(branches, ""));

                var response = this.Controller.Post(branches);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("notAcceptable"));
            }

            [Test]
            public void ShouldReturnFailureWhenUnhandledExceptionOccurs()
            {
                var branches = new Branch[] { BranchFactory.New.Build(), BranchFactory.New.Build() };
                var exception = new Exception();

                this.branchService.Setup(x => x.SaveBranchesForUser(It.IsAny<Branch[]>(), "")).Throws(exception);
                this.logger.Setup(x => x.LogError("Error when trying to save branches for the user", exception));
                var response = this.Controller.Post(branches);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("failure"));
            }
        }
    }
}