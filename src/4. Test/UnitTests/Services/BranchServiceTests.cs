using PH.Well.Common.Contracts;

namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class BranchServiceTests
    {
        private Mock<IUserRepository> userRepository;
        private Mock<IBranchRepository> branchRepository;
        private Mock<IActiveDirectoryService> activeDirectoryService;
        private BranchService service;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IDbMultiConfiguration> connections;
        private Mock<IUserService> userService;

        private const string ConnectionString = "Connection";

        [SetUp]
        public void Setup()
        {
            userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);
            activeDirectoryService = new Mock<IActiveDirectoryService>(MockBehavior.Strict);
            userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            userNameProvider.Setup(x => x.GetUserName()).Returns("foo");
            connections = new Mock<IDbMultiConfiguration>();
            userService = new Mock<IUserService>();

            this.connections.Setup(x => x.ConnectionStrings).Returns(new List<string> { ConnectionString });

            this.service = new BranchService(userRepository.Object, 
                branchRepository.Object, 
                activeDirectoryService.Object, 
                userNameProvider.Object,
                connections.Object,
                userService.Object);

        }

        public class TheSaveBranchesForUserMethod : BranchServiceTests
        {
            [Test]
            public void ShouldSaveNewUserWhenUserDoesntExist()
            {
                var username = "foo";
                var branches = new Branch[] { BranchFactory.New.Build() };

                userRepository.Setup(x => x.GetByIdentity(username)).Returns((User)null);
                //userRepository.Setup(x => x.Save(It.IsAny<User>(), ConnectionString));
                branchRepository.Setup(x => x.SaveBranchesForUser(branches, It.IsAny<User>(),ConnectionString));
                var user = new User();

                activeDirectoryService.Setup(x => x.GetUser("foo")).Returns(user);
                userService.Setup(x => x.CreateUserIfNotExists(user, ConnectionString)).Returns(user);
                service.SaveBranchesForUser(branches);

                userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
                branchRepository.Verify(x => x.SaveBranchesForUser(branches, It.IsAny<User>(), ConnectionString), Times.Once);
                userService.Verify(x => x.CreateUserIfNotExists(user, ConnectionString), Times.Once);
                userService.Verify(x => x.CreateUserIfNotExists(user, ConnectionString), Times.Once);
            }

            [Test]
            public void ShouldDeleteUsersOldBranchesAndPersistTheNewBranchesWhenTheUserExists()
            {
                var username = "foo";
                var branches = new Branch[] { BranchFactory.New.Build() };
                var user = UserFactory.New.Build();

                userRepository.Setup(x => x.GetByIdentity(username)).Returns(user);
                branchRepository.Setup(x => x.SaveBranchesForUser(branches, user, ConnectionString));
                branchRepository.Setup(x => x.DeleteUserBranches(user, ConnectionString));
                userService.Setup(x => x.CreateUserIfNotExists(user, ConnectionString)).Returns(user);

                service.SaveBranchesForUser(branches);

                userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
                branchRepository.Verify(x => x.SaveBranchesForUser(branches, user, ConnectionString), Times.Once);
                branchRepository.Verify(x => x.DeleteUserBranches(user, ConnectionString), Times.Once);
                userService.Verify(x => x.CreateUserIfNotExists(user, ConnectionString), Times.Once);
            }
        }

        public class TheGetUserBranchesFriendlyString : BranchServiceTests
        {
            [Test]
            public void IfLess6ThanShouldShowFullBranchName()
            {
                var branches = new List<Branch>
                {
                    BranchFactory.New.With(x => x.Name = "Medway").Build(),
                    BranchFactory.New.With(x => x.Name = "Coventry").Build(),
                    BranchFactory.New.With(x => x.Name = "Farham").Build()
                };

                branchRepository.Setup(x => x.GetBranchesForUser("foo")).Returns(branches);
                branchRepository.Setup(x => x.GetAllValidBranches()).Returns(BranchFactory.GetAllBranches());
                var branchInformation = service.GetUserBranchesFriendlyInformation("foo");

                Assert.That(branchInformation, Is.EqualTo("Medway, Coventry, Farham"));
            }

            [Test]
            public void IfMoreThan6ShouldThanShouldShowFirstThreeCharacters()
            {
                var branches = new List<Branch>
                {
                    BranchFactory.New.With(x => x.Name = "Medway").Build(),
                    BranchFactory.New.With(x => x.Name = "Coventry").Build(),
                    BranchFactory.New.With(x => x.Name = "Farham").Build(),
                    BranchFactory.New.With(x => x.Name = "Dunfermline").Build(),
                    BranchFactory.New.With(x => x.Name = "Leeds").Build(),
                    BranchFactory.New.With(x => x.Name = "Hemel").Build(),
                    BranchFactory.New.With(x => x.Name = "Belfast").Build()
                };

                branchRepository.Setup(x => x.GetBranchesForUser("foo")).Returns(branches);
                branchRepository.Setup(x => x.GetAllValidBranches()).Returns(BranchFactory.GetAllBranches());
                var branchInformation = service.GetUserBranchesFriendlyInformation("foo");

                Assert.That(branchInformation, Is.EqualTo("Med, Cov, Far, Dun, Lee, Hem, Bel"));
            }

            [Test]
            public void IfAllBranchesShouldSayAllBranches()
            {
                var branches = BranchFactory.GetAllBranches();
                branchRepository.Setup(x => x.GetBranchesForUser("foo")).Returns(branches);
                branchRepository.Setup(x => x.GetAllValidBranches()).Returns(BranchFactory.GetAllBranches());
                var branchInformation = service.GetUserBranchesFriendlyInformation("foo");

                Assert.That(branchInformation, Is.EqualTo("All branches selected"));
            }

        }
    }
}