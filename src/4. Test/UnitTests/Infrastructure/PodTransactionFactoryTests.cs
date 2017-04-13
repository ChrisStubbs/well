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
    public class PodTransactionFactoryTests
    {
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IAccountRepository> accountRepository;
        private PodTransactionFactory factory;

        [SetUp]
        public void Setup()
        {
            this.jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            this.accountRepository = new Mock<IAccountRepository>(MockBehavior.Strict);
            this.factory = new PodTransactionFactory(this.accountRepository.Object, this.jobDetailRepository.Object);
        }

        [Test]
        public void BuildPodTransaction_ReturnsPodTransaction()
        {
            var job = new Job { Id = 1 , ProofOfDelivery = 8, PhAccount = "12345", StopId = 1};

            var jobDetails = new List<JobDetail>
            {
                new JobDetail { Id = 1, PhProductCode = "12345", ShortQty = 2 },
                new JobDetail { Id = 2, PhProductCode = "22345", ShortQty = 0 , JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 , PdaReasonDescription = "Damaged Outer"} } }
            };

            var account = new Account { Id = 1, Code = "12345", ContactName = "Donald"};

            this.accountRepository.Setup(x => x.GetAccountByStopId(1)).Returns(account);

            this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);
            
            var podLines = new List<PodDeliveryLineCredit>
            {
                new PodDeliveryLineCredit { JobId = 1, ProductCode = "12345", Quantity = 2, Reason = 1, Source = 1},
                new PodDeliveryLineCredit { JobId = 1, ProductCode = "22345", Quantity = 3, Reason = 2, Source = 2}
            };

            var podTransaction = this.factory.Build(job, 22);

            this.accountRepository.Verify(x => x.GetAccountByStopId(1), Times.Once);
            this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once);

            Assert.That(podTransaction.LineSql.Count, Is.EqualTo(2));
            Assert.That(podTransaction.BranchId, Is.EqualTo(22));
            Assert.That(podTransaction.CanWriteHeader, Is.EqualTo(false));
            Assert.That(podTransaction.HeaderSql, !Is.Empty);

        }

        [Test]
        public void GetPodDeliveryLineCredits_ReturnsPodDeliveryLinesForCCE()
        {
            var jobDetails = new List<JobDetail>
            {
                new JobDetail { Id = 1, JobId = 1, PhProductCode = "12345", ShortQty = 2 , DeliveredQty = 0 },
                new JobDetail { Id = 2, JobId = 1, PhProductCode = "22345", ShortQty = 0 , DeliveredQty = 0, JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 , PdaReasonDescription = "Not Required"} } }
            };

            this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);

            var podLines = this.factory.GetPodDeliveryLineCredits(1, (int)JobStatus.Exception, (int)ProofOfDelivery.CocaCola);

            this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once );

            Assert.That(podLines.Count(), Is.EqualTo(2));
            var lines = podLines.ToList();
            Assert.That(lines[0].JobId, Is.EqualTo(1));
            Assert.That(lines[0].Quantity, Is.EqualTo(0));
            Assert.That(lines[0].ProductCode, Is.EqualTo("12345"));
            Assert.That(lines[0].Reason, Is.EqualTo(2));

            Assert.That(lines[1].JobId, Is.EqualTo(1));
            Assert.That(lines[1].Quantity, Is.EqualTo(0));
            Assert.That(lines[1].ProductCode, Is.EqualTo("22345"));
            Assert.That(lines[1].Reason, Is.EqualTo(3));
        }


        [Test]
        public void GetPodDeliveryLineCredits_ReturnsPodDeliveryLinesForLRS()
        {
            var jobDetails = new List<JobDetail>
            {
                new JobDetail { Id = 1, JobId = 1, PhProductCode = "12345", ShortQty = 2 , DeliveredQty = 0 },
                new JobDetail { Id = 2, JobId = 1, PhProductCode = "22345", ShortQty = 0 , DeliveredQty = 0, JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 , PdaReasonDescription = "Not Required"} } },
                new JobDetail { Id = 3, JobId = 1, PhProductCode = "32345", ShortQty = 0 , DeliveredQty = 1, JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 , PdaReasonDescription = "Damaged Outer"} } }
            };

            this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);

            var podLines = this.factory.GetPodDeliveryLineCredits(1, (int)JobStatus.Exception, (int)ProofOfDelivery.Lucozade);

            this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once);

            Assert.That(podLines.Count(), Is.EqualTo(3));
            var lines = podLines.ToList();
            Assert.That(lines[0].JobId, Is.EqualTo(1));
            Assert.That(lines[0].Quantity, Is.EqualTo(0));
            Assert.That(lines[0].ProductCode, Is.EqualTo("12345"));
            Assert.That(lines[0].Reason, Is.EqualTo(5));

            Assert.That(lines[1].JobId, Is.EqualTo(1));
            Assert.That(lines[1].Quantity, Is.EqualTo(0));
            Assert.That(lines[1].ProductCode, Is.EqualTo("22345"));
            Assert.That(lines[1].Reason, Is.EqualTo(3));

            Assert.That(lines[2].JobId, Is.EqualTo(1));
            Assert.That(lines[2].Quantity, Is.EqualTo(1));
            Assert.That(lines[2].ProductCode, Is.EqualTo("32345"));
            Assert.That(lines[2].Reason, Is.EqualTo(1));
        }

        [Test]
        [TestCase(ProofOfDelivery.Lucozade, PodReason.UnableToOffload)]
        [TestCase(ProofOfDelivery.CocaCola, PodReason.DeliveryFailure)]
        public void GetPodDeliveryLineCredits_ReturnsPodDeliveryLinesBypassed(int proofOfDelivery, int podReason)
        {
            var jobDetails = new List<JobDetail>
            {
                new JobDetail { Id = 1, JobId = 1, PhProductCode = "12345", ShortQty = 0 , DeliveredQty = 0 },
                new JobDetail { Id = 2, JobId = 1, PhProductCode = "22345", ShortQty = 0 , DeliveredQty = 0 },
                new JobDetail { Id = 3, JobId = 1, PhProductCode = "32345", ShortQty = 0 , DeliveredQty = 0 }
            };

            this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);

            var podLines = this.factory.GetPodDeliveryLineCredits(1, (int)JobStatus.Bypassed, proofOfDelivery);

            this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once);

            Assert.That(podLines.Count(), Is.EqualTo(3));
            var lines = podLines.ToList();
            Assert.That(lines[0].JobId, Is.EqualTo(1));
            Assert.That(lines[0].Quantity, Is.EqualTo(0));
            Assert.That(lines[0].ProductCode, Is.EqualTo("12345"));
            Assert.That(lines[0].Reason, Is.EqualTo(podReason));

            Assert.That(lines[1].JobId, Is.EqualTo(1));
            Assert.That(lines[1].Quantity, Is.EqualTo(0));
            Assert.That(lines[1].ProductCode, Is.EqualTo("22345"));
            Assert.That(lines[1].Reason, Is.EqualTo(podReason));

            Assert.That(lines[2].JobId, Is.EqualTo(1));
            Assert.That(lines[2].Quantity, Is.EqualTo(0));
            Assert.That(lines[2].ProductCode, Is.EqualTo("32345"));
            Assert.That(lines[2].Reason, Is.EqualTo(podReason));
        }
    }
}
