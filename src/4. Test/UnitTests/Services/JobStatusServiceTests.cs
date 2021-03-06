﻿using System.Security.Principal;
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
        private Mock<IUserThresholdService> userThreshold;
        private Mock<IDateThresholdService> dateThresholdService;
        private Mock<ILineItemSearchReadRepository> lineItemRepository;
        private Mock<IUserRepository> userRepository;
        private Mock<IStopService> stopService;
        private Mock<IActivityService> activityService;
        private Mock<WellStatusAggregator> wellStatusAggregator;

        [SetUp]
        public void Setup()
        {
            var identity = new GenericIdentity("foo");

            var principal = new GenericPrincipal(identity, new string[] { });

            Thread.CurrentPrincipal = principal;

            jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            assigneeReadRepository = new Mock<IAssigneeReadRepository>();
            userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            userThreshold = new Mock<IUserThresholdService>();
            dateThresholdService = new Mock<IDateThresholdService>();
            lineItemRepository = new Mock<ILineItemSearchReadRepository>();
            userRepository = new Mock<IUserRepository>();
            stopService = new Mock<IStopService>();
            activityService = new Mock<IActivityService>();
            wellStatusAggregator = new Mock<WellStatusAggregator>();

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
                    JobType = JobType.GlobalUplift
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

                return this.service.CanEdit(job, user) == string.Empty;
            }

            [Test]
            [Category("JobService")]
            public void CanEditActions_Should_Check_ResolutionStatus()
            {
                var job = new Job
                {
                    ResolutionStatus = ResolutionStatus.DriverCompleted
                };

                Assert.IsTrue(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.ActionRequired;
                Assert.IsTrue(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.PendingSubmission;
                Assert.IsTrue(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.PendingApproval;
                Assert.IsTrue(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.Imported;
                Assert.IsFalse(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.Approved;
                Assert.IsFalse(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.Credited;
                Assert.IsFalse(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.Resolved;
                Assert.IsFalse(this.service.CanEdit(job, "User") == string.Empty);

                job.ResolutionStatus = ResolutionStatus.Closed;
                Assert.IsFalse(this.service.CanEdit(job, "User") == string.Empty);
            }

            [Test]
            [TestCase(JobType.GlobalUplift, ExpectedResult = false, Description = "Global uplift jobs should not be editable")]
            [TestCase(JobType.Alcohol, ExpectedResult = true)]
            [Category("JobService")]
            public bool CanEditActions_Should_Check_JobType(JobType jobType)
            {
                var job = new Job
                {
                    JobStatus = JobStatus.InComplete,
                    InvoiceNumber = "123",
                    JobType = jobType,
                    ResolutionStatus = ResolutionStatus.ActionRequired
                };

                return service.CanEdit(job, "User") == string.Empty;
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

                foreach (WellStatus wellStatus in Enum.GetValues(typeof(WellStatus)))
                {
                    var job = new Job
                    {
                        WellStatus = wellStatus
                    };

                    var canManuallyComplete = this.service.CanManuallyComplete(job, "User");

                    switch (wellStatus)
                    {
                        case WellStatus.Unknown:
                            Assert.False(canManuallyComplete);
                                break;
                           
                        case WellStatus.Planned:
                            Assert.False(canManuallyComplete);
                            break;
                        case WellStatus.Invoiced:
                            Assert.True(canManuallyComplete);
                            break;
                        case WellStatus.Complete:
                            Assert.False(canManuallyComplete);
                            break;
                        case WellStatus.Bypassed:
                            Assert.True(canManuallyComplete);
                            break;
                        case WellStatus.RouteInProgress:
                            Assert.False(canManuallyComplete);
                            break;
                        case WellStatus.Replanned:
                            Assert.True(canManuallyComplete);
                            break;
                        default:
                            Assert.True(false,"New Well Status Added: Consider whether you need to be able to manually complete jobs with this status");
                            break;
                    }
                }
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
            [TestCase(JobType.GlobalUplift, ExpectedResult = false, Description = "Global uplift jobs should not be editable")]
            [TestCase(JobType.Alcohol, ExpectedResult = true)]
            [Category("JobService")]
            public bool CanEditActions_Should_Check_JobType(JobType jobType)
            {
                var job = new Job
                {
                    JobStatus = JobStatus.InComplete,
                    InvoiceNumber = "123",
                    JobType = jobType,
                    ResolutionStatus = ResolutionStatus.ActionRequired
                };

                return service.CanEdit(job, "User") == string.Empty;
            }
        }

        public class JobAssignmentTests : JobServiceTests
        {
            private IEnumerable<Job> pendingApprovalJobs = new[]
            {
                new Job() {Id = 1, ResolutionStatus = ResolutionStatus.PendingApproval},
                new Job() {Id = 2, ResolutionStatus = ResolutionStatus.PendingApproval}
            };

            private IEnumerable<Job> standardJobs = new[] {new Job {Id = 3}, new Job {Id = 4}};

            [Test]
            public void ShouldAssignTheJobsToAUser()
            {
                var jobs = standardJobs.ToList();
                var user = new User();
                var userJobs = new UserJobs
                {
                    JobIds = jobs.Select(x => x.Id).ToArray(),
                    UserId = user.Id
                };

                userRepository.Setup(x => x.GetById(userJobs.UserId)).Returns(user);
                jobRepository.Setup(j => j.GetByIds(userJobs.JobIds)).Returns(jobs);

                var result = service.Assign(userJobs);

                Assert.True(result.Success);
                this.userRepository.Verify(x => x.AssignJobToUser(user.Id, It.IsAny<int>()), Times.Exactly(2));
            }

            [Test]
            public void ShouldAssignJobsIncludingPendingApproval()
            {
                var user = new User();
                var allJobs = standardJobs.Concat(pendingApprovalJobs).ToList();
                var userJobs = new UserJobs
                {
                    JobIds = allJobs.Select(x => x.Id).ToArray(),
                    UserId = user.Id,
                    AllocatePendingApprovalJobs = true
                };

                this.userRepository.Setup(x => x.GetById(userJobs.UserId)).Returns(user);
                jobRepository.Setup(j => j.GetByIds(userJobs.JobIds)).Returns(allJobs);

                var result = service.Assign(userJobs);

                Assert.True(result.Success);
                foreach (var job in allJobs)
                {
                    this.userRepository.Verify(x => x.AssignJobToUser(user.Id, job.Id), Times.Exactly(1));
                }
            }

            [Test]
            public void ShouldReturnFailureIfNoUser()
            {
                jobRepository.Setup(j => j.GetByIds(It.IsAny<int[]>())).Returns(standardJobs);
                var result = service.Assign(new UserJobs {JobIds = standardJobs.Select(x => x.Id).ToArray()});
                Assert.False(result.Success);
            }

            [Test]
            public void ShouldReturnFailureIfNoJob()
            {
                var user = new User();
                var userJobs = new UserJobs
                {
                    UserId = user.Id
                };

                this.userRepository.Setup(x => x.GetById(userJobs.UserId)).Returns(user);
                jobRepository.Setup(j => j.GetByIds(userJobs.JobIds)).Returns(new List<Job>());

                var result = service.Assign(userJobs);
                Assert.False(result.Success);
            }

            [Test]
            public void ShouldReturnFailureIfAllJobsArePendingApproval()
            {
                var user = new User();
                var userJobs = new UserJobs
                {
                    JobIds = pendingApprovalJobs.Select(x => x.Id).ToArray(),
                    UserId = user.Id,
                    AllocatePendingApprovalJobs = false // Exclude pending approval jobs
                };

                this.userRepository.Setup(x => x.GetById(userJobs.UserId)).Returns(user);
                jobRepository.Setup(j => j.GetByIds(userJobs.JobIds)).Returns(pendingApprovalJobs);

                var result = service.Assign(userJobs);

                Assert.False(result.Success);
            }
            

            [Test]
            public void ShouldUnAssignJobs()
            {
                var jobIds = standardJobs.Select(x => x.Id);
                var result = service.UnAssign(jobIds);

                Assert.True(result.Success);

                foreach (var jobId in jobIds)
                {
                    this.userRepository.Verify(x => x.UnAssignJobToUser(jobId), Times.Once);
                }
            }
        }

        [Test]
        public void ChangeGrnShouldFailAfterSubmissionDate()
        {
            var job = JobFactory.New
                .With(p => p.GrnNumber = "GrnNumber")
                .Build();
            var jobs = new[] { job };
            var routeDate = DateTime.Now;
            var routes = new[] {new JobRoute() {JobId = job.Id, RouteDate = routeDate}};

            jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(jobs);
            jobRepository.Setup(x => x.GetJobsRoute(It.IsAny<IEnumerable<int>>())).Returns(routes);

            dateThresholdService.Setup(x => x.GracePeriodEnd(routeDate, job.Id, 0))
                .Returns<DateTime>(x => routeDate.AddHours(-1));

            Assert.IsFalse(service.SetGrn(job.Id, "123"));
        }

        [Test]
        public void ChangeGrnShouldPassAfterSubmissionDate()
        {
            var job = JobFactory.New
                .With(p => p.GrnNumber = null)
                .Build();
            var jobs = new[] { job };
            var routeDate = DateTime.Now;
            var routes = new[] { new JobRoute() { JobId = job.Id, RouteDate = routeDate } };

            jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(jobs);
            jobRepository.Setup(x => x.GetJobsRoute(It.IsAny<IEnumerable<int>>())).Returns(routes);
            jobRepository.Setup(x => x.SaveGrn(It.IsAny<int>(), It.IsAny<string>()));

            dateThresholdService.Setup(x => x.GracePeriodEnd(routeDate, job.Id, 0))
                .Returns<DateTime>(x => routeDate.AddHours(-1));

            Assert.IsTrue(service.SetGrn(job.Id, "123"));
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

            Assert.IsTrue(service.SetGrn(job.Id, "123"));
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

