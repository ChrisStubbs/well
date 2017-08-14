namespace PH.Well.UnitTests.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class AmendTransactionFactoryTests
    {
        private Mock<IUserRepository> userRepository;
        private AmendmentFactory factory;

        [SetUp]
        public void Setup()
        {
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.factory = new AmendmentFactory(this.userRepository.Object);
        }

        [Test]
        public void BuildAmendTransaction_ReturnsAmendTransaction()
        {
            var amend = new Amendment
            {
                JobId = 1, AccountNumber = "12345.123", BranchId = 2, InvoiceNumber = "123456789", AmenderName = "Amanda Amender",
                AmendmentLines = new List<AmendmentLine>() { new AmendmentLine {JobId = 1, ProductCode = "56123", DeliveredQuantity = 10, ShortTotal = 0, DamageTotal = 0, RejectedTotal = 0, AmendedDeliveredQuantity = 8, AmendedShortTotal = 1, AmendedRejectedTotal = 0, AmendedDamageTotal = 1} }
            };

            var amanda = new User { Name = "Amanda Amender" };

            this.userRepository.Setup(x => x.GetByIdentity(amend.AmenderName)).Returns(amanda);
            

            var amendTransaction = this.factory.Build(amend);

            this.userRepository.Verify(x => x.GetByIdentity(amend.AmenderName), Times.Once);

            Assert.That(amendTransaction.LineSql.Count, Is.EqualTo(1));
            Assert.That(amendTransaction.BranchId, Is.EqualTo(2));
            Assert.That(amendTransaction.CanWriteHeader, Is.EqualTo(false));
            Assert.That(amendTransaction.HeaderSql, !Is.Empty);

        }

        //[Test]
        //public void GetPodDeliveryLineCredits_ReturnsPodDeliveryLinesForCCE()
        //{
        //    var job = new Job { Id = 1, ProofOfDelivery = 8, PhAccount = "12345", StopId = 1 };
        //    var jobDetails = new List<JobDetail>
        //    {
        //        new JobDetail { Id = 1, JobId = 1, PhProductCode = "12345", ShortQty = 2 , DeliveredQty = 0 , LineItemId = 1},
        //        new JobDetail { Id = 2, JobId = 1, PhProductCode = "22345", ShortQty = 0 , DeliveredQty = 0, LineItemId = 2, JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 , PdaReasonDescription = "Not Required"} } }
        //    };

        //    foreach (var detail in jobDetails)
        //    {
        //        job.JobDetails.Add(detail);
        //    }
        //    //this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);

        //    var lineItems = new List<LineItem>
        //    {
        //        new LineItem
        //        {
        //            Id = 1, ProductCode = "12345", OriginalShortQuantity = 2 ,
        //             LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 2, PdaReasonDescription = "", ExceptionType = ExceptionType.Short}

        //            }
        //        },
        //        new LineItem { Id = 2, ProductCode = "32165", OriginalShortQuantity = 0,
        //            LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 1, PdaReasonDescription = "Not required", ExceptionType = ExceptionType.Damage}

        //            } }
        //    };

        //    foreach (var li in lineItems)
        //    {
        //        job.LineItems.Add(li);
        //    }

        //    var podLines = this.factory.GetPodDeliveryLineCredits(job);

        //    //this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once );

        //    Assert.That(podLines.Count(), Is.EqualTo(2));
        //    var lines = podLines.ToList();
        //    Assert.That(lines[0].JobId, Is.EqualTo(1));
        //    Assert.That(lines[0].Quantity, Is.EqualTo(0));
        //    Assert.That(lines[0].ProductCode, Is.EqualTo("12345"));
        //    Assert.That(lines[0].Reason, Is.EqualTo(2));

        //    Assert.That(lines[1].JobId, Is.EqualTo(1));
        //    Assert.That(lines[1].Quantity, Is.EqualTo(0));
        //    Assert.That(lines[1].ProductCode, Is.EqualTo("32165"));
        //    Assert.That(lines[1].Reason, Is.EqualTo(3));
        //}


        //[Test]
        //public void GetPodDeliveryLineCredits_ReturnsPodDeliveryLinesForLRS()
        //{
        //    var job = new Job { Id = 1, ProofOfDelivery = 1, PhAccount = "12345", StopId = 1 };
        //    var jobDetails = new List<JobDetail>
        //    {
        //        new JobDetail { Id = 1, JobId = 1, PhProductCode = "12345", ShortQty = 2 , DeliveredQty = 0 , LineItemId = 1},
        //        new JobDetail { Id = 2, JobId = 1, PhProductCode = "22345", ShortQty = 0 , DeliveredQty = 0, LineItemId = 2, JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 , PdaReasonDescription = "Not Required"} } },
        //        new JobDetail { Id = 3, JobId = 1, PhProductCode = "32345", ShortQty = 0 , DeliveredQty = 1, LineItemId = 3, JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 , PdaReasonDescription = "Damaged Outer"} } }
        //    };

        //    foreach (var jd in jobDetails)
        //    {
        //        job.JobDetails.Add(jd);
        //    }
        //    //  this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);

        //    var lineItems = new List<LineItem>
        //    {
        //        new LineItem
        //        {
        //            Id = 1, ProductCode = "12345", OriginalShortQuantity = 2 ,
        //             LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 2, PdaReasonDescription = "", ExceptionType = ExceptionType.Short}

        //            }
        //        },
        //        new LineItem { Id = 2, ProductCode = "22345", OriginalShortQuantity = 0,
        //            LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 1, PdaReasonDescription = "Not required", ExceptionType = ExceptionType.Damage}

        //            }
        //        },
        //         new LineItem { Id = 3, ProductCode = "32345", OriginalShortQuantity = 0,
        //            LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 1, PdaReasonDescription = "Damaged Outer", ExceptionType = ExceptionType.Damage}

        //            } }
        //    };

        //    foreach (var li in lineItems)
        //    {
        //        job.LineItems.Add(li);
        //    }


        //    var podLines = this.factory.GetPodDeliveryLineCredits(job);

        //    //this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once);

        //    Assert.That(podLines.Count(), Is.EqualTo(3));
        //    var lines = podLines.ToList();
        //    Assert.That(lines[0].JobId, Is.EqualTo(1));
        //    Assert.That(lines[0].Quantity, Is.EqualTo(0));
        //    Assert.That(lines[0].ProductCode, Is.EqualTo("12345"));
        //    Assert.That(lines[0].Reason, Is.EqualTo(5));

        //    Assert.That(lines[1].JobId, Is.EqualTo(1));
        //    Assert.That(lines[1].Quantity, Is.EqualTo(0));
        //    Assert.That(lines[1].ProductCode, Is.EqualTo("22345"));
        //    Assert.That(lines[1].Reason, Is.EqualTo(3));

        //    Assert.That(lines[2].JobId, Is.EqualTo(1));
        //    Assert.That(lines[2].Quantity, Is.EqualTo(1));
        //    Assert.That(lines[2].ProductCode, Is.EqualTo("32345"));
        //    Assert.That(lines[2].Reason, Is.EqualTo(1));
        //}

        //[Test]
        //[TestCase(ProofOfDelivery.Lucozade, PodReason.UnableToOffload)]
        //[TestCase(ProofOfDelivery.CocaCola, PodReason.DeliveryFailure)]
        //public void GetPodDeliveryLineCredits_ReturnsPodDeliveryLinesBypassed(int proofOfDelivery, int podReason)
        //{

        //    var job = new Job { Id = 1, ProofOfDelivery = proofOfDelivery, PhAccount = "12345", StopId = 1 };

        //    var jobDetails = new List<JobDetail>
        //    {
        //        new JobDetail { Id = 1, JobId = 1, PhProductCode = "12345", ShortQty = 0 , DeliveredQty = 0, LineItemId =1 },
        //        new JobDetail { Id = 2, JobId = 1, PhProductCode = "22345", ShortQty = 0 , DeliveredQty = 0, LineItemId =2 },
        //        new JobDetail { Id = 3, JobId = 1, PhProductCode = "32345", ShortQty = 0 , DeliveredQty = 0, LineItemId =3 }
        //    };

        //    foreach (var jd in jobDetails)
        //    {
        //        job.JobDetails.Add(jd);
        //    }

        //    var lineItems = new List<LineItem>
        //    {
        //        new LineItem
        //        {
        //            Id = 1, ProductCode = "12345", OriginalShortQuantity = 0,
        //             LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 2, PdaReasonDescription = "", ExceptionType = ExceptionType.Bypass}

        //            }
        //        },
        //        new LineItem { Id = 2, ProductCode = "22345", OriginalShortQuantity = 0,
        //            LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 1, PdaReasonDescription = "", ExceptionType = ExceptionType.Bypass}

        //            }
        //        },
        //         new LineItem { Id = 3, ProductCode = "32345", OriginalShortQuantity = 0,
        //            LineItemActions = new List<LineItemAction>
        //            {
        //                new LineItemAction { Quantity = 1, PdaReasonDescription = "", ExceptionType = ExceptionType.Bypass}

        //            } }
        //    };

        //    foreach (var li in lineItems)
        //    {
        //        job.LineItems.Add(li);
        //    }

        //    // this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);

        //    var podLines = this.factory.GetPodDeliveryLineCredits(job);

        //  //  this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once);

        //    Assert.That(podLines.Count(), Is.EqualTo(3));
        //    var lines = podLines.ToList();
        //    Assert.That(lines[0].JobId, Is.EqualTo(1));
        //    Assert.That(lines[0].Quantity, Is.EqualTo(0));
        //    Assert.That(lines[0].ProductCode, Is.EqualTo("12345"));
        //    Assert.That(lines[0].Reason, Is.EqualTo(podReason));

        //    Assert.That(lines[1].JobId, Is.EqualTo(1));
        //    Assert.That(lines[1].Quantity, Is.EqualTo(0));
        //    Assert.That(lines[1].ProductCode, Is.EqualTo("22345"));
        //    Assert.That(lines[1].Reason, Is.EqualTo(podReason));

        //    Assert.That(lines[2].JobId, Is.EqualTo(1));
        //    Assert.That(lines[2].Quantity, Is.EqualTo(0));
        //    Assert.That(lines[2].ProductCode, Is.EqualTo("32345"));
        //    Assert.That(lines[2].Reason, Is.EqualTo(podReason));
        //}
    }
}
