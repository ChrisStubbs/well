namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class UserStatsRepositoryTests
    {
        private Mock<IDapperProxy> dapperProxy;

        private UserStatsRepository repository;

        [SetUp]
        public void Setup()
        {
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);

            this.repository = new UserStatsRepository(this.dapperProxy.Object);
        }

        public class TheGetByUserMethod : UserStatsRepositoryTests
        {
            [Test]
            public void ShouldReturnUserStats()
            {
                var userIdentity = "foo";

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.UserStatsGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UserIdentity", userIdentity, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<UserStats>()).Returns(new List<UserStats>());

                this.repository.GetByUser(userIdentity);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.UserStatsGet), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UserIdentity", userIdentity, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<UserStats>(), Times.Once);
            }
        }

        public class TheGetPendingCreditCountByUserMethod : UserStatsRepositoryTests
        {
            [Test]
            public void ShouldReturnCountOfPendingCreditsForThatUser()
            {
                var userIdentity = "foo";

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.PendingCreditCountByUserGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("identityName", userIdentity, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new List<int>());

                this.repository.GetPendingCreditCountByUser(userIdentity);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.PendingCreditCountByUserGet), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("identityName", userIdentity, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);
            }
        }
    }
}