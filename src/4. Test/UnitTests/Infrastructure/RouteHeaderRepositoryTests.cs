namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using PH.Well.Common.Contracts;
    using Domain;
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
        private Mock<IStopRepository> stopRepository;
        private RouteHeaderRepository repository;
        private string UserName = "TestUser";
        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);

            
            this.repository = new RouteHeaderRepository(this.logger.Object, this.dapperProxy.Object, stopRepository.Object);
            this.repository.CurrentUser = UserName;
        }

        public class TheGetRouteHeadersMethod : RouteHeaderRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                var name = "Test";
                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeaders_Get")).Returns(this.dapperProxy.Object);
               // dapperProxy.Setup(x => x.AddParameter("UserName", UserName, DbType.String, null)).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(new List<RouteHeader>());
                repository.GetRouteHeaders();

                dapperProxy.Verify(x => x.WithStoredProcedure("RouteHeaders_Get"), Times.Once);
               // dapperProxy.Verify(x => x.AddParameter("UserName", UserName, DbType.String, null), Times.Once);
                dapperProxy.Verify(x => x.Query<RouteHeader>(), Times.Once);

            }

            [Test]
            public void ShouldCallGetStopByRouteHedearIdOnceForEachRouteHeader()
            {
                var name = "Test";
                var routeHeaders = new List<RouteHeader>
                {
                    RouteHeaderFactory.New.With(x => x.Id = 1).Build(),
                    RouteHeaderFactory.New.With(x => x.Id = 2).Build()
                };

                var stops1 = new List<Stop> {new Stop(), new Stop()};
                var stops2 = new List<Stop> { new Stop(), new Stop() };

                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeaders_Get")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.AddParameter("UserName", UserName, DbType.String, null)). Returns(this.dapperProxy.Object);
                
                dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(routeHeaders);
                stopRepository.Setup(x => x.GetStopByRouteHeaderId(1)).Returns(stops1);
                stopRepository.Setup(x => x.GetStopByRouteHeaderId(2)).Returns(stops2);

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
    }
}