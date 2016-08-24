using PH.Well.Repositories.Contracts;

namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Moq;
    using NUnit.Framework;
    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class UserControllerTests : BaseControllerTests<UserController>
    {
        private Mock<IBranchService> branchService;
        private Mock<IUserRepository> userRepository;
        private Mock<ILogger> logger;
        private Mock<IActiveDirectoryService> activeDirectoryService;

        [SetUp]
        public void Setup()
        {
            this.branchService = new Mock<IBranchService>(MockBehavior.Strict);
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.activeDirectoryService = new Mock<IActiveDirectoryService>(MockBehavior.Strict);

            this.Controller = new UserController(this.branchService.Object,
                this.activeDirectoryService.Object,
                this.userRepository.Object,
                this.logger.Object);
            SetupController();
        }

        public class TheUserBranchesMethod : UserControllerTests
        {
            [Test]
            public void ShouldReturnTheUserBranchesInformationForDashboardHeader()
            {
                this.branchService.Setup(x => x.GetUserBranchesFriendlyInformation("")).Returns("med, bir");

                var response = this.Controller.UserBranches();

                var result = response.Content.ReadAsStringAsync().Result;

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(result, Is.EqualTo("\"med, bir\""));
            }
        }

        public class TheUsersMethod : UserControllerTests
        {
            [Test]
            public void ShouldReturnUsersfromActiveDirectory()
            {
                var users = new List<User> { UserFactory.New.Build(), UserFactory.New.Build() };
                
                this.activeDirectoryService.Setup(x => x.FindUsers("foo", "palmerharvey")).Returns(users);

                var response = this.Controller.Users("foo");

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var returnedUsers = new List<User>();

                response.TryGetContentValue(out returnedUsers);

                Assert.That(returnedUsers.Count, Is.EqualTo(2));
            }
        }
    }
}