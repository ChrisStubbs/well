namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;

    using PH.Well.Repositories;

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
                var userName = "TheUser";
                dapperProxy.Setup(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UserName", userName, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Delivery>()).Returns(new List<Delivery>());
                var result = repository.GetCleanDeliveries(userName);

                dapperProxy.Verify(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("UserName", userName, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Delivery>(), Times.Once());
            }
        }

        public class TheGetResolvedDeliveriesMethod : DeliveryReadRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var status = PerformanceStatus.Resolved;
                var userName = "TheUser";

                dapperProxy.Setup(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UserName", userName, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Delivery>()).Returns(new List<Delivery>());
                var result = repository.GetResolvedDeliveries(userName);

                dapperProxy.Verify(x => x.WithStoredProcedure("Deliveries_GetByPerformanceStatus"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("PerformanceStatusId", status, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("UserName", userName, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Delivery>(), Times.Once());
            }
        }

        public class TheGetPendingCreditDeliveriesMethod : DeliveryReadRepositoryTests
        {
            [Test]
            public void ShouldReturnAllExceptionsPendingCreditAuthorisation()
            {
                var username = "foo";

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.PendingCreditDeliveriesGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("UserName", username, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<Delivery>()).Returns(new List<Delivery>());

                this.repository.GetPendingCreditDeliveries(username);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.PendingCreditDeliveriesGet), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("UserName", username, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<Delivery>(), Times.Once);
            }
        }

        public class TheGetPendingCreditDetailMethod : DeliveryReadRepositoryTests
        {
            [Test]
            public void ShouldGetPendingCreditDetails()
            {
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.JobDetailActionsGet))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("jobId", 1, DbType.Int32, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<PendingCreditDetail>()).Returns(new List<PendingCreditDetail>());

                this.repository.GetPendingCreditDetail(1);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.JobDetailActionsGet), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("jobId", 1, DbType.Int32, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<PendingCreditDetail>(), Times.Once);
            }
        }
    }
}