﻿using PH.Well.Common.Contracts;

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

        [SetUp]
        public void Setup()
        {
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);
            this.activeDirectoryService = new Mock<IActiveDirectoryService>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("foo");

            this.service = new BranchService(this.userRepository.Object, this.branchRepository.Object, this.activeDirectoryService.Object, this.userNameProvider.Object);

            //////this.userRepository.SetupSet(x => x.CurrentUser = "foo");
            //////this.branchRepository.SetupSet(x => x.CurrentUser = "foo");
        }

        public class TheSaveBranchesForUserMethod : BranchServiceTests
        {
            [Test]
            public void ShouldSaveNewUserWhenUserDoesntExist()
            {
                var username = "foo";
                var branches = new Branch[] { BranchFactory.New.Build() };

                this.userRepository.Setup(x => x.GetByIdentity(username)).Returns((User)null);

                this.userRepository.Setup(x => x.Save(It.IsAny<User>()));
                this.branchRepository.Setup(x => x.SaveBranchesForUser(branches, It.IsAny<User>()));

                this.activeDirectoryService.Setup(x => x.GetUser("foo")).Returns(new User());

                this.service.SaveBranchesForUser(branches);

                this.userRepository.Verify(x => x.GetByIdentity(username), Times.Once);

                this.userRepository.Verify(x => x.Save(It.IsAny<User>()), Times.Once);
                this.branchRepository.Verify(x => x.SaveBranchesForUser(branches, It.IsAny<User>()), Times.Once);
            }

            [Test]
            public void ShouldDeleteUsersOldBranchesAndPersistTheNewBranchesWhenTheUserExists()
            {
                var username = "foo";
                var branches = new Branch[] { BranchFactory.New.Build() };
                var user = UserFactory.New.Build();

                this.userRepository.Setup(x => x.GetByIdentity(username)).Returns(user);

                this.branchRepository.Setup(x => x.SaveBranchesForUser(branches, user));
                this.branchRepository.Setup(x => x.DeleteUserBranches(user));

                this.service.SaveBranchesForUser(branches);

                this.userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
                this.branchRepository.Verify(x => x.SaveBranchesForUser(branches, user), Times.Once);
                this.branchRepository.Verify(x => x.DeleteUserBranches(user), Times.Once);
            }
        }

        public class TheGetUserBranchesFriendlyString : BranchServiceTests
        {
            [Test]
            public void IfLess6ThanShouldShowFullBranchName()
            {
                var branches = new List<Branch>();
                branches.Add(BranchFactory.New.With(x => x.Name = "Medway").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Coventry").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Farham").Build());

                this.branchRepository.Setup(x => x.GetBranchesForUser("foo")).Returns(branches);
                this.branchRepository.Setup(x => x.GetAllValidBranches()).Returns(BranchFactory.GetAllBranches());
                var branchInformation = this.service.GetUserBranchesFriendlyInformation("foo");

                Assert.That(branchInformation, Is.EqualTo("Medway, Coventry, Farham"));
            }

            [Test]
            public void IfMoreThan6ShouldThanShouldShowFirstThreeCharacters()
            {
                var branches = new List<Branch>();
                branches.Add(BranchFactory.New.With(x => x.Name = "Medway").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Coventry").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Farham").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Dunfermline").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Leeds").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Hemel").Build());
                branches.Add(BranchFactory.New.With(x => x.Name = "Belfast").Build());

                this.branchRepository.Setup(x => x.GetBranchesForUser("foo")).Returns(branches);
                this.branchRepository.Setup(x => x.GetAllValidBranches()).Returns(BranchFactory.GetAllBranches());
                var branchInformation = this.service.GetUserBranchesFriendlyInformation("foo");

                Assert.That(branchInformation, Is.EqualTo("Med, Cov, Far, Dun, Lee, Hem, Bel"));
            }

            [Test]
            public void IfAllBranchesShouldSayAllBranches()
            {
                var branches = BranchFactory.GetAllBranches();
                this.branchRepository.Setup(x => x.GetBranchesForUser("foo")).Returns(branches);
                this.branchRepository.Setup(x => x.GetAllValidBranches()).Returns(BranchFactory.GetAllBranches());
                var branchInformation = this.service.GetUserBranchesFriendlyInformation("foo");

                Assert.That(branchInformation, Is.EqualTo("All branches selected"));
            }

        }
    }
}