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
    using Factories;
    using System.Linq;

    [TestFixture]
    public class SingleRouteControllerTests : BaseControllerTests<SingleRouteController>
    {
        private Mock<IBranchRepository> branchRepository;
        private Mock<IRouteHeaderRepository> routeHeaderRepository;
        private Mock<IStopRepository> stopRepository;
        private Mock<IJobRepository> jobRepository;
        private Mock<IAssigneeReadRepository> assigneeRepository;
        private Mock<ISingleRouteMapper> mapper;

        [SetUp]
        public virtual void Setup()
        {
            branchRepository = new Mock<IBranchRepository>(MockBehavior.Strict);
            routeHeaderRepository = new Mock<IRouteHeaderRepository>(MockBehavior.Strict);
            stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);
            jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            assigneeRepository = new Mock<IAssigneeReadRepository>(MockBehavior.Strict);
            mapper = new Mock<ISingleRouteMapper>(MockBehavior.Strict);
            Controller = new SingleRouteController(
                branchRepository.Object,
                routeHeaderRepository.Object,
                stopRepository.Object,
                jobRepository.Object,
                assigneeRepository.Object,
                mapper.Object);
            SetupController();
        }

        public class TheGetMethod : SingleRouteControllerTests
        {
            private readonly RouteHeader routeHeader = new RouteHeader();
            private readonly List<Branch> branches = new List<Branch>();
            private readonly List<Stop> stops = new List<Stop>();
            private readonly List<Assignee> assignees = new List<Assignee>();
            private readonly List<Job> jobs = new List<Job>();
            private readonly SingleRoute singleRoute = new SingleRoute();
            private List<JobDetailLineItemTotals> jobDetailTotalsPerRouteHeader;
            private Dictionary<int, string> jobPrimaryAccountNumber;
            private const int RouteHeaderId = 10;


            [SetUp]
            public override void Setup()
            {
                base.Setup();

                var job = JobFactory.New
                    .Build();

                jobs.Add(job);
                jobDetailTotalsPerRouteHeader = new List<JobDetailLineItemTotals>();
                this.jobPrimaryAccountNumber = jobs.Select(p => p.Id).ToDictionary(k => k, v => v.ToString());

                routeHeaderRepository.Setup(x => x.GetRouteHeaderById(RouteHeaderId)).Returns(routeHeader);
                branchRepository.Setup(x => x.GetAll()).Returns(branches);
                stopRepository.Setup(x => x.GetStopByRouteHeaderId(RouteHeaderId)).Returns(stops);

                jobRepository.Setup(x => x.GetByRouteHeaderId(RouteHeaderId)).Returns(jobs);
                jobRepository.Setup(x => x.JobDetailTotalsPerRouteHeader(RouteHeaderId)).Returns(jobDetailTotalsPerRouteHeader);
                jobRepository.Setup(x => x.GetPrimaryAccountNumberByRouteHeaderId(RouteHeaderId)).Returns(jobPrimaryAccountNumber);

                assigneeRepository.Setup(x => x.GetByRouteHeaderId(RouteHeaderId)).Returns(assignees);

                mapper.Setup(x => x.Map(branches, routeHeader, stops, jobs, assignees, jobDetailTotalsPerRouteHeader, jobPrimaryAccountNumber)).Returns(singleRoute);
            }

            [Test]
            public void ShouldCallMapperWithCorrectValuesAndReturnSingleRoute()
            {
                var response = this.Controller.Get(RouteHeaderId);

                routeHeaderRepository.Verify(x => x.GetRouteHeaderById(RouteHeaderId), Times.Once);
                branchRepository.Verify(x => x.GetAll(), Times.Once);
                stopRepository.Verify(x => x.GetStopByRouteHeaderId(RouteHeaderId), Times.Once);
                jobRepository.Verify(x => x.GetByRouteHeaderId(RouteHeaderId), Times.Once);
                assigneeRepository.Verify(x => x.GetByRouteHeaderId(RouteHeaderId), Times.Once);
                mapper.Verify(x => x.Map(branches, routeHeader, stops, jobs, assignees, jobDetailTotalsPerRouteHeader, jobPrimaryAccountNumber), Times.Once);

                Assert.That(response, Is.EqualTo(singleRoute));
            }
        }
    }
}