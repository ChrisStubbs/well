using PH.Well.Repositories.Contracts;

namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net;
    using System.Net.Http;
    using Moq;
    using NUnit.Framework;
    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class UserControllerTests : BaseControllerTests<UserController>
    {
        private Mock<IBranchService> branchService;
        private Mock<IUserRepository> userRepository;
        private Mock<ILogger> logger;
        private Mock<IActiveDirectoryService> activeDirectoryService;
        private Mock<IUserNameProvider> userNameProvider;

        private Mock<ICreditThresholdRepository> creditThresholdRepository;
        private Mock<IJobService> jobService;

        [SetUp]
        public void Setup()
        {
            this.branchService = new Mock<IBranchService>(MockBehavior.Strict);
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.activeDirectoryService = new Mock<IActiveDirectoryService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("foo");
            jobService = new Mock<IJobService>();
            this.creditThresholdRepository = new Mock<ICreditThresholdRepository>(MockBehavior.Strict);

            //////this.userRepository.SetupSet(x => x.CurrentUser = "foo");

            this.Controller = new UserController(this.branchService.Object,
                this.activeDirectoryService.Object,
                this.userRepository.Object,
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
            public void ShouldOrderByUsersAndReturnCurrentUserFirst()
            {
                var me = new User
                {
                    Name = "Z",
                    IdentityName = "Z"
                };
                var users = new List<User>
                {
                    UserFactory.New
                        .With(p => p.Name = "A")
                        .With(p => p.IdentityName = "A")
                        .Build(),
                    UserFactory.New
                        .With(p => p.Name = "B")
                        .With(p => p.IdentityName = "B")
                        .Build(),
                    me
                };

                var expectResult = new List<User>();
                expectResult.Add(users[2]);
                expectResult.Add(users[0]);
                expectResult.Add(users[1]);

                this.userNameProvider.Setup(x => x.GetUserName()).Returns(me.Name);
                this.userRepository.Setup(p => p.Get(null, null, null, null, null)).Returns(users);

                var response = this.Controller.Get();

                Assert.That(response, Is.EqualTo(expectResult));
            }
        }

        public class TheUserByNameMethod : UserControllerTests
        {
            [Test]
            public void ShouldReturnUserCreditThreshold()
            {
                var user = new User { Id = 227 };
                this.userRepository.Setup(x => x.GetByName("lee grunion")).Returns(user);

                var returnedUser = this.Controller.UserByName("lee grunion");

                Assert.That(returnedUser, Is.Not.Null);
                Assert.That(returnedUser.Id, Is.EqualTo(227));
            }

            [Test]
            public void ShouldReturnNullCreditThresholdIfNoUserThreshold()
            {
                var firstName = "A";
                var lastName = "User";
                var userName = $"{firstName} {lastName}";
                var usr = new User
                {
                    Name = userName,
                    Domain = "Domain",
                    IdentityName = $"Domain\\{firstName}.{lastName}"
                };
                var resultUsers = new List<User> { usr };

                //Domain\A.User
                this.activeDirectoryService.Setup(p => p.GetUser($"{resultUsers[0].Domain}\\{firstName}.{lastName}")).Returns(usr);
                this.activeDirectoryService.Setup(p => p.FindUsers(firstName, It.IsAny<string>())).Returns(resultUsers);
                this.userRepository.Setup(x => x.GetByName(userName)).Returns((User)null);
                this.userRepository.Setup(p => p.Save(usr));

                var returnedUser = this.Controller.UserByName(userName);

                Assert.That(returnedUser, Is.EqualTo(usr));
                userRepository.Verify(p => p.Save(usr), Times.Once);
            }
        }
    }
}