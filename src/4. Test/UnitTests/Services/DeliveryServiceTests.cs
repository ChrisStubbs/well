namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Services;

    [TestFixture]
    public class DeliveryServiceTests
    {
        private DeliveryService service;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IJobDetailDamageRepo> jobDetailDamageRepo;
        private Mock<IJobRepository> jobRepo;
        private Mock<IAuditRepository> auditRepo;
        private Mock<IStopRepository> stopRepo;

        [SetUp]
        public void Setup()
        {
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            jobDetailDamageRepo = new Mock<IJobDetailDamageRepo>(MockBehavior.Strict);
            jobRepo = new Mock<IJobRepository>(MockBehavior.Strict);
            auditRepo = new Mock<IAuditRepository>(MockBehavior.Strict);
            stopRepo = new Mock<IStopRepository>(MockBehavior.Strict);

           service = new DeliveryService(
                jobDetailRepository.Object,
                jobDetailDamageRepo.Object,
                jobRepo.Object,
                auditRepo.Object,
                stopRepo.Object);

            jobDetailRepository.SetupSet(x => x.CurrentUser = "user");
            jobDetailDamageRepo.SetupSet(x => x.CurrentUser = "user");
            jobRepo.SetupSet(x => x.CurrentUser = "user");
            auditRepo.SetupSet(a => a.CurrentUser = "user");
            stopRepo.SetupSet(a => a.CurrentUser = "user");
        }

        public class UpdateDeliveryLineTests : DeliveryServiceTests
        {
            [Test]
            public void GivenCleanJob_WhenShortQtyIncreased_ThenJobSetToIncomplete()
            {
                var jobDetail = new JobDetail()
                {
                    Id = 1,
                    LineNumber = 1,
                    ShortQty = 5,
                    JobId = 3
                };

                jobDetailRepository.Setup(j => j.GetByJobId(jobDetail.JobId)).Returns(new List<JobDetail>()
                {
                     new JobDetail() {Id = 1, JobId = 3, LineNumber = 1, ShortQty = 0}
                });

                jobDetailRepository.Setup(j => j.Update(It.IsAny<JobDetail>()));
                jobDetailDamageRepo.Setup(d => d.Delete(jobDetail.Id));

                jobRepo.Setup(j => j.GetById(jobDetail.JobId)).Returns(new Job());
                jobRepo.Setup(j => j.JobCreateOrUpdate(It.IsAny<Job>()));

                jobDetailRepository.Setup(r => r.GetByJobLine(jobDetail.JobId, jobDetail.LineNumber))
                    .Returns(new JobDetail());
                stopRepo.Setup(r => r.GetByJobId(jobDetail.JobId)).Returns(new Stop());
                auditRepo.Setup(a => a.Save(It.IsAny<Audit>()));

                //ACT
                service.UpdateDeliveryLine(jobDetail, "user");

                jobDetailRepository.Verify(j => j.Update(It.Is<JobDetail>(
                    jd => jd.Id == jobDetail.Id &&
                          jd.ShortQty == jobDetail.ShortQty)));

                jobRepo.Verify(j => j.JobCreateOrUpdate(
                    It.Is<Job>(job => job.PerformanceStatus == PerformanceStatus.Incom)));

                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.HasEntry)));
            }

            [Test]
            public void GivenCleanJob_WhenDamageAdded_ThenJobSetToIncomplete()
            {
                var jobDetail = new JobDetail()
                {
                    Id = 1,
                    LineNumber = 1,
                    ShortQty = 0,
                    JobDetailDamages = new Collection<JobDetailDamage>()
                    {
                        new JobDetailDamage() {Qty = 3}
                    },
                    JobId = 3
                };

                jobDetailRepository.Setup(j => j.GetByJobId(jobDetail.JobId)).Returns(new List<JobDetail>()
                {
                    new JobDetail() {Id = 1, JobId = 3, LineNumber = 1, ShortQty = 0}
                });

                jobDetailRepository.Setup(j => j.Update(It.IsAny<JobDetail>()));
                jobDetailDamageRepo.Setup(d => d.Delete(jobDetail.Id));
                jobDetailDamageRepo.Setup(d => d.Save(It.IsAny<JobDetailDamage>()));

                jobRepo.Setup(j => j.GetById(jobDetail.JobId)).Returns(new Job());
                jobRepo.Setup(j => j.JobCreateOrUpdate(It.IsAny<Job>()));

                jobDetailRepository.Setup(r => r.GetByJobLine(jobDetail.JobId, jobDetail.LineNumber))
                   .Returns(new JobDetail());
                stopRepo.Setup(r => r.GetByJobId(jobDetail.JobId)).Returns(new Stop());
                auditRepo.Setup(a => a.Save(It.IsAny<Audit>()));

                //ACT
                service.UpdateDeliveryLine(jobDetail, "user");

                jobDetailRepository.Verify(j => j.Update(It.Is<JobDetail>(
                    jd => jd.Id == jobDetail.Id &&
                          jd.ShortQty == jobDetail.ShortQty)));

                jobDetailDamageRepo.Verify(
                    d => d.Save(It.Is<JobDetailDamage>(jdd => jdd.Qty == jobDetail.JobDetailDamages[0].Qty)));

                jobRepo.Verify(j => j.JobCreateOrUpdate(It.Is<Job>(
                    job => job.PerformanceStatus == PerformanceStatus.Incom)));

                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.HasEntry)));
            }

            [Test]
            public void GivenDirtyJob_WhenShortsAndDamagesRemoved_ThenJobSetToResolved()
            {
                var jobDetail = new JobDetail()
                {
                    Id = 1,
                    LineNumber = 1,
                    ShortQty = 0,
                    JobId = 3
                };

                jobDetailRepository.Setup(j => j.GetByJobId(jobDetail.JobId)).Returns(new List<JobDetail>()
                {
                    new JobDetail()
                    {
                        Id = 1,
                        JobId = 3,
                        LineNumber = 1,
                        ShortQty = 3,
                        JobDetailDamages = new Collection<JobDetailDamage>()
                        {
                            new JobDetailDamage() {Qty = 5}
                        }
                    }
                });

                jobDetailRepository.Setup(j => j.Update(It.IsAny<JobDetail>()));
                jobDetailDamageRepo.Setup(d => d.Delete(jobDetail.Id));

                jobRepo.Setup(j => j.GetById(jobDetail.JobId)).Returns(new Job());
                jobRepo.Setup(j => j.JobCreateOrUpdate(It.IsAny<Job>()));

                jobDetailRepository.Setup(r => r.GetByJobLine(jobDetail.JobId, jobDetail.LineNumber))
                   .Returns(new JobDetail() {ShortQty = 3});
                stopRepo.Setup(r => r.GetByJobId(jobDetail.JobId)).Returns(new Stop());
                auditRepo.Setup(a => a.Save(It.IsAny<Audit>()));

                //ACT
                service.UpdateDeliveryLine(jobDetail, "user");

                jobDetailRepository.Verify(j => j.Update(It.Is<JobDetail>(
                    jd => jd.Id == jobDetail.Id &&
                          jd.ShortQty == jobDetail.ShortQty)));

                jobRepo.Verify(j => j.JobCreateOrUpdate(
                    It.Is<Job>(job => job.PerformanceStatus == PerformanceStatus.Resolved)));

                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.HasEntry)));
            }
        }

    }
}
