namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Moq;
    using NUnit.Framework;
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

        [SetUp]
        public void Setup()
        {
            jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            jobDetailDamageRepo = new Mock<IJobDetailDamageRepository>(MockBehavior.Strict);
            jobRepo = new Mock<IJobRepository>(MockBehavior.Strict);
            auditRepo = new Mock<IAuditRepository>(MockBehavior.Strict);
            stopRepo = new Mock<IStopRepository>(MockBehavior.Strict);
            jobDetailActionRepo = new Mock<IJobDetailActionRepository>(MockBehavior.Strict);
            userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            exceptionEventRepo = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            deliveryReadRepository = new Mock<IDeliveryReadRepository>(MockBehavior.Strict);
            branchRepository =  new Mock<IBranchRepository>(MockBehavior.Strict);

            service = new DeliveryService(jobDetailRepository.Object,
                jobDetailDamageRepo.Object,
                jobRepo.Object,
                auditRepo.Object,
                stopRepo.Object,
                jobDetailActionRepo.Object,
                userRepo.Object,
                exceptionEventRepo.Object,
                deliveryReadRepository.Object,
                branchRepository.Object);

            jobDetailRepository.SetupSet(x => x.CurrentUser = "user");
            jobDetailDamageRepo.SetupSet(x => x.CurrentUser = "user");
            jobRepo.SetupSet(x => x.CurrentUser = "user");
            auditRepo.SetupSet(a => a.CurrentUser = "user");
            stopRepo.SetupSet(a => a.CurrentUser = "user");
            jobDetailActionRepo.SetupSet(a => a.CurrentUser = "user");
            exceptionEventRepo.SetupSet(a => a.CurrentUser = "user");
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
                jobRepo.Setup(j => j.Update(It.IsAny<Job>()));

                jobDetailRepository.Setup(r => r.GetByJobLine(jobDetail.JobId, jobDetail.LineNumber))
                    .Returns(new JobDetail());
                stopRepo.Setup(r => r.GetByJobId(jobDetail.JobId)).Returns(new Stop());
                auditRepo.Setup(a => a.Save(It.IsAny<Audit>()));

                //ACT
                service.UpdateDeliveryLine(jobDetail, "user");

                jobDetailRepository.Verify(j => j.Update(It.Is<JobDetail>(
                    jd => jd.Id == jobDetail.Id &&
                          jd.ShortQty == jobDetail.ShortQty)));

                jobRepo.Verify(j => j.Update(
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
                    JobDetailDamages = new List<JobDetailDamage>()
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
                jobRepo.Setup(j => j.Update(It.IsAny<Job>()));

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

                jobRepo.Verify(j => j.Update(It.Is<Job>(
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
                        JobDetailDamages = new List<JobDetailDamage>()
                        {
                            new JobDetailDamage() {Qty = 5}
                        }
                    }
                });

                jobDetailRepository.Setup(j => j.Update(It.IsAny<JobDetail>()));
                jobDetailDamageRepo.Setup(d => d.Delete(jobDetail.Id));

                jobRepo.Setup(j => j.GetById(jobDetail.JobId)).Returns(new Job());
                jobRepo.Setup(j => j.Update(It.IsAny<Job>()));

                jobDetailRepository.Setup(r => r.GetByJobLine(jobDetail.JobId, jobDetail.LineNumber))
                    .Returns(new JobDetail() {ShortQty = 3});
                stopRepo.Setup(r => r.GetByJobId(jobDetail.JobId)).Returns(new Stop());
                auditRepo.Setup(a => a.Save(It.IsAny<Audit>()));

                this.userRepo.Setup(x => x.UnAssignJobToUser(0));

                //ACT
                service.UpdateDeliveryLine(jobDetail, "user");

                jobDetailRepository.Verify(j => j.Update(It.Is<JobDetail>(
                    jd => jd.Id == jobDetail.Id &&
                          jd.ShortQty == jobDetail.ShortQty)));

                jobRepo.Verify(j => j.Update(
                    It.Is<Job>(job => job.PerformanceStatus == PerformanceStatus.Resolved)));

                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.HasEntry)));
            }
        }

        public class GivenAmendedActions_WhenUpdatingDeliveryLineActions : DeliveryServiceTests
        {
            private JobDetail jobDetailUpdates;

            public void CommonArrangeAndAct()
            {
                jobDetailUpdates = new JobDetail
                {
                    Id = 5,
                    Actions = new List<JobDetailAction>(new List<JobDetailAction>()
                    {
                        new JobDetailAction()
                        {
                            JobDetailId = 5,
                            Action = EventAction.Reject,
                            Quantity = 2,
                            Status = ActionStatus.Draft
                        },
                        new JobDetailAction()
                        {
                            JobDetailId = 5,
                            Action = EventAction.CreditAndReorder,
                            Quantity = 3,
                            Status = ActionStatus.Draft
                        }
                    })
                };

                jobDetailActionRepo.Setup(j => j.DeleteDrafts(It.IsAny<int>()));

                jobDetailActionRepo.Setup(j => j.Save(It.IsAny<JobDetailAction>()));

                jobRepo.Setup(jr => jr.GetById(It.IsAny<int>())).Returns(new Job());
                jobDetailRepository.Setup(jdr => jdr.GetByJobLine(It.IsAny<int>(), It.IsAny<int>())).Returns(new JobDetail());
                stopRepo.Setup(s => s.GetByJobId(It.IsAny<int>())).Returns(new Stop());

                auditRepo.Setup(r => r.Save(It.IsAny<Audit>()));

                //ACT
                service.UpdateDraftActions(jobDetailUpdates, "user");
            }

            [Test]
            public void ThenExistingDraftActionsDeleted()
            {
                CommonArrangeAndAct();

                jobDetailActionRepo.Verify(j => j.DeleteDrafts(It.Is<int>(i => i == jobDetailUpdates.Id)));
            }

            [Test]
            public void ThenAmendedDraftActionsSaved()
            {
                CommonArrangeAndAct();

                jobDetailActionRepo.Verify(j => j.Save(
                    It.Is<JobDetailAction>(a => a.JobDetailId == 5 &&
                                                a.Action == EventAction.Reject &&
                                                a.Quantity == 2 &&
                                                a.Status == ActionStatus.Draft)), Times.Once);
                jobDetailActionRepo.Verify(j => j.Save(
                    It.Is<JobDetailAction>(a => a.JobDetailId == 5 &&
                                                a.Action ==
                                                EventAction.CreditAndReorder &&
                                                a.Quantity == 3 &&
                                                a.Status == ActionStatus.Draft)), Times.Once);
               
                jobDetailActionRepo.VerifySet(a => a.CurrentUser = "user");
            }                          

            [Test]
            public void ThenAuditCreatedAndSaved()
            {
                CommonArrangeAndAct();

                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.HasEntry == true)));
                auditRepo.VerifySet(a => a.CurrentUser = "user");
            }
        }

        public class WhenSubmittingActions : DeliveryServiceTests
        {
            private List<JobDetail> GetOrigJobDetails()
            {
                return new List<JobDetail>()
                {
                    new JobDetail()
                    {
                        Id = 1, PhProductCode = "123", ProdDesc = "Cheesy Chips",
                        Actions =
                            new List<JobDetailAction>(new List<JobDetailAction>()
                            {
                                new JobDetailAction()
                                {
                                    JobDetailId = 1,
                                    Action = EventAction.Credit,
                                    Quantity = 1,
                                    Status = ActionStatus.Draft
                                },
                                new JobDetailAction()
                                {
                                    JobDetailId = 1,
                                    Action = EventAction.CreditAndReorder,
                                    Quantity = 2,
                                    Status = ActionStatus.Submitted
                                }
                            })
                    },
                    new JobDetail()
                    {
                        Id = 2, PhProductCode = "456", ProdDesc = "Dirty Burger",
                        Actions =
                            new List<JobDetailAction>(new List<JobDetailAction>()
                            {
                                new JobDetailAction()
                                {
                                    JobDetailId = 2,
                                    Action = EventAction.ReplanInRoadnet,
                                    Quantity = 3,
                                    Status = ActionStatus.Draft
                                }
                            })
                    }
                };
            }

            private void CommonArrangeAct()
            {
                int deliveryId = 1;

                jobDetailRepository.Setup(r => r.GetByJobId(deliveryId)).Returns(GetOrigJobDetails());

                jobDetailActionRepo.Setup(r => r.Update(It.IsAny<JobDetailAction>()));

                jobRepo.Setup(jr => jr.GetById(It.IsAny<int>())).Returns(new Job());
                jobDetailRepository.Setup(jdr => jdr.GetById(1)).Returns(GetOrigJobDetails()[0]);
                jobDetailRepository.Setup(jdr => jdr.GetById(2)).Returns(GetOrigJobDetails()[1]);
                stopRepo.Setup(s => s.GetByJobId(It.IsAny<int>())).Returns(new Stop());
                auditRepo.Setup(r => r.Save(It.IsAny<Audit>()));

                //ACT
                service.SubmitActions(deliveryId, "user");
            }

            [Test]
            public void ThenAllDraftActionsSetToSubmitted()
            {
                CommonArrangeAct();

                jobDetailActionRepo.VerifySet(r => r.CurrentUser = "user");

                jobDetailActionRepo.Verify(r => r.Update(It.Is<JobDetailAction>(a => a.JobDetailId == 1 &&
                                                                                     a.Action == EventAction.Credit &&
                                                                                     a.Quantity == 1 &&
                                                                                     a.Status == ActionStatus.Submitted)),Times.Once);

                jobDetailActionRepo.Verify(r => r.Update(It.Is<JobDetailAction>(a => a.JobDetailId == 2 &&
                                                                                     a.Action ==
                                                                                     EventAction.ReplanInRoadnet &&
                                                                                     a.Quantity == 3 &&
                                                                                     a.Status == ActionStatus.Submitted)),Times.Once);

            }

            [Test]
            public void ThenAuditsCreatedAndSaved()
            {
                CommonArrangeAct();

                var submittedJobDetails = new List<JobDetail>()
                {
                    new JobDetail()
                    {
                        Actions = new List<JobDetailAction>(new List<JobDetailAction>()
                            {
                                new JobDetailAction()
                                {
                                    JobDetailId = 1, Action = EventAction.Credit, Quantity = 1, Status = ActionStatus.Submitted
                                },
                                new JobDetailAction()
                                {
                                    JobDetailId = 1, Action = EventAction.CreditAndReorder, Quantity = 2,Status = ActionStatus.Submitted
                                }
                            })
                    },
                    new JobDetail()
                    {
                        Actions =
                            new List<JobDetailAction>(new List<JobDetailAction>()
                            {
                                new JobDetailAction()
                                {
                                    JobDetailId = 2,
                                    Action = EventAction.ReplanInRoadnet,
                                    Quantity = 3,
                                    Status = ActionStatus.Submitted
                                }
                            })
                    }
                };

                var jobDetails = GetOrigJobDetails();
                string entry1 = $"Product: {jobDetails[0].PhProductCode} - {jobDetails[0].ProdDesc}. " +
                    "Actions changed from " +
                                $"'{string.Join(", ", jobDetails[0].Actions.Select(d => d.GetString()))}' to " +
                                $"'{string.Join(", ", submittedJobDetails[0].Actions.Select(d => d.GetString()))}'. ";
                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.Entry == entry1)),Times.Once);

                string entry2 = $"Product: {jobDetails[1].PhProductCode} - {jobDetails[1].ProdDesc}. " +
                    "Actions changed from " +
                 $"'{string.Join(", ", jobDetails[1].Actions.Select(d => d.GetString()))}' to " +
                 $"'{string.Join(", ", submittedJobDetails[1].Actions.Select(d => d.GetString()))}'. ";
                auditRepo.Verify(r => r.Save(It.Is<Audit>(a => a.Entry == entry2)), Times.Once);

                auditRepo.VerifySet(a => a.CurrentUser = "user");
            }
        }

        public class WhenSavingGrnTests : DeliveryServiceTests
        {
            private void CommonArrangeAct()
            {
                var jobId = 1;
                var grn = "1234";
                var branchId = 2;

                jobRepo.Setup(jr => jr.SaveGrn(It.IsAny<int>(), It.IsAny<string>()));
                exceptionEventRepo.Setup(er => er.InsertGrnEvent(It.IsAny<GrnEvent>()));

                //ACT
                service.SaveGrn(jobId, grn, branchId, "user");
            }

            [Test]
            public void GrnIsSavedForJob()
            {
                CommonArrangeAct();
                jobRepo.Verify(j => j.SaveGrn(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
                exceptionEventRepo.Verify(e => e.InsertGrnEvent(It.IsAny<GrnEvent>()), Times.Once);

            }

        }
    }
}
