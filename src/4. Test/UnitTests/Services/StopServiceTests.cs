using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Services.Contracts;

namespace PH.Well.UnitTests.Services
{
    using System.Threading;

    [TestFixture]
    public class StopServiceTests
    {
        private StopService service;
        private Mock<IStopRepository> stopRepository;
        private Mock<IRouteService> routeService;
        private Mock<IJobRepository> jobRepository;
        private Mock<WellStatusAggregator> wellStatusAggregator = new Mock<WellStatusAggregator>()
        {
            CallBase = true
        };

        [SetUp]
        public void Setup()
        {
            stopRepository = new Mock<IStopRepository>();
            routeService = new Mock<IRouteService>();
            jobRepository = new Mock<IJobRepository>();
            service = new StopService(stopRepository.Object, routeService.Object, wellStatusAggregator.Object,
                jobRepository.Object);
        }

        [Test]
        [Explicit]
        public void Should_ComputeStopWellStatusAndUpdate()
        {
            var stops = new List<Stop> { GetStopWithStatusChange(), GetStopWithStatusChange() };
            stops[0].Id = 1;
            stops[1].Id = 2;
            var ids = stops.Select(p => p.Id).ToList();

            this.stopRepository.Setup(p => p.GetForWellStatusCalculationById(ids)).Returns(stops);
            service.ComputeWellStatus(ids);

            stops[0].WellStatus = WellStatus.Complete;
            stops[1].WellStatus = WellStatus.Complete;

            Thread.Sleep(100);
            this.stopRepository.Verify(p => p.GetForWellStatusCalculationById(ids), Times.Once);
            stopRepository.Verify(x => x.UpdateWellStatus(It.Is<IList<Stop>>(y => y[0].Id == stops[0].Id && y[1].Id == stops[1].Id)), Times.Once);
        }

        [Test]
        public void ShouldNot_ComputeStopWellStatusAndUpdate()
        {
            var stops = new List<Stop> { GetStopWithoutStatusChange() };
            var stopsIds = stops.Select(p => p.Id).ToList();

            this.stopRepository.Setup(p => p.GetForWellStatusCalculationById(stopsIds)).Returns(stops);

            service.ComputeWellStatus(stopsIds);

            // Stop updated
            stopRepository.Verify(x => x.UpdateWellStatus(stops), Times.Never);
            // Status not changed
            Assert.AreEqual(WellStatus.Complete, stops[0].WellStatus);
        }

        [Test]
        public void Should_ComputeAndPropagateStopWellStatus()
        {
            var stop = GetStopWithStatusChange();
            service.ComputeAndPropagateWellStatus(stop);
            // Stop updated
            stopRepository.Verify(x => x.UpdateWellStatus(It.Is<IList<Stop>>(p => p[0] == stop)), Times.Once);
            // Propagated to route service
            routeService.Verify(x => x.ComputeWellStatus(stop.RouteHeaderId));
        }

        [Test]
        public void ShouldNot_ComputeAndPropagateStopWellStatus()
        {
            var stop = GetStopWithoutStatusChange();
            var changed = service.ComputeAndPropagateWellStatus(stop);

            // Stop updated
            stopRepository.Verify(x => x.UpdateWellStatus(It.IsAny<IList<Stop>>()), Times.Never);
            // Dont propagate
            routeService.Verify(x => x.ComputeWellStatus(stop.RouteHeaderId), Times.Never);
            // Status not changed
            Assert.AreEqual(WellStatus.Complete, stop.WellStatus);
            // Not changed
            Assert.False(changed);
        }

        private Stop GetStopWithStatusChange()
        {
            return new Stop
            {
                WellStatus = WellStatus.Unknown,
                Jobs = new[] { new Job { WellStatus = WellStatus.Complete } }.ToList()
            };
        }

        private Stop GetStopWithoutStatusChange()
        {
            return new Stop
            {
                WellStatus = WellStatus.Complete,
                Jobs = new[] { new Job { WellStatus = WellStatus.Complete } }.ToList()
            };
        }
    }
}
