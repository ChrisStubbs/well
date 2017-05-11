namespace PH.Well.UnitTests.Api.Mapper
{
    using System.Collections.Generic;
    using Factories;
    using NUnit.Framework;
    using Well.Api.Mapper;
    using Well.Domain;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class StopMapperTests
    {

        private readonly Branch branch = new BranchFactory().Build();
        private readonly RouteHeader routeHeader = new RouteHeaderFactory().Build();
        private List<Branch> branches;
        private StopMapper mapper;

        [SetUp]
        public void SetUp()
        {
            mapper = new StopMapper();
            branches = new List<Branch> { branch };
        }

        [Test]
        public void ShouldMapStopModelHeaderItems()
        {
            var stop = new StopFactory().Build();
            var stopModel = mapper.Map(branches, routeHeader, stop, new List<Job>(), new List<Assignee>());

            Assert.That(stopModel.RouteId,Is.EqualTo(routeHeader.Id));
            Assert.That(stopModel.RouteNumber, Is.EqualTo(routeHeader.RouteNumber));
            Assert.That(stopModel.Branch, Is.EqualTo(branch.BranchName));
            Assert.That(stopModel.BranchId, Is.EqualTo(branch.Id));
            Assert.That(stopModel.Driver, Is.EqualTo(routeHeader.DriverName));
            Assert.That(stopModel.RouteDate, Is.EqualTo(routeHeader.RouteDate));
            //Assert.That(stopModel.Tba, Is.EqualTo(routeHeader.Id));
            Assert.That(stopModel.StopNo, Is.EqualTo(stop.PlannedStopNumber));
            Assert.That(stopModel.TotalNoOfStopsOnRoute, Is.EqualTo(routeHeader.PlannedStops));
        }
    }
}