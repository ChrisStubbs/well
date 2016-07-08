namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Domain;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;

    [TestFixture]
    public class RouteHeaderRepositoryTests
    {
        private Mock<ILogger> logger;
        private Mock<IWellDapperProxy> dapperProxy;
        private Mock<IStopRepository> stopRepository;
        private RouteHeaderRepository repository;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.dapperProxy = new Mock<IWellDapperProxy>(MockBehavior.Strict);
            this.stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);

            this.repository = new RouteHeaderRepository(this.logger.Object, this.dapperProxy.Object, stopRepository.Object);
        }

        public class TheGetRouteHeadersMethod : RouteHeaderRepositoryTests
        {
            [Test]
            public void ShouldCallTheStoredProcedureCorrectly()
            {
                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeaders_Get")).Returns(this.dapperProxy.Object);
                dapperProxy.Setup(x => x.Query<RouteHeader>()).Returns(new List<RouteHeader>());
                repository.GetRouteHeaders();

                dapperProxy.Verify(x => x.WithStoredProcedure("RouteHeaders_Get"), Times.Once);
                dapperProxy.Verify(x => x.Query<RouteHeader>(), Times.Once);

            }

            [Test]
            public void ShouldCallGetStopByRouteHedaerIdOnceForEachRouteHeader()
            {
                var routeHeaders = new List<RouteHeader>
                {
                    RouteHeaderFactory.New.With(x => x.Id = 1).Build(),
                    RouteHeaderFactory.New.With(x => x.Id = 2).Build()
                };

                var stops1 = new List<Stop> {new Stop(), new Stop()};
                var stops2 = new List<Stop> { new Stop(), new Stop() };

                dapperProxy.Setup(x => x.WithStoredProcedure("RouteHeaders_Get")).Returns(this.dapperProxy.Object);
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
    }
}