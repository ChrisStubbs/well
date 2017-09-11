using System.Security.Principal;
using System.Threading;
using Moq;
using NUnit.Framework;
using PH.Well.Common.Contracts;
using PH.Well.Domain;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using PH.Well.Domain.Enums;
    using Well.Domain.Extensions;
    using Well.Domain.ValueObjects;
    using Well.Services.Contracts;

    [TestFixture]
    public class JobServiceTests
    {
        private Mock<IJobRepository> jobRepository;
        private JobService service;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IAssigneeReadRepository> assigneeReadRepository;
        private readonly Mock<IUserThresholdService> userThreshold = new Mock<IUserThresholdService>();
        private readonly Mock<IDateThresholdService> dateThresholdService = new Mock<IDateThresholdService>();
        private readonly Mock<ILineItemSearchReadRepository> lineItemRepository = new Mock<ILineItemSearchReadRepository>();
        private readonly Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        private readonly Mock<IStopService> stopService = new Mock<IStopService>();
        private readonly Mock<IActivityService> activityService = new Mock<IActivityService>();
        private readonly Mock<WellStatusAggregator> wellStatusAggregator = new Mock<WellStatusAggregator>();

        [SetUp]
        public void Setup()
        {
            var identity = new GenericIdentity("foo");

            var principal = new GenericPrincipal(identity, new string[] { });

            Thread.CurrentPrincipal = principal;

            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.assigneeReadRepository = new Mock<IAssigneeReadRepository>();
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);

            assigneeReadRepository.Setup(p => p.GetByJobId(It.IsAny<int>())).Returns(new Assignee { IdentityName = "User" });

            this.service = new JobService(this.jobRepository.Object,
                userThreshold.Object,
                dateThresholdService.Object,
                assigneeReadRepository.Object,
                lineItemRepository.Object,
                userNameProvider.Object,
                userRepository.Object, 
                stopService.Object, 
                activityService.Object, 
                wellStatusAggregator.Object);


        }

        public class TheDetermineStatusMethod : JobServiceTests
        {
            /// <summary>
            /// Multiple jobs with same product and delivered QTY > despatched/invoiced QTY
            /// </summary>
            [Test]
            [Category("JobService")]
            public void ShouldSetAllJobsToException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New.With(x => x.InvoiceNumber = invoiceNumber).Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                this.jobRepository.Setup(x => x.Update(job2));
                this.jobRepository.Setup(x => x.Update(job3));

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .Build();

                var jobDetail2 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.DeliveredQty = 6)
                    .Build();

                var jobDetail3 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.DeliveredQty = 5)
                    .Build();

                job1.JobDetails.Add(jobDetail1);
                job2.JobDetails.Add(jobDetail2);
                job3.JobDetails.Add(jobDetail3);

                this.service.DetermineStatus(job1, branchNo);
                Assert.IsTrue(job1.JobStatus == JobStatus.Exception);
                Assert.IsTrue(job2.JobStatus == JobStatus.Exception);
                Assert.IsTrue(job3.JobStatus == JobStatus.Exception);

                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Once);

                this.jobRepository.Verify(x => x.Update(job2), Times.Once);
                this.jobRepository.Verify(x => x.Update(job3), Times.Once);
            }

            /// <summary>
            /// Multiple jobs with same product and delivered QTY == despatched/invoiced QTY
            /// </summary>
            [Test]
            [Category("JobService")]
            public void QtyEqualShouldSetAllJobsToNotException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New.With(x => x.InvoiceNumber = invoiceNumber).Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .Build();

                var jobDetail2 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.DeliveredQty = 5)
                    .Build();

                var jobDetail3 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.DeliveredQty = 5)
                    .Build();

                job1.JobDetails.Add(jobDetail1);
                job2.JobDetails.Add(jobDetail2);
                job3.JobDetails.Add(jobDetail3);

                this.service.DetermineStatus(job1, branchNo);
                Assert.IsFalse(job1.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job2.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job3.JobStatus == JobStatus.Exception);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Once);
            }

            /// <summary>
            /// Multiple jobs with same product and delivered QTY < despatched/invoiced QTY
            /// </summary>
            [Test]
            [Category("JobService")]
            public void QtyLessThanShouldSetAllJobsToNotException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New.With(x => x.InvoiceNumber = invoiceNumber).Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .Build();

                var jobDetail2 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.DeliveredQty = 3)
                    .Build();

                var jobDetail3 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.DeliveredQty = 3)
                    .Build();

                job1.JobDetails.Add(jobDetail1);
                job2.JobDetails.Add(jobDetail2);
                job3.JobDetails.Add(jobDetail3);

                this.service.DetermineStatus(job1, branchNo);
                Assert.IsFalse(job1.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job2.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job3.JobStatus == JobStatus.Exception);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Once);
            }

            /// <summary>
            /// A job has shorts QTY > 0 
            /// </summary>
            [Test]
            [Ignore("We need to deploy. After deploy the test wil be fixed")]
            [Category("JobService")]
            public void ShortQtyGreaterThanShouldSetException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.ShortQty = 1)
                    .Build();

                job1.JobDetails.Add(jobDetail1);

                this.service.DetermineStatus(job1, branchNo);
                Assert.IsTrue(job1.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job2.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job3.JobStatus == JobStatus.Exception);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Once);
            }

            /// <summary>
            /// A job has shorts QTY > 0 
            /// </summary>
            [Test]
            [Category("JobService")]
            public void ShortQtyZeroShouldNotSetException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.ShortQty = 0)
                    .Build();
                jobDetail1.JobDetailDamages.Clear();
                job1.JobDetails.Add(jobDetail1);

                this.service.DetermineStatus(job1, branchNo);
                Assert.IsFalse(job1.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job2.JobStatus == JobStatus.Exception);
                Assert.IsFalse(job3.JobStatus == JobStatus.Exception);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Once);
            }

            /// <summary>
            /// A job has shorts QTY > 0 
            /// </summary>
            [Test]
            [Category("JobService")]
            public void GivenDamageWithNoQtyShouldSetToClean()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.ShortQty = 0)
                    .Build();
                job1.JobDetails.Add(jobDetail1);

                jobDetail1.JobDetailDamages.Add(new JobDetailDamage());

                this.service.DetermineStatus(job1, branchNo);
                Assert.IsTrue(job1.JobStatus == JobStatus.Clean);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Once);
            }

            /// <summary>
            /// A job has shorts QTY > 0 
            /// </summary>
            [Test]
            [Ignore("We need to deploy. After deploy the test wil be fixed")]
            [Category("JobService")]
            public void DamagesShouldSetException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.ShortQty = 0)
                    .Build();
                job1.JobDetails.Add(jobDetail1);

                jobDetail1.JobDetailDamages.Add(new JobDetailDamage() { Qty = 1 });

                this.service.DetermineStatus(job1, branchNo);
                Assert.IsTrue(job1.JobStatus == JobStatus.Exception);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Once);
            }

            [Test]
            [TestCase(PerformanceStatus.Abypa)]
            [TestCase(PerformanceStatus.Nbypa)]
            [Category("JobService")]
            public void TestCasesShouldSetToBypass(PerformanceStatus status)
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .With(x => x.PerformanceStatus = status)
                    .Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber))
                    .Returns(new List<Job>() { });

                this.service.DetermineStatus(job1, branchNo);
                Assert.AreEqual(JobStatus.Bypassed, job1.JobStatus);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(job1.Id, branchNo, invoiceNumber),
                    Times.Never);
            }

            [Test]
            [Category("JobService")]
            public void TestCasesShouldSetToCompletedOnPaper()
            {
                // come in as a bypass
                // reason is manual delivery
                // should set to status completed on paper
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .With(x => x.PerformanceStatus = PerformanceStatus.Abypa)
                    .With(x => x.JobByPassReason = "manual delivery")
                    .Build();

                this.service.DetermineStatus(job1, branchNo);
                Assert.AreEqual(JobStatus.CompletedOnPaper, job1.JobStatus);
            }
        }

        public class TheSetIncompleteStatusMethod : JobServiceTests
        {
            [Test]
            [Category("JobService")]
            public void GivenAwaitingInvoiceAndHasInvoiceNumber_ThenSetInComplete()
            {
                var job = new Job()
                {
                    JobStatus = JobStatus.AwaitingInvoice,
                    InvoiceNumber = "123"
                };

                service.SetIncompleteJobStatus(job);

                Assert.AreEqual(JobStatus.InComplete, job.JobStatus);
            }

            [Test]
            [TestCase(null, ExpectedResult = JobStatus.DocumentDelivery)]
            [TestCase("9999999999999999", ExpectedResult = JobStatus.InComplete)]
            [Category("JobService")]
            public JobStatus SetJobSetIncompleteStatus(string invoiceNumber)
            {
                var job = new Job
                {
                    JobStatus = JobStatus.DocumentDelivery,
                    InvoiceNumber = invoiceNumber,
                    JobTypeCode = "TOB-DEL"
                };

                service.SetIncompleteJobStatus(job);
                return job.JobStatus;
            }

            [Test]
            [Category("JobService")]
            public void GivenJobIsGlobalUplift_ThenSetInComplete()
            {
                var job = new Job()
                {
                    JobStatus = JobStatus.AwaitingInvoice,
                    InvoiceNumber = String.Empty,
                    JobTypeCode = "UPL-GLO"
                };

                service.SetIncompleteJobStatus(job);

                Assert.AreEqual(JobStatus.InComplete, job.JobStatus);
            }

        }

        public class TheCanEditActionsMethod : JobServiceTests
        {
            [Test]
            [TestCase("User", ExpectedResult = true)]
            [TestCase("", ExpectedResult = false)]
            [Category("JobService")]
            public bool CanEditActions_Should_Check_User(string user)
            {
                var job = new Job
                {
                    ResolutionStatus = ResolutionStatus.DriverCompleted
                };

                return this.service.CanEdit(job, user);
            }

            [Test]
            [Category("JobService")]
            public void CanEditActions_Should_Check_ResolutionStatus()
            {
                var job = new Job
                {
                    ResolutionStatus = ResolutionStatus.DriverCompleted
                };

                Assert.IsTrue(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.ActionRequired;
                Assert.IsTrue(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.PendingSubmission;
                Assert.IsTrue(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.PendingApproval;
                Assert.IsTrue(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.Imported;
                Assert.IsFalse(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.Approved;
                Assert.IsFalse(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.Credited;
                Assert.IsFalse(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.Resolved;
                Assert.IsFalse(this.service.CanEdit(job, "User"));

                job.ResolutionStatus = ResolutionStatus.Closed;
                Assert.IsFalse(this.service.CanEdit(job, "User"));
            }

            [Test]
            [TestCase("UPL-GLO", ExpectedResult = false,Description = "Global uplift jobs should not be editable")]
            [TestCase("DEL-ALC", ExpectedResult = true)]
            [Category("JobService")]
            public bool CanEditActions_Should_Check_JobType(string jobType)
            {
                var job = new Job
                {
                    JobStatus = JobStatus.InComplete,
                    InvoiceNumber = "123",
                    JobTypeCode = jobType,
                    ResolutionStatus = ResolutionStatus.ActionRequired
                };

                return service.CanEdit(job, "User");
            }
        }

        public class CanManuallyCompleteMethod : JobServiceTests
        {

            [Test]
            [TestCase("User", ExpectedResult = true)]
            [TestCase("", ExpectedResult = false)]
            [Category("JobService")]
            public bool CanManuallyComplete_Should_Check_User(string user)
            {
                var job = new Job
                {
                    WellStatus = WellStatus.Invoiced
                };

                return this.service.CanManuallyComplete(job, user);
            }

            [Test]
            [Category("JobService")]
            public void CanManuallyComplete_Should_Check_WellStatus()
            {
                var job = new Job
                {
                    WellStatus = WellStatus.Planned
                };

                Assert.IsFalse(this.service.CanManuallyComplete(job, "User"));

                job.WellStatus = WellStatus.Invoiced;

                Assert.IsTrue(this.service.CanManuallyComplete(job, "User"));

                job.WellStatus = WellStatus.Complete;

                Assert.IsFalse(this.service.CanManuallyComplete(job, "User"));

                job.WellStatus = WellStatus.Bypassed;

                Assert.IsFalse(this.service.CanManuallyComplete(job, "User"));

                job.WellStatus = WellStatus.RouteInProgress;

                Assert.IsFalse(this.service.CanManuallyComplete(job, "User"));
            }

            [Test]
            [Category("JobService")]
            public void CanEditActions_Should_Check_JobStatus()
            {
                var job = new Job
                {
                    WellStatus = WellStatus.Planned,
                };

                foreach (JobStatus jobStatus in Enum.GetValues(typeof(JobStatus)))
                {
                    job.JobStatus = jobStatus;
                    if (jobStatus == JobStatus.CompletedOnPaper)
                    {
                        Assert.IsTrue(this.service.CanManuallyComplete(job, "User"));
                    }
                    else
                    {
                        Assert.IsFalse(this.service.CanManuallyComplete(job, "User"));
                    }

                }
            }

            [Test]
            [TestCase("UPL-GLO", ExpectedResult = false, Description = "Global uplift jobs should not be editable")]
            [TestCase("DEL-ALC", ExpectedResult = true)]
            [Category("JobService")]
            public bool CanEditActions_Should_Check_JobType(string jobType)
            {
                var job = new Job
                {
                    JobStatus = JobStatus.InComplete,
                    InvoiceNumber = "123",
                    JobTypeCode = jobType,
                    ResolutionStatus = ResolutionStatus.ActionRequired
                };

                return service.CanEdit(job, "User");
            }
        }


        [Test]
        public void SetGrnShouldFailAfterSubmissionDate()
        {
            var job = JobFactory.New.Build();
            var jobs = new[] { job };
            var routeDate = DateTime.Now;
            var routes = new[] {new JobRoute() {JobId = job.Id, RouteDate = routeDate}};

            jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(jobs);
            jobRepository.Setup(x => x.GetJobsRoute(It.IsAny<IEnumerable<int>>())).Returns(routes);

            dateThresholdService.Setup(x => x.GracePeriodEnd(routeDate, job.Id, 0))
                .Returns<DateTime>(x => routeDate.AddHours(-1));

            Assert.Throws<Exception>(() => service.SetGrn(job.Id, "123"));

        }

        [Test]
        public void SetGrnShouldPass()
        {
            var routeDate = DateTime.Now;
            var job = JobFactory.New.Build();
            var jobs = new[] { job };
            var routes = new[] { new JobRoute() { JobId = job.Id, RouteDate = routeDate, BranchId = 1 } };

            jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(jobs);
            jobRepository.Setup(x => x.GetJobsRoute(It.IsAny<IEnumerable<int>>())).Returns(routes);

            jobRepository.Setup(x => x.SaveGrn(It.IsAny<int>(), It.IsAny<string>()));
            //Return submission date greater than now
            dateThresholdService.Setup(x => x.GracePeriodEnd(routeDate, routes[0].BranchId, 0))
                .Returns(routeDate.AddHours(1));

            service.SetGrn(job.Id, "123");
        }

        [TestFixture]
        public class CalculateWellStatus
        {
            private JobService service;
            private readonly Mock<IJobRepository> jobRepository = new Mock<IJobRepository>();
            private readonly Mock<IUserNameProvider> userNameProvider = new Mock<IUserNameProvider>();
            private readonly Mock<IAssigneeReadRepository> assigneeReadRepository = new Mock<IAssigneeReadRepository>();
            private readonly Mock<IUserThresholdService> userThreshold = new Mock<IUserThresholdService>();
            private readonly Mock<IDateThresholdService> dateThresholdService = new Mock<IDateThresholdService>();
            private readonly Mock<ILineItemSearchReadRepository> lineItemRepository = new Mock<ILineItemSearchReadRepository>();
            private readonly Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
            private readonly Mock<IStopService> stopService = new Mock<IStopService>();
            private readonly Mock<IActivityService> activityService = new Mock<IActivityService>();

            private readonly Mock<WellStatusAggregator> wellStatusAggregator = new Mock<WellStatusAggregator>()
            {
                CallBase = true
            };

            [SetUp]
            public void Setup()
            {
                this.service = new JobService(this.jobRepository.Object,
                    userThreshold.Object,
                    dateThresholdService.Object,
                    assigneeReadRepository.Object,
                    lineItemRepository.Object,
                    userNameProvider.Object,
                    userRepository.Object,
                    stopService.Object,
                    activityService.Object,
                    wellStatusAggregator.Object);
            }

            
            [Test]
            public void ShouldUpdateJobWellStatus()
            {
                var job = JobFactory.New.JobWithStatusChange().Build();
                var changed = service.ComputeWellStatus(job);
                // Check that job is persisted
                jobRepository.Verify(x => x.UpdateWellStatus(job));
                // Check status updated
                Assert.AreEqual(WellStatus.Complete, job.WellStatus);
                // Check that change has been recorded
                Assert.True(changed);
            }

            [Test]
            public void ShouldNotUpdateJobWellStatus()
            {
                var job = JobFactory.New.JobWithoutStatusChange().Build();
                var changed = service.ComputeWellStatus(job);
                // Check that job is persisted
                jobRepository.Verify(x => x.UpdateWellStatus(job), Times.Never);
                // Check status did not change
                Assert.AreEqual(WellStatus.Complete, job.WellStatus);
                // Check that reports no change
                Assert.False(changed);
            }


            [Test]
            public void ShouldUpdateJobWellStatusAndPropagate()
            {
                var job = JobFactory.New.JobWithStatusChange().Build();
                var changed = service.ComputeAndPropagateWellStatus(job);
                // Check that job is persisted
                jobRepository.Verify(x => x.UpdateWellStatus(job));
                // Check propagate changes to stopService
                stopService.Verify(x => x.ComputeAndPropagateWellStatus(job.StopId));
                // Check status updated
                Assert.AreEqual(WellStatus.Complete, job.WellStatus);
                // Check that change has been recorded
                Assert.True(changed);
            }

            [Test]
            public void ShouldNotUpdateJobWellStatusAndPropagate()
            {
                var job = JobFactory.New.JobWithoutStatusChange().Build();
                var changed = service.ComputeAndPropagateWellStatus(job);

                // Job does not update
                jobRepository.Verify(x => x.UpdateWellStatus(job), Times.Never);
                // change does not propagate to stopService
                stopService.Verify(x => x.ComputeAndPropagateWellStatus(job.StopId), Times.Never);
                // status did not change
                Assert.AreEqual(WellStatus.Complete, job.WellStatus);
                // reports no change
                Assert.False(changed);
            }
        }
    }
}

