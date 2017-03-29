using PH.Well.Repositories.Contracts;

namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Security.Principal;
    using System.Web.Http.Controllers;

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

        public class TheAssignMethod : UserControllerTests
        {
            [Test]
            public void ShouldAssignTheJobToAUser()
            {
                var job = new UserJob { JobId = 2, UserId = 5 };

                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns(new User());
                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobId));

                jobRepository.Setup(j => j.GetById(job.JobId)).Returns(new Job());

                var response = this.Controller.Assign(job);
           
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("success"));

                this.userRepository.Verify(x => x.AssignJobToUser(job.UserId, job.JobId), Times.Once);
            }

            [Test]
            public void ShouldReturnNotAcceptableIfNoUser()
            {
                var job = new UserJob { JobId = 2, UserId = 5 };

                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns((User) null);
                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobId));

                jobRepository.Setup(j => j.GetById(job.JobId)).Returns(new Job());

                var response = this.Controller.Assign(job);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("notAcceptable"));
            }

            [Test]
            public void ShouldReturnNotAcceptableIfNoJob()
            {
                var job = new UserJob { JobId = 2, UserId = 5 };

                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns(new User());
                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobId));

                jobRepository.Setup(j => j.GetById(job.JobId)).Returns((Job) null);

                var response = this.Controller.Assign(job);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("notAcceptable"));
            }

            [Test]
            public void ShouldLogErrorWhenExceptionThrown()
            {
                var job = new UserJob { JobId = 2, UserId = 5 };

                var exception = new Exception();
                this.userRepository.Setup(x => x.GetById(job.UserId)).Returns(new User());
                jobRepository.Setup(j => j.GetById(job.JobId)).Returns(new Job());

                this.userRepository.Setup(x => x.AssignJobToUser(job.UserId, job.JobId)).Throws(exception);

                this.logger.Setup(x => x.LogError("Error when trying to assign job for the user", exception));

                var response = this.Controller.Assign(job);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("failure"));

                this.userRepository.Verify(x => x.AssignJobToUser(job.UserId, job.JobId), Times.Once);
                this.logger.Verify(x => x.LogError("Error when trying to assign job for the user", exception), Times.Once);
            }
        }

        public class TheUnAssignMethod : UserControllerTests
        {
            [Test]
            public void ShouldInAssignTheJobToAUser()
            {
                var jobId = 5;

                //////this.userRepository.SetupSet(x => x.CurrentUser = It.IsAny<string>());
                this.userRepository.Setup(x => x.UnAssignJobToUser(jobId));

                var response = this.Controller.UnAssign(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("success"));

                this.userRepository.Verify(x => x.UnAssignJobToUser(jobId), Times.Once);
            }

            [Test]
            public void ShouldLogErrorWhenExceptionThrown()
            {
                var jobId = 5;

                var exception = new Exception();

                //////this.userRepository.SetupSet(x => x.CurrentUser = It.IsAny<string>());
                this.userRepository.Setup(x => x.UnAssignJobToUser(jobId)).Throws(exception);

                this.logger.Setup(x => x.LogError("Error when trying to unassign the user from the job", exception));

                var response = this.Controller.UnAssign(jobId);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.Content.ReadAsStringAsync().Result, Does.Contain("failure"));

                this.userRepository.Verify(x => x.UnAssignJobToUser(jobId), Times.Once);
                this.logger.Verify(x => x.LogError("Error when trying to unassign the user from the job", exception), Times.Once);
            }
        }

        public class TheUserByNameMethod : UserControllerTests
        {
            [Test]
            public void ShouldReturnUserCreditThreshold()
            {
                var user = new User {Id = 227};
                this.userRepository.Setup(x => x.GetByName("lee grunion")).Returns(user);

                var response = this.Controller.UserByName("lee grunion");
                
                var returnedUser = new User();
                response.TryGetContentValue(out returnedUser);

                Assert.That(returnedUser, Is.Not.Null);
                Assert.That(returnedUser.Id, Is.EqualTo(227));
            }

            [Test]
            public void ShouldReturnNullCreditThresholdIfNoUserThreshold()
            {
                
                this.userRepository.Setup(x => x.GetByName("lee grunion")).Returns((User)null);
                var response = this.Controller.UserByName("lee grunion");

                var returnedUser = new User();
                response.TryGetContentValue(out returnedUser);

                Assert.That(returnedUser, Is.Null);
            }
        }
    }
}