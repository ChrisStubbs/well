namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using PH.Well.Common.Contracts;
    using Moq;
    using NUnit.Framework;

    using PH.Well.UnitTests.Factories;

    using Well.Domain;
    using Well.Repositories;
    using Well.Repositories.Contracts;

    [TestFixture]
    public class StopRepositoryTests
    {
        private Mock<ILogger> logger;
        private Mock<IWellDapperProxy> dapperProxy;
        private Mock<IUserNameProvider> userNameProvider;

        private StopRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("TestUser");

            this.repository = new StopRepository(this.logger.Object, this.dapperProxy.Object, this.userNameProvider.Object);
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

        public class TheGetByJobMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldGetTheStopByAJob()
            {
                var job =
                    new JobFactory().With(x => x.PickListRef = "12221")
                        .With(x => x.PhAccount = "55444.333")
                        .With(x => x.InvoiceNumber = "54444444")
                        .Build();
                
                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopGetByJob))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Picklist", job.PickListRef, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Account", job.PhAccount, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Invoice", job.InvoiceNumber, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<Stop>()).Returns(new List<Stop> { new Stop() });

                this.repository.GetByJobDetails(job.PickListRef, job.PhAccount, job.InvoiceNumber);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopGetByJob), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Picklist", job.PickListRef, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Account", job.PhAccount, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.AddParameter("Invoice", job.InvoiceNumber, DbType.String, null), Times.Once);

                this.dapperProxy.Verify(x => x.Query<Stop>(), Times.Once);
            }
        }
    }
}
