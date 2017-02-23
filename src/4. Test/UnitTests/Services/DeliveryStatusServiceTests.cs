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

    [TestFixture]
    public class DeliveryStatusServiceTests
    {
        private Mock<IJobRepository> jobRepository;

        private DeliveryStatusService service;

        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public void Setup()
        {
            var identity = new GenericIdentity("foo");

            var principal = new GenericPrincipal(identity, new string[] {});

            Thread.CurrentPrincipal = principal;

            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);

            //////this.jobRepository.SetupSet(x => x.CurrentUser = "foo");

            this.service = new DeliveryStatusService(this.jobRepository.Object);

            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
        }

        public class TheSetStatusMethod : DeliveryStatusServiceTests
        {
            /// <summary>
            /// Multiple jobs with same product and delivered QTY > despatched/invoiced QTY
            /// </summary>
            [Test]
            public void ShouldSetAllJobsToException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New.With(x=>x.InvoiceNumber = invoiceNumber).Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();
                
                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                this.jobRepository.Setup(x => x.Update(job2));
                this.jobRepository.Setup(x => x.Update(job3));

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x=>x.OriginalDespatchQty = 10)
                    .Build();

                var jobDetail2 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.DeliveredQty = 6)
                    .Build();

                var jobDetail3 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x=>x.DeliveredQty = 5)
                    .Build();

                job1.JobDetails.Add(jobDetail1);
                job2.JobDetails.Add(jobDetail2);
                job3.JobDetails.Add(jobDetail3);

                this.service.SetStatus(job1, branchNo);
                Assert.IsTrue(job1.HasException);
                Assert.IsTrue(job2.HasException);
                Assert.IsTrue(job3.HasException);

                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber), Times.Once);
                this.jobRepository.Verify(x => x.Update(job2), Times.Once);
                this.jobRepository.Verify(x => x.Update(job3), Times.Once);
            }

            /// <summary>
            /// Multiple jobs with same product and delivered QTY == despatched/invoiced QTY
            /// </summary>
            [Test]
            public void QtyEqualShouldSetAllJobsToNotException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New.With(x => x.InvoiceNumber = invoiceNumber).Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber))
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

                this.service.SetStatus(job1, branchNo);
                Assert.IsFalse(job1.HasException);
                Assert.IsFalse(job2.HasException);
                Assert.IsFalse(job3.HasException);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber), Times.Once);
            }

            /// <summary>
            /// Multiple jobs with same product and delivered QTY < despatched/invoiced QTY
            /// </summary>
            [Test]
            public void QtyLessThanShouldSetAllJobsToNotException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New.With(x => x.InvoiceNumber = invoiceNumber).Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber))
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

                this.service.SetStatus(job1, branchNo);
                Assert.IsFalse(job1.HasException);
                Assert.IsFalse(job2.HasException);
                Assert.IsFalse(job3.HasException);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber), Times.Once);
            }

            /// <summary>
            /// A job has shorts QTY > 0 
            /// </summary>
            [Test]
            public void ShortQtyGreaterThanShouldSetException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x=>x.ShortQty = 1)
                    .Build();

                job1.JobDetails.Add(jobDetail1);

                this.service.SetStatus(job1, branchNo);
                Assert.IsTrue(job1.HasException);
                Assert.IsFalse(job2.HasException);
                Assert.IsFalse(job3.HasException);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber), Times.Once);
            }

            /// <summary>
            /// A job has shorts QTY > 0 
            /// </summary>
            [Test]
            public void ShortQtyZeroShouldNotSetException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.ShortQty = 0)
                    .Build();
                jobDetail1.JobDetailDamages.Clear();
                job1.JobDetails.Add(jobDetail1);

                this.service.SetStatus(job1, branchNo);
                Assert.IsFalse(job1.HasException);
                Assert.IsFalse(job2.HasException);
                Assert.IsFalse(job3.HasException);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber), Times.Once);
            }

            /// <summary>
            /// A job has shorts QTY > 0 
            /// </summary>
            [Test]
            public void DamagesShouldSetException()
            {
                var branchNo = 55;
                var invoiceNumber = "12345678";

                var job1 = JobFactory.New
                    .With(x => x.InvoiceNumber = invoiceNumber)
                    .Build();
                var job2 = JobFactory.New.Build();
                var job3 = JobFactory.New.Build();

                this.jobRepository.Setup(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber))
                    .Returns(new List<Job>() { job2, job3 });

                // Use duplicate product for all jobs
                var jobDetail1 = JobDetailFactory.New
                    .With(x => x.PhProductCode = "2001")
                    .With(x => x.OriginalDespatchQty = 10)
                    .With(x => x.ShortQty = 0)
                    .Build();
                job1.JobDetails.Add(jobDetail1);

                jobDetail1.JobDetailDamages.Add(new JobDetailDamage());

                this.service.SetStatus(job1, branchNo);
                Assert.IsTrue(job1.HasException);
                Assert.IsFalse(job2.HasException);
                Assert.IsFalse(job3.HasException);
                this.jobRepository.Verify(x => x.GetJobsByBranchAndInvoiceNumber(branchNo, invoiceNumber), Times.Once);
            }
        }
    }
}
