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
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class UserRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private UserRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);

            this.repository = new UserRepository(this.logger.Object, this.dapperProxy.Object);
            this.repository.CurrentUser = "Some user";
        }

        public class TheGetByNameMethod : UserRepositoryTests
        {
            [Test]
            public void ShouldGetTheUserByName()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.UserGetByName))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Name", "Leeroy", DbType.String, 500))
                    .Returns(this.dapperProxy.Object);

                var users = new List<User> { UserFactory.New.Build() };

                this.dapperProxy.Setup(x => x.Query<User>()).Returns(users);

                var result = this.repository.GetByName("Leeroy");

                Assert.That(result.Name, Is.EqualTo(users.First().Name));

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.UserGetByName), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Name", "Leeroy", DbType.String, 500), Times.Once);

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

                this.dapperProxy.Setup(x => x.AddParameter("Name", user.Name, DbType.String, 500))
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

                this.dapperProxy.Verify(x => x.AddParameter("Name", user.Name, DbType.String, 500), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("CreatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateCreated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UpdatedBy", this.repository.CurrentUser, DbType.String, 50), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("DateUpdated", It.IsAny<DateTime>(), DbType.DateTime, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);
            }
        }
    }
}