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
        public void Should_ComputeStopWellStatusAndUpdate()
        {
            var stop = GetStopWithStatusChange();
            var changed = service.ComputeWellStatus(stop);

            // Stop updated
            stopRepository.Verify(x => x.Update(stop));
            // Status changed
            Assert.AreEqual(WellStatus.Complete, stop.WellStatus);
            // Change reported
            Assert.True(changed);
        }

        [Test]
        public void ShouldNot_ComputeStopWellStatusAndUpdate()
        {
            var stop = GetStopWithoutStatusChange();
            var changed = service.ComputeWellStatus(stop);

            // Stop updated
            stopRepository.Verify(x => x.Update(stop),Times.Never);
            // Status not changed
            Assert.AreEqual(WellStatus.Complete, stop.WellStatus);
            // Not changed
            Assert.False(changed);
        }

        [Test]
        public void Should_ComputeAndPropagateStopWellStatus()
        {
            var stop = GetStopWithStatusChange();
            var changed = service.ComputeAndPropagateWellStatus(stop);

            // Stop updated
            stopRepository.Verify(x => x.Update(stop));
            // Propagated to route service
            routeService.Verify(x => x.ComputeWellStatus(stop.RouteHeaderId));
            // Status changed
            Assert.AreEqual(WellStatus.Complete, stop.WellStatus);
            // Change reported
            Assert.True(changed);
        }

        [Test]
        public void ShouldNot_ComputeAndPropagateStopWellStatus()
        {
            var stop = GetStopWithoutStatusChange();
            var changed = service.ComputeAndPropagateWellStatus(stop);

            // Stop updated
            stopRepository.Verify(x => x.Update(stop), Times.Never);
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
                Jobs = new[] {new Job { WellStatus = WellStatus.Complete} }.ToList()
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
