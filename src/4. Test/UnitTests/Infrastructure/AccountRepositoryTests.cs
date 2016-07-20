namespace PH.Well.UnitTests.Infrastructure
{
    using System.Data;
    using PH.Well.Common.Contracts;
    using Domain;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using System.Collections.Generic;
    using Well.Domain;

    [TestFixture]
    public class AccountRepositoryTests
    {

        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private AccountRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new AccountRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetGetAccountByStopIdMethod : AccountRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int stopId = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Account_GetByStopId")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", stopId, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Account>()).Returns(new List<Account>());
                var result = repository.GetAccountByStopId(1);

                dapperProxy.Verify(x => x.WithStoredProcedure("Account_GetByStopId"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("StopId", stopId, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Account>(), Times.Once());
            }
        }
    }
}
