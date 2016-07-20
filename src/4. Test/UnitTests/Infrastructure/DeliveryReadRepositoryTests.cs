namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Repositories.Read;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class DeliveryReadRepositoryTests
    {
        private Mock<ILogger> logger = new Mock<ILogger>(MockBehavior.Strict);
        private Mock<IDapperReadProxy> dapperProxy= new Mock<IDapperReadProxy>(MockBehavior.Strict);

        private DeliveryReadRepository repository;
        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IDapperReadProxy>(MockBehavior.Strict);

            this.repository = new DeliveryReadRepository(this.logger.Object, this.dapperProxy.Object);
        }

        public class TheGetCleanDeliveriesMethod : DeliveryReadRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var status = PerformanceStatus.Compl;
                dapperProxy.Setup(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Delivery>()).Returns(new List<Delivery>());
                var result = repository.GetCleanDeliveries();

                dapperProxy.Verify(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Delivery>(), Times.Once());
            }
        }

        public class TheGetResolvedDeliveriesMethod : DeliveryReadRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var status = PerformanceStatus.Incom;
                dapperProxy.Setup(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Delivery>()).Returns(new List<Delivery>());
                var result = repository.GetResolvedDeliveries();

                dapperProxy.Verify(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Delivery>(), Times.Once());
            }
        }
    }
}