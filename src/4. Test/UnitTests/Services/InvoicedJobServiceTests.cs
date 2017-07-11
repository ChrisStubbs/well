namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using StructureMap;
    using Well.Api.DependencyResolution;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Services;
    using Well.Services.Contracts;


    [TestFixture]
    public class InvoicedJobServiceTests
    {
        private Mock<IJobService> jobService;
        private Mock<IEpodUpdateService> epodUpdateService;
        private InvoicedJobService invoicedJobService;

        [SetUp]
        public virtual void SetUp()
        {
            jobService = new Mock<IJobService>();
            epodUpdateService = new Mock<IEpodUpdateService>();
            invoicedJobService = new InvoicedJobService(jobService.Object, epodUpdateService.Object);
        }

        public class TheMarkAsBypassedMethod : InvoicedJobServiceTests
        {
            [Test]
            public void ShouldSetJobPerformanceStatusAsWellBypassed()
            {
                var jobIds = new List<int> { 1, 2 };

                var job = JobFactory.New.With(x => x.Id = 1)
                                        .WithJobRoute(1, DateTime.Today)
                                        .With(x => x.PerformanceStatus = PerformanceStatus.Notdef)
                                        .With(x => x.WellStatus = WellStatus.Invoiced).Build();

                var job2 = JobFactory.New.With(x => x.Id = 2)
                                         .WithJobRoute(1, DateTime.Today)
                                         .With(x => x.PerformanceStatus = PerformanceStatus.Notdef)
                                         .With(x => x.WellStatus = WellStatus.Invoiced).Build();

                var jobList = new List<Job> { job, job2 };

                jobService.Setup(x => x.GetJobsWithRoute(jobIds)).Returns(jobList);
                invoicedJobService.MarkAsBypassed(jobIds);

                Assert.That(job.PerformanceStatus, Is.EqualTo(PerformanceStatus.Wbypa));
                Assert.That(job2.PerformanceStatus, Is.EqualTo(PerformanceStatus.Wbypa));

            }
        }

        public class TheMarkAsCompleteMethod : InvoicedJobServiceTests
        {
            [Test]
            public void ShouldSetJobStatusAsClean()
            {
                var jobIds = new List<int> { 1, 2 };

                var job = JobFactory.New.With(x => x.Id = 1)
                    .WithJobRoute(1, DateTime.Today)
                    .With(x => x.WellStatus = WellStatus.Invoiced).Build();

                var job2 = JobFactory.New.With(x => x.Id = 2)
                    .WithJobRoute(1, DateTime.Today)
                    .With(x => x.WellStatus = WellStatus.Invoiced).Build();

                var jobList = new List<Job> { job, job2 };

                jobService.Setup(x => x.GetJobsWithRoute(jobIds)).Returns(jobList);

                invoicedJobService.MarkAsComplete(jobIds);

                Assert.That(job.JobStatus, Is.EqualTo(JobStatus.Clean));
                Assert.That(job2.JobStatus, Is.EqualTo(JobStatus.Clean));

            }
        }

        public class TheManuallyCompleteJobsMethod : InvoicedJobServiceTests
        {
            private List<int> jobIds;
            private List<Job> jobList;
            private Job job1;
            private Job job2;
            private Job job3;

            [SetUp]
            public override void SetUp()
            {
                base.SetUp();

                jobIds = new List<int> { 1, 2, 3 };
                job1 = JobFactory.New.With(x => x.Id = 1)
                    .WithJobRoute(11, DateTime.Today)
                    .With(x => x.WellStatus = WellStatus.Invoiced).Build();

                job2 = JobFactory.New.With(x => x.Id = 2)
                    .WithJobRoute(22, DateTime.Today.AddDays(-1))
                    .With(x => x.WellStatus = WellStatus.Invoiced).Build();

                job3 = JobFactory.New.With(x => x.Id = 3)
                    .WithJobRoute(33, DateTime.Today.AddDays(-2))
                    .With(x => x.WellStatus = WellStatus.RouteInProgress).Build();

                jobList = new List<Job> { job1, job2, job3 };

                jobService.Setup(x => x.GetJobsWithRoute(jobIds)).Returns(jobList);

            }

            [Test]
            public void ShouldCallEpodUpdateServiceOnceForEachInvoicedJob()
            {
                invoicedJobService.ManuallyCompleteJobs(jobIds, DoNothingAction);
                epodUpdateService.Verify(x => x.UpdateJob(It.IsAny<JobDTO>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Exactly(2));
                epodUpdateService.Verify(x => x.UpdateJob(It.Is<JobDTO>(dto => dto.Id == job1.Id), job1, job1.JobRoute.BranchId, job1.JobRoute.RouteDate), Times.Once);
                epodUpdateService.Verify(x => x.UpdateJob(It.Is<JobDTO>(dto => dto.Id == job2.Id), job2, job2.JobRoute.BranchId, job2.JobRoute.RouteDate), Times.Once);
                epodUpdateService.Verify(x => x.UpdateJob(It.Is<JobDTO>(dto => dto.Id == job3.Id), job3, job3.JobRoute.BranchId, job3.JobRoute.RouteDate), Times.Never);
            }

            [Test]
            public void ShouldSetResolutionStatusCompletedByWell()
            {
                invoicedJobService.ManuallyCompleteJobs(jobIds, DoNothingAction);
                Assert.That(job1.ResolutionStatus, Is.EqualTo(ResolutionStatus.CompletedByWell));
                Assert.That(job2.ResolutionStatus, Is.EqualTo(ResolutionStatus.CompletedByWell));
            }


            [Test]
            public void ShouldRunPostImvoiceProcessingOnceForEachJob()
            {
                invoicedJobService.ManuallyCompleteJobs(jobIds, DoNothingAction);
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.IsAny<List<int>>()), Times.Exactly(2));
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.Is<List<int>>(jobs => jobs.Contains(job1.Id) && jobs.Count == 1)), Times.Once);
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.Is<List<int>>(jobs => jobs.Contains(job2.Id) && jobs.Count == 1)), Times.Once);
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.Is<List<int>>(jobs => jobs.Contains(job3.Id) && jobs.Count == 1)), Times.Never);
            }

            private void DoNothingAction(IEnumerable<Job> invoicedJobs) { }
        }

        public class InvoicedJobServiceManualIntegrationTests : InvoicedJobServiceTests
        {
            readonly IContainer container = IoC.Container;

            [Test]
            [Explicit]
            public void MarkAsComplete()
            {
                var jobIds = new[] { 6, 7 };

                var jobService = container.GetInstance<IJobService>();
                var epodUpdateService = container.GetInstance<IEpodUpdateService>();
                var service = new InvoicedJobService(jobService, epodUpdateService);

                service.MarkAsComplete(jobIds);
            }

            [Test]
            [Explicit]
            public void MarkAsBypassed()
            {
                var jobIds = new[] { 4, 5 };

                var jobService = container.GetInstance<IJobService>();
                var epodUpdateService = container.GetInstance<IEpodUpdateService>();
                var service = new InvoicedJobService(jobService, epodUpdateService);

                service.MarkAsBypassed(jobIds);
            }
        }
    }
}