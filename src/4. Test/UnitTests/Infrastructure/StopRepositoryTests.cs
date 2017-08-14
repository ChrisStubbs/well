namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Dapper;
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

        protected void SetUpGetByIds()
        {
            dapperProxy.Setup(x => x.WithStoredProcedure("Stops_GetByIds")).Returns(this.dapperProxy.Object);
            dapperProxy.Setup(x => x.AddParameter("Ids", It.IsAny<DataTable>(), DbType.Object, null)).Returns(this.dapperProxy.Object);
            dapperProxy.Setup(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IList<Stop>>>())).Returns(new List<Stop>());
        }

        public class TheGetStopByRouteHeaderId : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int routeHeaderId = 1;
                SetUpGetByIds();
                dapperProxy.Setup(x => x.WithStoredProcedure("Stops_GetByRouteHeaderId")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("routeHeaderId", routeHeaderId, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<int>()).Returns(new[] { 1, 2 });

                var result = repository.GetStopByRouteHeaderId(1);

                dapperProxy.Verify(x => x.WithStoredProcedure("Stops_GetByRouteHeaderId"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("routeHeaderId", routeHeaderId, DbType.Int32, null), Times.Once);
                dapperProxy.Verify(x => x.Query<int>(), Times.Once());


            }
        }

        public class TheGetByIdMethod : StopRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                const int id = 1;
                SetUpGetByIds();
                var result = repository.GetById(id);

                dapperProxy.Verify(x => x.WithStoredProcedure("Stops_GetByIds"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Ids", It.Is<DataTable>(dt => (int)dt.Rows[0][0] == 1 && dt.Rows.Count == 1), DbType.Object, null), Times.Once);
                dapperProxy.Verify(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IList<Stop>>>()), Times.Once());

                dapperProxy.Verify(x => x.WithStoredProcedure("Stops_GetByIds"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Ids", It.Is<DataTable>(dt => 
                                (int)dt.Rows[0][0] == 1
                                && dt.Rows.Count == 1), DbType.Object, null), Times.Once);
                dapperProxy.Verify(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IList<Stop>>>()), Times.Once());
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
                        .Build();

                var branchId = 2;

                SetUpGetByIds();

                this.dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.StopGetByJob))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Picklist", job.PickListRef, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("Account", job.PhAccount, DbType.String, null))
                    .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.AddParameter("BranchId", branchId, DbType.Int32, null))
              .Returns(this.dapperProxy.Object);

                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new[] { 1 });

                this.repository.GetByJobDetails(job.PickListRef, job.PhAccount, branchId);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.StopGetByJob), Times.Once);
                this.dapperProxy.Verify(x => x.AddParameter("Picklist", job.PickListRef, DbType.String, null), Times.Once);
                this.dapperProxy.Verify(x => x.AddParameter("Account", job.PhAccount, DbType.String, null), Times.Once);
                this.dapperProxy.Verify(x => x.AddParameter("BranchId", branchId, DbType.Int32, null), Times.Once);
                this.dapperProxy.Verify(x => x.Query<int>(), Times.Once);

                dapperProxy.Verify(x => x.WithStoredProcedure("Stops_GetByIds"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("Ids", It.Is<DataTable>(dt => (int)dt.Rows[0][0] == 1 && dt.Rows.Count == 1), DbType.Object, null), Times.Once);
                dapperProxy.Verify(x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IList<Stop>>>()), Times.Once());
            }
        }
    }
}
