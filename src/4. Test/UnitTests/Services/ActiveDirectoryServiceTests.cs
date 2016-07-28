namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Services;

    [TestFixture]
    public class ActiveDirectoryServiceTests
    {
        private Mock<ILogger> logger;

        private ActiveDirectoryService service;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.service = new ActiveDirectoryService(this.logger.Object);
        }

        public class FindUsersMethod : ActiveDirectoryServiceTests
        {
            [Test]
            public void ShouldReturnUsersThatMatchAGivenName()
            {
                var users = service.FindUsers("grindon", "palmerharvey;thebuyco;phdirect").ToList();

                Assert.That(users.Count, Is.EqualTo(1));
                Assert.That(
                    string.Compare(
                        "lee grindon",
                        users[0].Name,
                        StringComparison.CurrentCultureIgnoreCase),
                    Is.EqualTo(0));
            }
        }

        public class TheGetUserMethod : ActiveDirectoryServiceTests
        {
            [Test]
            public void ShouldReturnTheUserFromActiveDirectoryByTheIdentityName()
            {
                var identityName = "palmerharvey\\fiona.pond";

                var user = this.service.GetUser(identityName);

                Assert.IsNotNull(user);

                Assert.That(user.Name, Is.EqualTo("Fiona Pond"));
            }
        }
    }
}