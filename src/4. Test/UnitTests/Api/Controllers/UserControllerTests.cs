using PH.Well.Repositories.Contracts;

namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
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
        private Mock<IJobRepository> jobRepository;
        private Mock<ICreditThresholdRepository> creditThresholdRepository;
        [SetUp]
        public void Setup()
        {
            this.branchService = new Mock<IBranchService>(MockBehavior.Strict);
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.activeDirectoryService = new Mock<IActiveDirectoryService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("foo");
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.creditThresholdRepository = new Mock<ICreditThresholdRepository>(MockBehavior.Strict);

            //////this.userRepository.SetupSet(x => x.CurrentUser = "foo");

            this.Controller = new UserController(this.branchService.Object,
                this.activeDirectoryService.Object,
                this.userRepository.Object,
                this.logger.Object,
                this.userNameProvider.Object,
                this.jobRepository.Object
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

        public class TheAssignMethod : UserControllerTests
        {
            [Test]
            public void ShouldAssignTheJobsToAUser()
            {
                var job = new UserJobs { JobIds = new[] { 2, 3 }, UserId = 5 };

                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns(new User());
                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobIds[0]));
                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobIds[1]));

                jobRepository.Setup(j => j.GetByIds(job.JobIds)).Returns(new List<Job> { new Job { Id = 2 }, new Job { Id = 3 } });

                var response = this.Controller.Assign(job);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("success"));

                this.userRepository.Verify(x => x.AssignJobToUser(job.UserId, It.IsAny<int>()), Times.Exactly(2));
                this.userRepository.Verify(x => x.AssignJobToUser(job.UserId, job.JobIds[0]), Times.Once);
                this.userRepository.Verify(x => x.AssignJobToUser(job.UserId, job.JobIds[1]), Times.Once);
            }

            [Test]
            public void ShouldReturnNotAcceptableIfNoUser()
            {
                var job = new UserJobs { JobIds = new[] { 2 }, UserId = 5 };

                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns((User)null);
                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobIds[0]));

                jobRepository.Setup(j => j.GetByIds(job.JobIds)).Returns(new List<Job> { new Job { Id = 2 } });

                var response = this.Controller.Assign(job);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("notAcceptable"));
            }

            [Test]
            public void ShouldReturnNotAcceptableIfNoJob()
            {
                var job = new UserJobs { JobIds = new[] { 2 }, UserId = 5 };

                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns(new User());
                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobIds[0]));

                jobRepository.Setup(j => j.GetByIds(job.JobIds)).Returns(new List<Job>());

                var response = this.Controller.Assign(job);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("notAcceptable"));
            }

            [Test]
            public void ShouldLogErrorWhenExceptionThrown()
            {
                var job = new UserJobs { JobIds = new[] { 2 }, UserId = 5 };

                var exception = new Exception();
                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns(new User());
                jobRepository.Setup(j => j.GetByIds(job.JobIds)).Returns(new List<Job> { new Job { Id = 2 } });

                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobIds[0])).Throws(exception);

                this.logger.Setup(x => x.LogError("Error when trying to assign job for the user", exception));

                var response = this.Controller.Assign(job);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("failure"));

                this.userRepository.Verify(x => x.AssignJobToUser(job.UserId, job.JobIds[0]), Times.Once);
                this.logger.Verify(x => x.LogError("Error when trying to assign job for the user", exception), Times.Once);
            }
        }

        public class TheUnAssignMethod : UserControllerTests
        {
            [Test]
            public void ShouldUnAssignTheJobsToAUser()
            {
                var jobIds = new[] { 5, 7 };

                this.userRepository.Setup(x => x.UnAssignJobToUser(5));
                this.userRepository.Setup(x => x.UnAssignJobToUser(7));

                var response = this.Controller.UnAssign(jobIds);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("success"));


                this.userRepository.Verify(x => x.UnAssignJobToUser(5), Times.Once);
                this.userRepository.Verify(x => x.UnAssignJobToUser(7), Times.Once);
            }

            [Test]
            public void ShouldLogErrorWhenExceptionThrown()
            {
                var jobIds = new[] { 5 };

                var exception = new Exception();

                //////this.userRepository.SetupSet(x => x.CurrentUser = It.IsAny<string>());
                this.userRepository.Setup(x => x.UnAssignJobToUser(5)).Throws(exception);

                this.logger.Setup(x => x.LogError("Error when trying to unassign the user from the job", exception));

                var response = this.Controller.UnAssign(jobIds);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("failure"));

                this.userRepository.Verify(x => x.UnAssignJobToUser(5), Times.Once);
                this.logger.Verify(x => x.LogError("Error when trying to unassign the user from the job", exception), Times.Once);
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
                this.userRepository.Setup(x => x.GetByName("lee grunion")).Returns((User)null);
                var returnedUser = this.Controller.UserByName("lee grunion");

                Assert.That(returnedUser, Is.Null);
            }
        }
    }
}