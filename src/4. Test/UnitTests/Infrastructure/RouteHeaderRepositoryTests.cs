using Dapper;

namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using PH.Well.Common.Contracts;

    using Factories;

    using Moq;

    using NUnit.Framework;

    using Repositories;
    using Repositories.Contracts;

    using Well.Domain;

    [TestFixture]
    public class RouteHeaderRepositoryTests
    {
        private Mock<ILogger> logger;

        private Mock<IWellDapperProxy> dapperProxy;

        private RouteHeaderRepository repository;

        public string UserName
        {
            get
            {
                return this.userNameProvider.Object.GetUserName();
            }
        }

        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>(MockBehavior.Strict);
            dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("TestUser");

            repository = new RouteHeaderRepository(
                this.logger.Object,
                this.dapperProxy.Object,
                this.userNameProvider.Object);
        }

        public class TheGetRouteHeadersMethod : RouteHeaderRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteHeaderGetAll))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UserName", UserName, DbType.String, null))
                    .Returns(this.dapperProxy.Object);
                dapperProxy.Setup(
                        x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<RouteHeader>>>()))
                    .Returns(new List<RouteHeader>());
                repository.GetRouteHeaders();

                dapperProxy.Verify(
                    x => x.WithStoredProcedure(StoredProcedures.RouteHeaderGetAll),
                    Times.Once);
                dapperProxy.Verify(x => x.AddParameter("UserName", UserName, DbType.String, null), Times.Once);
                dapperProxy.Verify(
                    x => x.QueryMultiple(It.IsAny<Func<SqlMapper.GridReader, IEnumerable<RouteHeader>>>()));
            }

            public class TheGetRouteHeaderByIdMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldCallTheStoredProcedureCorrectly()
                {
                    var routeId = 1;
                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteHeaderGetById))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("Id", routeId, DbType.Int32, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(new List<RouteHeader>());
                    repository.GetRouteHeaderById(routeId);

                    dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RouteHeaderGetById), Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("Id", routeId, DbType.Int32, null), Times.Once);
                    dapperProxy.Verify(x => x.Query<RouteHeader>(), Times.Once);

                }
            }

            public class TheGetRoutesByIdMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldCallTheStoredProcedureCorrectly()
                {
                    var routesId = 1;
                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RoutesGetById))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("Id", routesId, DbType.Int32, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.Query<Routes>()).Returns(new List<Routes>());
                    repository.GetById(routesId);

                    dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RoutesGetById), Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("Id", routesId, DbType.Int32, null), Times.Once);
                    dapperProxy.Verify(x => x.Query<Routes>(), Times.Once);
                }
            }


            public class TheGetByFilenameMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldCallTheStoredProcedureCorrectly()
                {
                    var filename = "routefile1.xml";
                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RoutesCheckDuplicate))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("FileName", filename, DbType.String, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.Query<Routes>()).Returns(new List<Routes>());
                    repository.FileAlreadyLoaded(filename);

                    dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RoutesCheckDuplicate), Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("FileName", filename, DbType.String, null), Times.Once);
                    dapperProxy.Verify(x => x.Query<Routes>(), Times.Once);
                }
            }

            public class TheGetRouteHeaderByRouteNumberAndDateMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldCallTheStoredProcedureCorrectly()
                {
                    var branchId = 1;
                    var routeNumber = "001";
                    var routeDate = DateTime.Now;
                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteHeaderGetByBranchRouteNumberAndDate))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("BranchId", branchId, DbType.Int32, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("RouteNumber", routeNumber, DbType.String, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("RouteDate", routeDate, DbType.DateTime, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(new List<RouteHeader>());
                    repository.GetRouteHeaderByRoute(branchId,routeNumber, routeDate);

                    dapperProxy.Verify(
                        x => x.WithStoredProcedure(StoredProcedures.RouteHeaderGetByBranchRouteNumberAndDate),
                        Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("BranchId", branchId, DbType.Int32, null), Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("RouteNumber", routeNumber, DbType.String, null), Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("RouteDate", routeDate, DbType.DateTime, null), Times.Once);
                    dapperProxy.Verify(x => x.Query<RouteHeader>(), Times.Once);
                }
            }

            public class TheGetRouteAttributeExceptionMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldCallTheStoredProcedureCorrectly()
                {
                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteAttributesGetExceptions))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.Query<RouteAttributeException>())
                        .Returns(new List<RouteAttributeException>());
                    repository.GetRouteAttributeException();

                    dapperProxy.Verify(
                        x => x.WithStoredProcedure(StoredProcedures.RouteAttributesGetExceptions),
                        Times.Once);
                    dapperProxy.Verify(x => x.Query<RouteAttributeException>(), Times.Once);
                }
            }

            public class TheSaveRoutesMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldSaveRoutesFile()
                {
                    var routes = RoutesFactory.New.Build();

                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteInsert))
                        .Returns(dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("Filename", routes.FileName, DbType.String, null))
                        .Returns(dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null))
                        .Returns(dapperProxy.Object);
                    this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] { 1 });

                    this.repository.Create(routes);

                    this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RouteInsert), Times.Exactly(1));
                    this.dapperProxy.Verify(
                        x => x.AddParameter("Filename", routes.FileName, DbType.String, null),
                        Times.Exactly(1));
                    this.dapperProxy.Verify(
                        x => x.AddParameter("Username", UserName, DbType.String, null),
                        Times.Exactly(1));

                    this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));
                }
            }

            public class TheDeleteRouteHeaderByIdMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldCallTheStoredProcedureCorrectly()
                {
                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.DeleteRouteHeaderWithNoStops))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("UpdatedBy", UserName, DbType.String, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.Execute());

                    this.repository.DeleteRouteHeaderWithNoStops();

                    dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.DeleteRouteHeaderWithNoStops), Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("UpdatedBy", UserName, DbType.String, null), Times.Once);
                    dapperProxy.Verify(x => x.Execute());
                }
            }

            public class TheDeleteRoutesFileIdMethod : RouteHeaderRepositoryTests
            {
                [Test]
                public void ShouldCallTheStoredProcedureCorrectly()
                {
                    const int id = 1;

                    dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RoutesDeleteById))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.AddParameter("RoutesId", id, DbType.Int32, null))
                        .Returns(this.dapperProxy.Object);
                    dapperProxy.Setup(x => x.Execute());

                    this.repository.RoutesDeleteById(id);

                    dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RoutesDeleteById), Times.Once);
                    dapperProxy.Verify(x => x.AddParameter("RoutesId", id, DbType.Int32, null), Times.AtLeastOnce);
                    dapperProxy.Verify(x => x.Execute());
                }
            }
        }
    }
} 