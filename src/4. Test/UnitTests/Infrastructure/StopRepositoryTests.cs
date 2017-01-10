namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using PH.Well.Common.Contracts;
    using Domain;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Repositories;
    using Well.Repositories.Contracts;

    [TestFixture]
    public class StopRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private StopRepository repository;

        private string UserName = "TestUser";

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);

            this.repository = new StopRepository(this.logger.Object, this.dapperProxy.Object);
            this.repository.CurrentUser = UserName;
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

        public class TheGetStopByRouteNumberAndDropNumber : StopRepositoryTests
        {
            [Test]
            public void ShouldGetStopRouteNumberAndDropNumber()
            {
                var transportOrderReference = "BRI-999911111";

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopGetByTransportOrderReference)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("TransportOrderReference", transportOrderReference, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop>());

                var result = this.repository.GetByTransportOrderReference(transportOrderReference);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopGetByTransportOrderReference), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("TransportOrderReference", transportOrderReference, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Stop>(), Times.Once());
            }
        }

        public class TheDeleteStopByIdMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopDeleteById))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("StopId", id, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Execute());

                this.repository.DeleteStopById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopDeleteById), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Id", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("StopId", id, DbType.Int32, null), Times.AtLeastOnce);
                dapperProxy.Verify(x => x.Execute());
            }
        }

    }
}
