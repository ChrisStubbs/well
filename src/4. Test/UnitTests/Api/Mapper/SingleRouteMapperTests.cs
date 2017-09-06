namespace PH.Well.UnitTests.Api.Mapper
{
    using System.Collections.Generic;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Well.Api.Mapper;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.Extensions;
    using Well.Domain.ValueObjects;
    using Well.Services.Contracts;
    using Branch = Well.Domain.Branch;
    using System.Linq;

    [TestFixture]
    public class SingleRouteMapperTests
    {
        private Mock<IStopStatusService> stopStatusService;
        private readonly Branch branch = new BranchFactory().Build();
        private readonly RouteHeader routeHeader = new RouteHeaderFactory().Build();
        private List<Branch> branches;
        private SingleRouteMapper mapper;

        [SetUp]
        public void SetUp()
        {
            stopStatusService = new Mock<IStopStatusService>(MockBehavior.Strict);
            mapper = new SingleRouteMapper(stopStatusService.Object);
            branches = new List<Branch> { branch };
        }

        public class TheMapMethod : SingleRouteMapperTests
        {
            [Test]
            public void ShouldMapSingleRouteItemsFromRouteAndBranch()
            {

                var singleRoute = mapper.Map(branches, routeHeader, new List<Stop>(), new List<Job>(), new List<Assignee>(), new List<JobDetailLineItemTotals>(), new Dictionary<int, string>());

                Assert.That(singleRoute.Branch, Is.EqualTo(branch.BranchName));
                Assert.That(singleRoute.BranchId, Is.EqualTo(branch.Id));
                Assert.That(singleRoute.Id, Is.EqualTo(routeHeader.Id));
                Assert.That(singleRoute.RouteNumber, Is.EqualTo(routeHeader.RouteNumber));
                Assert.That(singleRoute.Driver, Is.EqualTo(routeHeader.DriverName));
                Assert.That(singleRoute.RouteDate, Is.EqualTo(routeHeader.RouteDate));
            }

            [Test]
            public void ShouldMapItems()
            {
                var stop = new StopFactory().Build();
                var stops = new List<Stop> {stop};

                var job = new JobFactory().With(x => x.StopId = stop.Id)
                    .With(x => x.JobTypeCode = EnumExtensions.GetDescription(JobType.GlobalUplift))
                    .With(x => x.JobStatus = JobStatus.CompletedOnPaper)
                    .With(x => x.JobDetails = GetTwoCleanAndOneExceptionJobDetail())
                    .With(x=> x.ResolutionStatus = ResolutionStatus.Credited)
                    .With(x => x.JobTypeAbbreviation = "UPL-GLO")
                    .WithCod("CODFISH")
                    .WithTotalShort(20)
                    .WithOuterDiscrepancyFound(true)
                    .WithOuterCount(1)
                    .With(x => x.ProofOfDelivery = (int)ProofOfDelivery.Lucozade)
                    .Build();
                     

                     var job2 = new JobFactory().With(x => x.StopId = stop.Id)
                    .With(x => x.Id = 2)
                    .WithTotalShort(20)
                    .WithOuterDiscrepancyFound(true)
                    .WithOuterCount(1)
                    .With(x => x.JobDetails = GetOneCleanOneExceptionJobDetail())
                    .Build();

                var jobs = new List<Job> {job, job2};

                var assignees = new List<Assignee>
                {
                    new Assignee {StopId = stop.Id, JobId = job.Id, Name = "Crip Bubbs"},
                    new Assignee {StopId = stop.Id, JobId = job2.Id, Name = "Enri Pears"}
                };

                var jobDetailLineItemTotals = new List<JobDetailLineItemTotals>
                {
                    new JobDetailLineItemTotals
                    {
                        DamageTotal = 3,
                        JobDetailId = jobs[0].JobDetails[0].Id
                    }
                };
                var primaryAccounts = jobs
                    .Select((p, index) => new { p.Id, index = index.ToString() })
                    .ToDictionary(k => k.Id, v => v.index);

                var singleRoute = mapper.Map(branches, routeHeader, stops, jobs, assignees, jobDetailLineItemTotals, primaryAccounts);

                Assert.That(singleRoute.Items.Count, Is.EqualTo(2));
                var item = singleRoute.Items[0];
                
                Assert.That(item.JobId, Is.EqualTo(job.Id));
                Assert.That(item.Stop, Is.EqualTo(stop.DropId));
                Assert.That(item.StopStatus, Is.EqualTo("Complete"));
                Assert.That(item.Tba, Is.EqualTo(20));
                Assert.That(item.StopAssignee, Is.EqualTo("CB, EP"));
                Assert.That(item.Resolution, Is.EqualTo(ResolutionStatus.Credited.Description));
                Assert.That(item.Invoice, Is.EqualTo(job.InvoiceNumber));
                Assert.That(item.JobType, Is.EqualTo("Global Uplift (UPL-GLO)"));
                Assert.That(item.JobStatus, Is.EqualTo(JobStatus.CompletedOnPaper));
                Assert.That(item.JobStatusDescription, Is.EqualTo("Completed On Paper"));
                Assert.That(item.Cod, Is.EqualTo("CODFISH"));
                Assert.IsTrue(item.Pod);
                Assert.That(item.Exceptions, Is.EqualTo(3));
                Assert.That(item.Clean, Is.EqualTo(3));
                Assert.That(item.Credit, Is.EqualTo(0));
                Assert.That(item.Assignee, Is.EqualTo("Crip Bubbs"));
                Assert.That(singleRoute.Items[1].Assignee, Is.EqualTo("Enri Pears"));
                Assert.That(singleRoute.Items[1].PrimaryAccountNumber, Is.EqualTo(primaryAccounts[singleRoute.Items[1].JobId]));
            }

            private static List<JobDetail> GetTwoCleanAndOneExceptionJobDetail()
            {
                var clean1 = new JobDetailFactory().With(x => x.ShortQty = 0).Build();
                var clean2 = new JobDetailFactory().With(x => x.ShortQty = 0).Build();
                var exception = new JobDetailFactory().With(x => x.ShortQty = 5).Build();

                return new List<JobDetail>
                {
                    clean1,
                    clean2,
                    exception
                };
            }

            private static List<JobDetail> GetOneCleanOneExceptionJobDetail()
            {
                return new List<JobDetail>
                {
                     new JobDetailFactory().With(x => x.ShortQty = 0).Build(),
                     new JobDetailFactory().With(x => x.ShortQty = 7).Build()
                };
            }
        }
    }
}