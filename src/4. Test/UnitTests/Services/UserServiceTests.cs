namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using Api.Controllers;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IActiveDirectoryService> activeDirectoryService;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IUserRepository> userRepository;
        private Mock<IDbMultiConfiguration> connections;

        private UserService userService;
        private const string ConnectionString = "TheConnection";

        [SetUp]
        public void Setup()
        {
            userRepository = new Mock<IUserRepository>();
            activeDirectoryService = new Mock<IActiveDirectoryService>();
            userNameProvider = new Mock<IUserNameProvider>();
            userNameProvider.Setup(x => x.GetUserName()).Returns("foo");
            connections = new Mock<IDbMultiConfiguration>();
            connections.Setup(x => x.ConnectionStrings).Returns(new List<string> { ConnectionString });
            userService = new UserService(activeDirectoryService.Object, userRepository.Object, connections.Object, userNameProvider.Object);
        }

        public class TheGetMethod : UserServiceTests
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
                this.userRepository.Setup(p => p.Get(null, null, null, null, null, null)).Returns(users);

                var response = this.userService.Get();

                Assert.That(response, Is.EqualTo(expectResult));
            }
        }

        public class TheGetByNameMethod : UserServiceTests
        {
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
                this.activeDirectoryService.Setup(p => p.FindUsers(firstName, "Domain")).Returns(resultUsers);
                this.userRepository.Setup(x => x.GetByName(userName)).Returns((User)null);
                this.userRepository.Setup(p => p.Save(usr));


                var returnedUser = this.userService.GetByName(userName, "Domain");

                Assert.That(returnedUser, Is.EqualTo(usr));
                userRepository.Verify(p => p.Save(usr, ConnectionString), Times.Once);
            }
        }
    }
}