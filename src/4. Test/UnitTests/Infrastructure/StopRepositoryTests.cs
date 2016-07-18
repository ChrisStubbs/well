namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Domain;
    using Moq;
    using NUnit.Framework;
    using Well.Domain;
    using Well.Repositories;
    using Well.Repositories.Contracts;

    [TestFixture]
    public class StopRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private StopRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new StopRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetStopByRouteHeaderId : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int routeHeaderId = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Stops_GetByRouteHeaderId")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("routeHeaderId", routeHeaderId, DbType.Int32,null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());
                var result = repository.GetStopByRouteHeaderId(1);

                dapperProxy.Verify(x=> x.WithStoredProcedure("Stops_GetByRouteHeaderId"),Times.Once);
                dapperProxy.Verify(x => x.AddParameter("routeHeaderId", routeHeaderId, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Stop>(), Times.Once());
            }
        }

        public class TheGetByIdMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("Stop_GetById")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());
                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure("Stop_GetById"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Stop>(), Times.Once());
            }

        }
        
    }
}
