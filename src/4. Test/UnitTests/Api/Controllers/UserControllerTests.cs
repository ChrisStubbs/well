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
        private Mock<IUserService> userService;
        private Mock<ILogger> logger;
        private Mock<IActiveDirectoryService> activeDirectoryService;
        private Mock<IUserNameProvider> userNameProvider;

        private Mock<ICreditThresholdRepository> creditThresholdRepository;
        private Mock<IJobService> jobService;

        [SetUp]
        public void Setup()
        {
            this.branchService = new Mock<IBranchService>(MockBehavior.Strict);
            this.userService = new Mock<IUserService>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.activeDirectoryService = new Mock<IActiveDirectoryService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("foo");
            jobService = new Mock<IJobService>();
            this.creditThresholdRepository = new Mock<ICreditThresholdRepository>(MockBehavior.Strict);

            //////this.userRepository.SetupSet(x => x.CurrentUser = "foo");

            this.Controller = new UserController(this.branchService.Object,
                this.activeDirectoryService.Object,
                this.userService.Object,
                this.logger.Object,
                this.userNameProvider.Object,
                jobService.Object
                );
            SetupController();
        }

        public class TheUserBranchesMethod : UserControllerTests
        {
            [Test]
            public void ShouldReturnTheUserBranchesInformationForDashboardHeader()
            {
                this.branchService.Setup(x => x.GetUserBranchesFriendlyInformation(It.IsAny<string>())).Returns("med, bir");

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

        public class TheGetMethod : UserControllerTests
        {
            [Test]
            public void ShouldCallUserServiceGet()
            {
                var users = new List<User>();
                this.userService.Setup(p => p.Get()).Returns(users);

                var response = this.Controller.Get();

                 userService.Verify(x=> x.Get(),Times.Once);
                Assert.That(response, Is.EqualTo(users));
            }
        }

        public class TheUserByNameMethod : UserControllerTests
        {
            [Test]
            public void ShouldReturnUserCreditThreshold()
            {
                var user = new User { Id = 227 };
                this.userService.Setup(x => x.GetByName("lee grunion", "palmerharvey")).Returns(user);

                var returnedUser = this.Controller.UserByName("lee grunion");

                Assert.That(returnedUser, Is.Not.Null);
                Assert.That(returnedUser.Id, Is.EqualTo(227));
            }

        }
    }
}