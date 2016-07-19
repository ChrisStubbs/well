namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class BranchRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IDapperProxy> dapperProxy;

        private BranchRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);
            
            this.repository = new BranchRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetAllMethod : BranchRepositoryTests
        {
            [Test]
            public void ShouldReturnAllBranches()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.BranchesGet)).Returns(this.dapperProxy.Object);
                this.dapperProxy.Setup(x => x.Query<Branch>()).Returns(new List<Branch>());

                this.repository.GetAll();

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.BranchesGet), Times.Once);
                this.dapperProxy.Verify(x => x.Query<Branch>(), Times.Once);
            }
        }
    }
}