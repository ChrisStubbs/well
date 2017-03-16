using PH.Well.Common.Contracts;

namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Moq;
    using NUnit.Framework;

    using PH.Well.Services.Contracts;

    using Repositories.Contracts;
    using Well.Common.Security;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services;

    [TestFixture]
    public class DeliveryServiceTests
    {
        private DeliveryService service;
        private Mock<IJobDetailRepository> jobDetailRepository;
        private Mock<IJobDetailDamageRepository> jobDetailDamageRepo;
        private Mock<IJobRepository> jobRepo;
        private Mock<IAuditRepository> auditRepo;
        private Mock<IStopRepository> stopRepo;
        private Mock<IJobDetailActionRepository> jobDetailActionRepo;
        private Mock<IUserRepository> userRepo;
        private Mock<IExceptionEventRepository> exceptionEventRepo;
        private Mock<IDeliveryReadRepository> deliveryReadRepository;
        private Mock<IBranchRepository> branchRepository;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IJobStatusService> deliveryStatusService;

        [SetUp]
        public void Setup()
        {
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            jobDetailDamageRepo = new Mock<IJobDetailDamageRepository>(MockBehavior.Strict);
            jobRepo = new Mock<IJobRepository>(MockBehavior.Strict);
            auditRepo = new Mock<IAuditRepository>(MockBehavior.Strict);
            stopRepo = new Mock<IStopRepository>(MockBehavior.Strict);
            userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            exceptionEventRepo = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);
            branchRepository =  new Mock<IBranchRepository>(MockBehavior.Strict);
            deliveryStatusService = new Mock<IJobStatusService>(MockBehavior.Strict);

            service = new DeliveryService(jobDetailRepository.Object,
                jobDetailDamageRepo.Object,
                jobRepo.Object,
                auditRepo.Object,
                stopRepo.Object,
                userRepo.Object,
                exceptionEventRepo.Object,
                deliveryReadRepository.Object,
                branchRepository.Object,
                this.deliveryStatusService.Object);
        }

        public class GetExceptionsTests : DeliveryServiceTests
        {
            [Test]
            public void WhenGettingExceptions_ThenExceptionAndCompletedOnPaperStatusesIncluded()
            {
                string user = "Bob";

                deliveryReadRepository.Setup(d => d.GetByStatuses(It.IsAny<string>(),
                    It.IsAny<IList<JobStatus>>())).Returns(new List<Delivery>());

                service.GetExceptions(user);

                deliveryReadRepository.Verify(d => d.GetByStatuses(user,
                    It.Is<IList<JobStatus>>(j => j.Contains(JobStatus.Exception)
                                                 && j.Contains(JobStatus.CompletedOnPaper))), Times.Once);
            }

            [Test]
            public void WhenGettingExceptions_ThenApprovalsAreExcluded()
            {
                string user = "Bob";

                deliveryReadRepository.Setup(d => d.GetByStatuses(It.IsAny<string>(), It.IsAny<IList<JobStatus>>()))
                    .Returns(new List<Delivery>()
                    {
                        new Delivery() {Id = 1, IsPendingCredit = true},
                        new Delivery() {Id = 2, IsPendingCredit = false}
                    });

                var actualDeliveries = service.GetExceptions(user);

                Assert.AreEqual(1, actualDeliveries.Count);
                Assert.AreEqual(2, actualDeliveries[0].Id);
            }
        }

        public class UpdateDeliveryLineTests : DeliveryServiceTests
        {
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
                        JobDetailDamages = new List<JobDetailDamage>()
                        {
                            new JobDetailDamage() {Qty = 5}
                        }
                    }
                });

                jobDetailRepository.Setup(j => j.Update(It.IsAny<JobDetail>()));
                jobDetailDamageRepo.Setup(d => d.Delete(jobDetail.Id));

                var job = new Job { Id = 1, JobStatus = JobStatus.Exception};
                jobRepo.Setup(j => j.GetById(jobDetail.JobId)).Returns(job);
                jobRepo.Setup(j => j.Update(It.IsAny<Job>()));

                jobDetailRepository.Setup(r => r.GetByJobLine(jobDetail.JobId, jobDetail.LineNumber))
                    .Returns(new JobDetail() {ShortQty = 3});
                stopRepo.Setup(r => r.GetByJobId(jobDetail.JobId)).Returns(new Stop());
                auditRepo.Setup(a => a.Save(It.IsAny<Audit>()));

                this.userRepo.Setup(x => x.UnAssignJobToUser(0));

                
                this.branchRepository.Setup(x => x.GetBranchIdForJob(job.Id)).Returns(22);

                this.deliveryStatusService.Setup(x => x.DetermineStatus(job, 22)).Returns(new Job { Id = 1, JobStatus = JobStatus.Clean });

                //ACT
                service.UpdateDeliveryLine(jobDetail, "user");

                jobDetailRepository.Verify(j => j.Update(It.Is<JobDetail>(
                    jd => jd.Id == jobDetail.Id &&
                          jd.ShortQty == jobDetail.ShortQty)));

                jobRepo.Verify(j => j.Update(
                    It.Is<Job>(jo => jo.JobStatus == JobStatus.Resolved)));

                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.HasEntry)));
            }
        }
    }
}
