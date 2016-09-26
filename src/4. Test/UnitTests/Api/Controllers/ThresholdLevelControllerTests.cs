namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Net;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class ThresholdLevelControllerTests : BaseControllerTests<ThresholdLevelController>
    {
        private Mock<IUserRepository> userRepository;

        private Mock<ILogger> logger;

        [SetUp]
        public void Setup()
        {
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);

            this.Controller = new ThresholdLevelController(this.userRepository.Object, this.logger.Object);

            this.SetupController();
        }

        public class ThePostMethod : ThresholdLevelControllerTests
        {
            [Test]
            public void ShouldSetTheThresholdAgainstTheUser()
            {
                var threshold = "Level 3";
                var username = "foo";
                var user = UserFactory.New.Build();

                this.userRepository.Setup(x => x.GetByName(username)).Returns(user);

                this.userRepository.Setup(x => x.SetThresholdLevel(user, ThresholdLevel.Level3));

                var response = this.Controller.Post(threshold, username);

                this.userRepository.Verify(x => x.GetByName(username), Times.Once);

                this.userRepository.Verify(x => x.SetThresholdLevel(user, ThresholdLevel.Level3), Times.Once);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.That(content, Does.Contain("\"success\":true"));
            }

            [Test]
            public void ShouldReturnMessageStatingUserNotSetupWhenDoesntExistInDatabase()
            {
                var threshold = "Level 3";
                var username = "foo";

                this.userRepository.Setup(x => x.GetByName(username)).Throws(new UserNotFoundException());

                var response = this.Controller.Post(threshold, username);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.That(content, Does.Contain("\"notAcceptable\":true"));
                Assert.That(content, Does.Contain("\"message\":\"foo does not exist please set the user up via 'User Branch Preferences'\""));
            }
        }
    }
}