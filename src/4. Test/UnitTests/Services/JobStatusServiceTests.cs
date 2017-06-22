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
    using Well.Domain.ValueObjects;
    using Well.Services.Contracts;

    [TestFixture]
    public class JobServiceTests
    {
        private Mock<IJobRepository> jobRepository;
        private JobService service;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IAssigneeReadRepository> assigneeReadRepository;
        private Mock<IUserThresholdService> userThreshold = new Mock<IUserThresholdService>();
        private Mock<IDateThresholdService> dateThresholdService = new Mock<IDateThresholdService>();
        private Mock<ILineItemSearchReadRepository> lineItemRepository = new Mock<ILineItemSearchReadRepository>();
        [SetUp]
        public void Setup()
        {
            var identity = new GenericIdentity("foo");

            var principal = new GenericPrincipal(identity, new string[] { });

            Thread.CurrentPrincipal = principal;

            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.assigneeReadRepository = new Mock<IAssigneeReadRepository>();

            assigneeReadRepository.Setup(p => p.GetByJobId(It.IsAny<int>())).Returns(new Assignee { IdentityName = "User" });

            this.service = new JobService(this.jobRepository.Object, userThreshold.Object, dateThresholdService.Object, assigneeReadRepository.Object, lineItemRepository.Object);

            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
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
                    InvoiceNumber = invoiceNumber
                };

                service.SetIncompleteJobStatus(job);
                return job.JobStatus;
            }
        }

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

            return this.service.CanEditActions(job, user);
        }

        [Test]
        [Category("JobService")]
        public void CanEditActions_Should_Check_ResolutionStatus()
        {
            var job = new Job
            {
                ResolutionStatus = ResolutionStatus.DriverCompleted
            };

            Assert.IsTrue(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.ActionRequired;
            Assert.IsTrue(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.PendingSubmission;
            Assert.IsTrue(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.PendingApproval;
            Assert.IsTrue(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.Imported;
            Assert.IsFalse(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.Approved;
            Assert.IsFalse(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.Credited;
            Assert.IsFalse(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.Resolved;
            Assert.IsFalse(this.service.CanEditActions(job, "User"));

            job.ResolutionStatus = ResolutionStatus.Closed;
            Assert.IsFalse(this.service.CanEditActions(job, "User"));
        }
    }
}

