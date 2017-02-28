namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class UserRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private UserRepository repository;

        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("Some user");

            this.repository = new UserRepository(this.logger.Object, this.dapperProxy.Object, this.userNameProvider.Object);
            //////this.repository.CurrentUser = "Some user";
        }

        public class TheGetByIdentityMethod : UserRepositoryTests
        {
            [Test]
            public void ShouldGetTheUserByIdentity()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.UserGetByIdentity))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Identity", "Leeroy", DbType.String, 255))
                    .Returns(this.dapperProxy.Object);

                var users = new List<User> { UserFactory.New.Build() };

                this.dapperProxy.Setup(x => x.Query<User>()).Returns(users);

                var result = this.repository.GetByIdentity("Leeroy");

                Assert.That(result.Name, Is.EqualTo(users.First().Name));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.UserGetByIdentity), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Identity", "Leeroy", DbType.String, 255), Times.Once);

                this.dapperProxy.Verify(x => x.Query<User>(), Times.Once);
            }
        }

        public class TheSaveMethod : UserRepositoryTests
        {
            [Test]
            public void ShouldSaveTheUser()
            {
                var user = UserFactory.New.Build();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.UserSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Name", user.Name, DbType.String, 255))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("JobDescription", user.JobDescription, DbType.String, 500))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("IdentityName", user.IdentityName, DbType.String, 255))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Domain", user.Domain, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreatedBy", this.repository.CurrentUser, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", this.repository.CurrentUser, DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                this.repository.Save(user);

                Assert.That(user.Id, Is.EqualTo(1));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.UserSave), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Name", user.Name, DbType.String, 255), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("JobDescription", user.JobDescription, DbType.String, 500), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Domain", user.Domain, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("IdentityName", user.IdentityName, DbType.String, 255), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);
            }
        }

        public class TheAssignJobToUserMethod : UserRepositoryTests
        {
            [Test]
            public void ShouldSaveAJob()
            {
                var userId = 3;
                var jobId = 5;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.AssignJobToUser))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UserId", userId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("JobId", jobId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("CreatedBy", "Some user", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UpdatedBy", "Some user", DbType.String, 50))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.AssignJobToUser(userId, jobId);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.AssignJobToUser), Times.Once());

                this.dapperProxy.Verify(x => x.AddParameter("UserId", userId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("JobId", jobId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", "Some user", DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", "Some user", DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

        public class TheUnAssignJobToUserMethod : UserRepositoryTests
        {
            [Test]
            public void ShouldDeleteTheUserJob()
            {
                var jobId = 3;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.UnAssignJobToUser))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("JobId", jobId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.UnAssignJobToUser(jobId);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.UnAssignJobToUser), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("JobId", jobId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

        public class TheSetThresholdLevelMethod : UserRepositoryTests
        {
            [Test]
            public void ShouldSetThresholdLevelAgainstAUser()
            {
                var thresholdLevel = ThresholdLevel.Level3;
                var user = UserFactory.New.Build();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.ThresholdLevelSave))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("ThresholdLevelId", (int)thresholdLevel, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UserId", user.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Execute());

                this.repository.SetThresholdLevel(user, thresholdLevel);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.ThresholdLevelSave), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("ThresholdLevelId", (int)thresholdLevel, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UserId", user.Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Execute(), Times.Once);
            }
        }

        public class TheGetUserByCreditThresholdMethod : UserRepositoryTests
        {
            [Test]
            public void ShouldGetUserByCreditThreshold()
            {
                var creditThreshold = new CreditThreshold { Id = 4 };

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.UserByCreditThresholdGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("creditThresholdId", creditThreshold.Id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<User>()).Returns(new List<User>());

                this.repository.GetUserByCreditThreshold(creditThreshold);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.UserByCreditThresholdGet), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("creditThresholdId", creditThreshold.Id, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<User>(), Times.Once);
            }
        }
    }
}