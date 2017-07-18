namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using StructureMap;
    using Well.Api.DependencyResolution;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class ManualCompletionServiceTests
    {
        private Mock<IJobService> jobService;
        private Mock<IEpodUpdateService> epodUpdateService;
        private Mock<ILineItemActionRepository> lineItemActionRepository;
        private ManualCompletionService manualCompletionService;

        [SetUp]
        public virtual void SetUp()
        {
            jobService = new Mock<IJobService>();
            epodUpdateService = new Mock<IEpodUpdateService>();
            lineItemActionRepository = new Mock<ILineItemActionRepository>();
            manualCompletionService = new ManualCompletionService(jobService.Object, epodUpdateService.Object, lineItemActionRepository.Object);
        }

        public class TheCompleteAsBypassedMethod : ManualCompletionServiceTests
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

                jobService.Setup(x => x.GetJobsIdsAssignedToCurrentUser(jobIds)).Returns(jobIds);
                jobService.Setup(x => x.GetJobsWithRoute(jobIds)).Returns(jobList);
                manualCompletionService.CompleteAsBypassed(jobIds);

                Assert.That(job.PerformanceStatus, Is.EqualTo(PerformanceStatus.Wbypa));
                Assert.That(job2.PerformanceStatus, Is.EqualTo(PerformanceStatus.Wbypa));
            }
        }

        public class TheCompleteAsCleanMethod : ManualCompletionServiceTests
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

                jobService.Setup(x => x.GetJobsIdsAssignedToCurrentUser(jobIds)).Returns(jobIds);
                jobService.Setup(x => x.GetJobsWithRoute(jobIds)).Returns(jobList);

                manualCompletionService.CompleteAsClean(jobIds);

                Assert.That(job.JobStatus, Is.EqualTo(JobStatus.Clean));
                Assert.That(job2.JobStatus, Is.EqualTo(JobStatus.Clean));
            }
        }

        public class TheManuallyCompleteJobsMethod : ManualCompletionServiceTests
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

                jobService.Setup(x => x.GetJobsIdsAssignedToCurrentUser(jobIds)).Returns(jobIds);
                jobService.Setup(x => x.GetJobsWithRoute(jobIds)).Returns(jobList);
            }

            [Test]
            public void ShouldCallEpodUpdateServiceOnceForEachInvoicedJob()
            {
                manualCompletionService.ManuallyCompleteJobs(jobIds, DoNothingAction);
                epodUpdateService.Verify(x => x.UpdateJob(It.IsAny<JobDTO>(), It.IsAny<Job>(), It.IsAny<int>(), It.IsAny<DateTime>()), Times.Exactly(2));
                epodUpdateService.Verify(x => x.UpdateJob(It.Is<JobDTO>(dto => dto.Id == job1.Id), job1, job1.JobRoute.BranchId, job1.JobRoute.RouteDate), Times.Once);
                epodUpdateService.Verify(x => x.UpdateJob(It.Is<JobDTO>(dto => dto.Id == job2.Id), job2, job2.JobRoute.BranchId, job2.JobRoute.RouteDate), Times.Once);
                epodUpdateService.Verify(x => x.UpdateJob(It.Is<JobDTO>(dto => dto.Id == job3.Id), job3, job3.JobRoute.BranchId, job3.JobRoute.RouteDate), Times.Never);
            }

            [Test]
            public void ShouldDleteLineItemActionsOnceForEachInvoicedJob()
            {
                manualCompletionService.ManuallyCompleteJobs(jobIds, DoNothingAction);
                lineItemActionRepository.Verify(x => x.DeleteAllLineItemActionsForJob(It.IsAny<int>()), Times.Exactly(2));
                lineItemActionRepository.Verify(x => x.DeleteAllLineItemActionsForJob(job1.Id), Times.Exactly(1));
                lineItemActionRepository.Verify(x => x.DeleteAllLineItemActionsForJob(job2.Id), Times.Exactly(1));
                lineItemActionRepository.Verify(x => x.DeleteAllLineItemActionsForJob(job3.Id), Times.Never);
               
            }

            [Test]
            public void ShouldSetResolutionStatusCompletedByWell()
            {
                manualCompletionService.ManuallyCompleteJobs(jobIds, DoNothingAction);
                Assert.That(job1.ResolutionStatus, Is.EqualTo(ResolutionStatus.ManuallyCompleted));
                Assert.That(job2.ResolutionStatus, Is.EqualTo(ResolutionStatus.ManuallyCompleted));
            }

            [Test]
            public void ShouldRunPostImvoiceProcessingOnceForEachJob()
            {
                manualCompletionService.ManuallyCompleteJobs(jobIds, DoNothingAction);
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.IsAny<List<int>>()), Times.Exactly(2));
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.Is<List<int>>(jobs => jobs.Contains(job1.Id) && jobs.Count == 1)), Times.Once);
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.Is<List<int>>(jobs => jobs.Contains(job2.Id) && jobs.Count == 1)), Times.Once);
                epodUpdateService.Verify(x => x.RunPostInvoicedProcessing(It.Is<List<int>>(jobs => jobs.Contains(job3.Id) && jobs.Count == 1)), Times.Never);
            }

            private void DoNothingAction(IEnumerable<Job> invoicedJobs) { }
        }

        public class TheGetJobsAvailableForCompletionMethod : ManualCompletionServiceTests
        {
            private Job job1;
            private Job job2;
            private Job job3;

            public override void SetUp()
            {
                base.SetUp();
                job1 = JobFactory.New.With(x => x.WellStatus = WellStatus.Invoiced).Build();
                job2 = JobFactory.New.With(x => x.WellStatus = WellStatus.Invoiced).Build();
                job3 = JobFactory.New.With(x => x.WellStatus = WellStatus.Invoiced).Build();
            }

            [Test]
            public void ShouldCallGetJobIdsAssignedToCurrentUserAndPassToGetJobsWithRoute()
            {
                var jobIds = new[] { 1, 2, 3 };

                var jobList = new List<Job> { job1, job2, job3 };

                jobService.Setup(x => x.GetJobsIdsAssignedToCurrentUser(jobIds)).Returns(new[] { 1, 3 });
                jobService.Setup(x => x.GetJobsWithRoute(It.IsAny<IEnumerable<int>>())).Returns(jobList);
                manualCompletionService.GetJobsAvailableForCompletion(jobIds);

                jobService.Verify(x => x.GetJobsIdsAssignedToCurrentUser(It.IsAny<IEnumerable<int>>()), Times.Once);
                jobService.Verify(x => x.GetJobsIdsAssignedToCurrentUser(jobIds), Times.Once);
                jobService.Verify(x => x.GetJobsWithRoute(It.IsAny<IEnumerable<int>>()), Times.Once);
                jobService.Verify(x => x.GetJobsWithRoute(It.Is<IEnumerable<int>>(
                    ids => ids.Count() == 2 && ids.ToList()[0] == 1 && ids.ToList()[1] == 3)), Times.Once);
            }

            [Test]
            public void ShouldFilterJobListByInvoicedOrCmpletedOnPaper()
            {
                var jobIds = new[] { 1, 2, 3, 4, 5 };
                var job4 = JobFactory.New.With(x => x.WellStatus = WellStatus.Complete).Build();
                var job5 = JobFactory.New.With(x => x.WellStatus = WellStatus.Complete).With(x => x.JobStatus = JobStatus.CompletedOnPaper).Build();

                var jobList = new List<Job> { job1, job2, job3, job4, job5 };

                jobService.Setup(x => x.GetJobsIdsAssignedToCurrentUser(jobIds)).Returns(new[] { 1, 3 });
                jobService.Setup(x => x.GetJobsWithRoute(It.IsAny<IEnumerable<int>>())).Returns(jobList);
                var jobsAvailableForCompletion = manualCompletionService.GetJobsAvailableForCompletion(jobIds);

                Assert.True(jobsAvailableForCompletion.All(x => x.WellStatus == WellStatus.Invoiced || x.JobStatus == JobStatus.CompletedOnPaper));

            }
        }
    }
}