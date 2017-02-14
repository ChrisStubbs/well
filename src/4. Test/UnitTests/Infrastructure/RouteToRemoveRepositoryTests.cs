namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Dapper;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;

    [TestFixture]
    public class RouteToRemoveRepositoryTests
    {
        private Mock<IDapperProxy> dapperProxy;
        private RouteToRemoveRepository repository;

        [SetUp]
        public void Setup()
        {
            this.dapperProxy = new Mock<IDapperProxy>(MockBehavior.Strict);

            this.repository = new RouteToRemoveRepository(this.dapperProxy.Object);
        }

        public class TheGetRouteIdsMethod : RouteToRemoveRepositoryTests
        {
            [Test]
            public void ShouldReturnAllRouteIdsThatAreNotSoftDeleted()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteIdsToRemoveGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new List<int> { 1, 2 });

                var ids = this.repository.GetRouteIds();

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RouteIdsToRemoveGet), Times.Once);

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);
            }
        }

        public class TheGetRouteToRemoveMethod : RouteToRemoveRepositoryTests
        {
            [Test]
            public void ShouldReturnTheFullObjectGraph()
            {
                var routeId = 1;

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteToRemoveFullObjectGraphGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("routeId", routeId, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, RouteToRemove>>())).Returns(new RouteToRemove());

                var route = this.repository.GetRouteToRemove(1);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RouteToRemoveFullObjectGraphGet), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("routeId", routeId, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, RouteToRemove>>()), Times.Once);
            }
        }
    }
}