namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Domain;
    using Domain.Enums;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;

    [TestFixture]
    public class JobRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private JobRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new JobRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetByIdMethod : JobRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Job_GetById")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Job>()).Returns(new List<Job>());

                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure("Job_GetById"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Job>(), Times.Once());
            }
        }

    }
}
