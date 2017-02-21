﻿namespace PH.Well.UnitTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using PH.Well.Common.Contracts;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;

    [TestFixture]
    public class RouteHeaderRepositoryTests
    {
        private Mock<ILogger> logger;
        private Mock<IWellDapperProxy> dapperProxy;
        private Mock<IStopRepository> stopRepository;
        private Mock<IJobRepository> jobRepository;
        private RouteHeaderRepository repository;
        
        //////private string UserName = "TestUser";
        public string UserName { get { return this.userNameProvider.Object.GetUserName(); } }

        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>(MockBehavior.Strict);
            dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);
            jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("TestUser");

            repository = new RouteHeaderRepository(this.logger.Object, this.dapperProxy.Object,
                stopRepository.Object, jobRepository.Object, this.userNameProvider.Object);
            //////repository.CurrentUser = UserName;
        }

        public class TheGetRouteHeadersMethod : RouteHeaderRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeaders_Get")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UserName", UserName, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(new List<RouteHeader>());

                repository.GetRouteHeaders();

                dapperProxy.Verify(x => x.WithStoredProcedure("RouteHeaders_Get"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("UserName", UserName, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<RouteHeader>(), Times.Once);
            }

            [Test]
            public void ShouldCallGetStopByRouteHedearIdOnceForEachRouteHeader()
            {
                var routeHeaders = new List<RouteHeader>
                {
                    RouteHeaderFactory.New.With(x => x.Id = 1).Build(),
                    RouteHeaderFactory.New.With(x => x.Id = 2).Build()
                };

                var stops1 = new List<Stop> {new Stop() {Id = 9}, new Stop() { Id = 8 } };
                var stops2 = new List<Stop> { new Stop() { Id = 9 }, new Stop() { Id = 8 } };

                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeaders_Get")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UserName", UserName, DbType.String, null)). Returns(this.dapperProxy.Object);
                
                dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(routeHeaders);

                stopRepository.Setup(x => x.GetStopByRouteHeaderId(1)).Returns(stops1);
                stopRepository.Setup(x => x.GetStopByRouteHeaderId(2)).Returns(stops2);

                jobRepository.Setup(j => j.GetByStopId(8)).Returns(new List<Job>() {new Job()});
                jobRepository.Setup(j => j.GetByStopId(9)).Returns(new List<Job>() {new Job()});

                var results = repository.GetRouteHeaders();

                stopRepository.Verify(x => x.GetStopByRouteHeaderId(1), Times.Once);
                stopRepository.Verify(x => x.GetStopByRouteHeaderId(2), Times.Once);

                Assert.That(results.First(x=> x.Id == 1).Stops.Contains(stops1[0]));
                Assert.That(results.First(x => x.Id == 1).Stops.Contains(stops1[1]));
                Assert.That(results.First(x => x.Id == 1).Stops.Count, Is.EqualTo(2));

                Assert.That(results.First(x => x.Id == 2).Stops.Contains(stops2[0]));
                Assert.That(results.First(x => x.Id == 2).Stops.Contains(stops2[1]));
                Assert.That(results.First(x => x.Id == 2).Stops.Count, Is.EqualTo(2));
            }
        }

        public class TheGetRouteHeaderByIdMethod : RouteHeaderRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var routeId = 1;
                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeader_GetById")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id",routeId,DbType.Int32,null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(new List<RouteHeader>());
                repository.GetRouteHeaderById(routeId);

                dapperProxy.Verify(x => x.WithStoredProcedure("RouteHeader_GetById"), Times.Once);
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
                dapperProxy.Setup(x => x.WithStoredProcedure("Routes_GetById")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Id", routesId, DbType.Int32, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Routes>()).Returns(new List<Routes>());
                repository.GetById(routesId);

                dapperProxy.Verify(x => x.WithStoredProcedure("Routes_GetById"), Times.Once);
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
                dapperProxy.Setup(x => x.WithStoredProcedure("Routes_CheckDuplicate")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("FileName", filename, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<Routes>()).Returns(new List<Routes>());
                repository.FileAlreadyLoaded(filename);

                dapperProxy.Verify(x => x.WithStoredProcedure("Routes_CheckDuplicate"), Times.Once);
                dapperProxy.Verify(x => x.AddParameter("FileName", filename, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<Routes>(), Times.Once);

            }
        }

        public class TheGetRouteHeaderByRouteNumberAndDateMethod : RouteHeaderRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var routeNumber = "001";
                var routeDate = DateTime.Now;
                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeader_GetByRouteNumberAndDate")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteNumber", routeNumber, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("RouteDate", routeDate, DbType.DateTime, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(new List<RouteHeader>());
                repository.GetRouteHeaderByRoute(routeNumber, routeDate);

                dapperProxy.Verify(x => x.WithStoredProcedure("RouteHeader_GetByRouteNumberAndDate"), Times.Once);
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
                dapperProxy.Setup(x => x.WithStoredProcedure("RouteAttributes_GetExceptions")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<RouteAttributeException>()).Returns(new List<RouteAttributeException>());
                repository.GetRouteAttributeException();

                dapperProxy.Verify(x => x.WithStoredProcedure("RouteAttributes_GetExceptions"), Times.Once);
                dapperProxy.Verify(x => x.Query<RouteAttributeException>(), Times.Once);

            }
        }

        // TODO

        public class TheSaveRoutesMethod : RouteHeaderRepositoryTests
        {
            [Test]
            public void ShouldSaveRoutesFile()
            {
                var routes = RoutesFactory.New.Build();

                dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteInsert)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Filename", routes.FileName, DbType.String, null)).Returns(dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("Username", UserName, DbType.String, null)).Returns(dapperProxy.Object);
                this.dapperProxy.Setup(x => x.Query<int>()).Returns(new int[] {1});

                this.repository.Create(routes);

                this.dapperProxy.Verify(x => x.WithStoredProcedure(StoredProcedures.RouteInsert), Times.Exactly(1));
                this.dapperProxy.Verify(x => x.AddParameter("Filename", routes.FileName, DbType.String, null), Times.Exactly(1));
                this.dapperProxy.Verify(x => x.AddParameter("Username", UserName, DbType.String, null), Times.Exactly(1));

                this.dapperProxy.Verify(x => x.Query<int>(), Times.Exactly(1));

            }
        }


    public class TheDeleteRouteHeaderByIdMethod : RouteHeaderRepositoryTests
         {
             [Test]
             public void ShouldCallTheStoredProcedureCorrectly()
             {
                 const int id = 1;

                 dapperProxy.Setup(x => x.WithStoredProcedure(StoredProcedures.RouteHeaderDeleteById))
                     .Returns(this.dapperProxy.Object);
                 dapperProxy.Setup(x => x.AddParameter("RouteheaderId", id, DbType.Int32, null))
                     .Returns(this.dapperProxy.Object);
                 dapperProxy.Setup(x => x.Execute());

                 this.repository.DeleteRouteHeaderById(id);

                 dapperProxy.Verify(
                     x => x.WithStoredProcedure(StoredProcedures.RouteHeaderDeleteById), Times.Once);
                 dapperProxy.Verify(x => x.AddParameter("RouteheaderId", id, DbType.Int32, null), Times.AtLeastOnce);
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

                 dapperProxy.Verify(
                     x => x.WithStoredProcedure(StoredProcedures.RoutesDeleteById), Times.Once);
                 dapperProxy.Verify(x => x.AddParameter("RoutesId", id, DbType.Int32, null), Times.AtLeastOnce);
                 dapperProxy.Verify(x => x.Execute());
             }
         }
     }
 }
 