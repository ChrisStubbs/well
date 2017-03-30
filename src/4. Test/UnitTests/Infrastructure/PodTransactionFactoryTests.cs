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
                new JobDetail { Id = 2, PhProductCode = "22345", ShortQty = 0 , JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 } } }
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
        public void GetPodDeliveryLineCredits_ReturnsPodDeliveryLines()
        {
            var jobDetails = new List<JobDetail>
            {
                new JobDetail { Id = 1, PhProductCode = "12345", ShortQty = 2 },
                new JobDetail { Id = 2, PhProductCode = "22345", ShortQty = 0 , JobDetailDamages = new List<JobDetailDamage> {new JobDetailDamage { Qty = 1 } } }
            };

            this.jobDetailRepository.Setup(x => x.GetByJobId(1)).Returns(jobDetails);

            var podLines = this.factory.GetPodDeliveryLineCredits(1, (int)JobStatus.Exception);

            this.jobDetailRepository.Verify(x => x.GetByJobId(1), Times.Once );

            Assert.That(podLines.Count(), Is.EqualTo(2));
        }
    }
}
