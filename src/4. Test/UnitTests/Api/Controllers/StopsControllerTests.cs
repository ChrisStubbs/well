namespace PH.Well.UnitTests.Api.Controllers
{
    using System.Collections.Generic;
    using PH.Well.Api.Controllers;
    using Repositories.Contracts;
    using Well.Api.Mapper.Contracts;
    using Moq;
    using NUnit.Framework;
    using Well.Api.Models;
    using Well.Domain;
    using Well.Domain.ValueObjects;
    using Stop = Well.Domain.Stop;

    [TestFixture]
    public class StopsControllerTests : BaseControllerTests<StopsController>
    {
        private Mock<IBranchRepository> branchRepository;
        private Mock<IRouteHeaderRepository> routeHeaderRepository;
        private Mock<IStopRepository> stopRepository;
        private Mock<IJobRepository> jobRepository;
        private Mock<IAssigneeReadRepository> assigneeRepository;
        private Mock<IStopMapper> mapper;

        [SetUp]
        public virtual void Setup()
        {
            branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);
            routeHeaderRepository = new Mock<IRouteHeaderRepository>(MockBehavior.Strict);
            stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);
            jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            assigneeRepository = new Mock<IAssigneeReadRepository>(MockBehavior.Strict);
            mapper = new Mock<IStopMapper>(MockBehavior.Strict);
            Controller = new StopsController(
                branchRepository.Object,
                routeHeaderRepository.Object,
                stopRepository.Object,
                jobRepository.Object,
                assigneeRepository.Object,
                mapper.Object);
            SetupController();
        }

        public class TheGetMethod : StopsControllerTests
        {
            private readonly RouteHeader routeHeader = new RouteHeader();
            private readonly List<Branch> branches = new List<Branch>();
            private readonly List<Stop> stops = new List<Stop>();
            private readonly List<Assignee> assignees = new List<Assignee>();
            private readonly List<Job> jobs = new List<Job>();
            private readonly StopModel stopModel = new StopModel();
            private List<JobDetailLineItemTotals> jobDetailLineItemTotals;

            private const int StopId = 10;
            private readonly Stop stop = new Stop {Id = StopId, RouteHeaderId = 27};

            [SetUp]
            public override void Setup()
            {
                base.Setup();

                stopRepository.Setup(x => x.GetById(StopId)).Returns(stop);
                routeHeaderRepository.Setup(x => x.GetRouteHeaderById(stop.RouteHeaderId)).Returns(routeHeader);
                branchRepository.Setup(x => x.GetAll()).Returns(branches);

                jobDetailLineItemTotals = new List<JobDetailLineItemTotals>();

                jobRepository.Setup(x => x.GetByStopId(StopId)).Returns(jobs);
                jobRepository.Setup(x => x.JobDetailTotalsPerStop(StopId)).Returns(jobDetailLineItemTotals);
                assigneeRepository.Setup(x => x.GetByStopId(StopId)).Returns(assignees);
                mapper.Setup(x => x.Map(branches, routeHeader, stop, jobs, assignees, jobDetailLineItemTotals)).Returns(stopModel);
            }

            [Test]
            public void ShouldCallMapperWithCorrectValuesAndReturnStopModel()
            {
                var response = this.Controller.Get(StopId);

                stopRepository.Verify(x => x.GetById(StopId), Times.Once);
                routeHeaderRepository.Verify(x => x.GetRouteHeaderById(stop.RouteHeaderId), Times.Once);
                branchRepository.Verify(x => x.GetAll(), Times.Once);
                
                jobRepository.Verify(x => x.GetByStopId(StopId), Times.Once);
                assigneeRepository.Verify(x => x.GetByStopId(StopId), Times.Once);
                mapper.Verify(x => x.Map(branches, routeHeader, stop, jobs, assignees, jobDetailLineItemTotals), Times.Once);

                Assert.That(response, Is.EqualTo(stopModel));
            }
        }
    }
}