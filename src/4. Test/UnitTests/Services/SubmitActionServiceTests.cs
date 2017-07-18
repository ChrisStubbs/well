namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using Factories;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class SubmitActionServiceTests
    {
        private Mock<ILogger> logger;
        private Mock<IDeliveryLineCreditMapper> deliveryLineCreditMapper;
        private Mock<ICreditTransactionFactory> creditTransactionFactory;
        private Mock<IExceptionEventRepository> exceptionEventRepository;
        private Mock<ISubmitActionValidation> validator;
        private Mock<IActionSummaryMapper> actionSummaryMapper;
        private Mock<IJobRepository> jobRepository;
        private Mock<IJobService> jobService;
        private Mock<IUserRepository> userRepository;
        private SubmitActionService submitActionService;
        private Mock<SubmitActionService> mockedSubmitActionService;

        [SetUp]
        public virtual void SetUp()
        {
            logger = new Mock<ILogger>();
            deliveryLineCreditMapper = new Mock<IDeliveryLineCreditMapper>();
            creditTransactionFactory = new Mock<ICreditTransactionFactory>();
            exceptionEventRepository = new Mock<IExceptionEventRepository>();
            validator = new Mock<ISubmitActionValidation>();
            actionSummaryMapper = new Mock<IActionSummaryMapper>();
            jobRepository = new Mock<IJobRepository>();
            jobService = new Mock<IJobService>();
            userRepository = new Mock<IUserRepository>();

            submitActionService = new SubmitActionService(
                logger.Object,
                deliveryLineCreditMapper.Object,
                creditTransactionFactory.Object,
                exceptionEventRepository.Object,
                validator.Object,
                actionSummaryMapper.Object,
                jobRepository.Object,
                jobService.Object,
                userRepository.Object);

            mockedSubmitActionService = new Mock<SubmitActionService>(
                MockBehavior.Loose,
                logger.Object,
                deliveryLineCreditMapper.Object,
                creditTransactionFactory.Object,
                exceptionEventRepository.Object,
                validator.Object,
                actionSummaryMapper.Object,
                jobRepository.Object,
                jobService.Object,
                userRepository.Object);
        }


        public class TheSubmitActionMethod : SubmitActionServiceTests
        {
            private SubmitActionModel submitAction;
            private List<Job> jobs;
            private Job job1;
            private Job job2;
            private Job job3;

            private readonly Queue<ResolutionStatus> job1GetCurrentResolutionStatusQueue = new Queue<ResolutionStatus>();
            private readonly Queue<ResolutionStatus> job1GetNextResolutionStatusQueue = new Queue<ResolutionStatus>();
            private readonly Queue<ResolutionStatus> job2GetCurrentResolutionStatusQueue = new Queue<ResolutionStatus>();
            private readonly Queue<ResolutionStatus> job2GetNextResolutionStatusQueue = new Queue<ResolutionStatus>();
            private readonly Queue<ResolutionStatus> job3GetCurrentResolutionStatusQueue = new Queue<ResolutionStatus>();
            private readonly Queue<ResolutionStatus> job3GetNextResolutionStatusQueue = new Queue<ResolutionStatus>();

            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
            }

            [Test]
            public void ShouldReturnInvalidIfValidationFails()
            {
                submitAction = new SubmitActionModel { JobIds = new[] { 1 } };
                jobs = new List<Job>();
                jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(jobs);
                jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>())).Returns(jobs);
                var submitActionResult = new SubmitActionResult { IsValid = false };
                validator.Setup(x => x.Validate(submitAction, jobs)).Returns(submitActionResult);

                var results = submitActionService.SubmitAction(submitAction);
                Assert.That(results, Is.EqualTo(submitActionResult));
            }

            private void WithHappyPathSetup()
            {
                submitAction = new SubmitActionModel { JobIds = new[] { 1, 2, 3 } };

                job1 = JobFactory.New.With(x => x.Id = 1).Build();
                job2 = JobFactory.New.With(x => x.Id = 2).Build();
                job3 = JobFactory.New.With(x => x.Id = 3).Build();
                jobs = new List<Job> { job1, job2, job3 };

                jobRepository.Setup(x => x.GetByIds(submitAction.JobIds)).Returns(jobs);
                jobService.Setup(x => x.PopulateLineItemsAndRoute(jobs)).Returns(jobs);
                var submitActionResult = new SubmitActionResult { IsValid = true };
                validator.Setup(x => x.Validate(submitAction, jobs)).Returns(submitActionResult);

                jobService.Setup(x => x.GetCurrentResolutionStatus(job1)).Returns(job1GetCurrentResolutionStatusQueue.Dequeue);
                jobService.Setup(x => x.GetNextResolutionStatus(job1)).Returns(job1GetNextResolutionStatusQueue.Dequeue);

                jobService.Setup(x => x.GetCurrentResolutionStatus(job2)).Returns(job2GetCurrentResolutionStatusQueue.Dequeue);
                jobService.Setup(x => x.GetNextResolutionStatus(job2)).Returns(job2GetNextResolutionStatusQueue.Dequeue);

                jobService.Setup(x => x.GetCurrentResolutionStatus(job3)).Returns(job3GetCurrentResolutionStatusQueue.Dequeue);
                jobService.Setup(x => x.GetNextResolutionStatus(job3)).Returns(job3GetNextResolutionStatusQueue.Dequeue);

                jobRepository.Setup(x => x.SaveJobResolutionStatus(It.IsAny<Job>()));
            }

            private void WithJob1PendingSubmissionAndApproved()
            {
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.PendingSubmission);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Credited);
            }

            private void WithJob1PendingApprovalAndApproved()
            {
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.PendingApproval);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Credited);
            }

            [Test]
            public void ShouldGetJobsWithPopulatedLineItemsAndRoutes()
            {
                WithHappyPathSetup();
                var results = submitActionService.SubmitAction(submitAction);

                jobRepository.Verify(x => x.GetByIds(submitAction.JobIds), Times.Once);
                jobRepository.Verify(x => x.GetByIds(It.IsAny<IEnumerable<int>>()), Times.Once);
                jobService.Verify(x => x.PopulateLineItemsAndRoute(jobs), Times.Once);
                jobService.Verify(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>()), Times.Once);
            }

            [Test]
            public void IfJobIsValidResolutionStatusPendingAndUserAndStatusIsApprovedThenCreditsSubmittedToAdam()
            {

                WithHappyPathSetup();
                WithJob1PendingSubmissionAndApproved();
                job2GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);
                job3GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);

                var results = mockedSubmitActionService.Object.SubmitAction(submitAction);

                mockedSubmitActionService.Verify(x => x.SubmitCredits(It.IsAny<Job>()), Times.Once);
                mockedSubmitActionService.Verify(x => x.SubmitCredits(job1), Times.Once);
            }

            //[Test]
            //public void IfJobIsValidResolutionStatusPendingAndUserAndStatusIsApprovedCorrectResolutionStatusSaved()
            //{

            //    WithHappyPathSetup();
            //    WithJob1PendingSubmissionAndApproved();
            //    job2GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);
            //    job3GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);

            //    var results = mockedSubmitActionService.Object.SubmitAction(submitAction);

            //    jobRepository.Verify(x => x.SaveJobResolutionStatus(It.IsAny<Job>()), Times.Exactly(2));
            //    jobRepository.Verify(x => x.SaveJobResolutionStatus(It.Is<Job>(j=> j.ResolutionStatus == ResolutionStatus.Approved)), Times.Exactly(1));
            //    jobRepository.Verify(x => x.SaveJobResolutionStatus(It.Is<Job>(j => j.ResolutionStatus == ResolutionStatus.Credited)), Times.Exactly(1));
            //    jobRepository.Verify(x => x.SaveJobResolutionStatus(job1), Times.Exactly(2));
            //}

            [Test]
            public void IfJobIsValidResolutionStatusPendingApprovalAndUserAndStatusIsApprovedThenCreditsSubmittedToAdam()
            {

                WithHappyPathSetup();
                WithJob1PendingApprovalAndApproved();
                job2GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);
                job3GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);

                var results = mockedSubmitActionService.Object.SubmitAction(submitAction);

                mockedSubmitActionService.Verify(x => x.SubmitCredits(It.IsAny<Job>()), Times.Once);
                mockedSubmitActionService.Verify(x => x.SubmitCredits(job1), Times.Once);

            }




            //[Test] TODO:
            //public void IfJobIsValidResolutionStatusPendingAndUserAndStatusIsApprovedThenCreditsSubmittedToAdam()
            //{
            //    var job = JobFactory.New.Build();
            //    var submitAction = new SubmitActionModel { JobIds = new[] { 1 } };
            //    var jobs = new List<Job>{ job };
            //    jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(jobs);
            //    jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>())).Returns(jobs);

            //    var submitActionResult = new SubmitActionResult { IsValid = true };
            //    validator.Setup(x => x.Validate(submitAction, jobs)).Returns(submitActionResult);

            //    var getCurrentResolution = new Queue<ResolutionStatus>();
            //    getCurrentResolution.Enqueue(ResolutionStatus.PendingSubmission);
            //    getCurrentResolution.Enqueue(ResolutionStatus.Approved);
            //    jobService.Setup(x => x.GetCurrentResolutionStatus(job)).Returns(getCurrentResolution.Dequeue);

            //    var getNextResolution = new Queue<ResolutionStatus>();
            //    getNextResolution.Enqueue(ResolutionStatus.Approved);
            //    getNextResolution.Enqueue(ResolutionStatus.Credited);
            //    jobService.Setup(x => x.GetNextResolutionStatus(job)).Returns(getNextResolution.Dequeue);
            //    jobRepository.Setup(x => x.SaveJobResolutionStatus(job));

            //    var results = submitActionService.SubmitAction(submitAction);

            //    Assert.That(results, Is.EqualTo(submitActionResult));
            //}

            //    private const string UserName = "Druno Bobson";
            //    private readonly User user = new User { Id = 555, Name = UserName };

            //    private LineItemActionSubmitModel[] GetAction()
            //    {
            //        return new[]
            //        {
            //            new LineItemActionSubmitModel{ JobId  = 1,BranchId =2, ProductCode = "Prod1",Reason = JobDetailReason.RecallProduct,Source = JobDetailSource.Assembler,Quantity = 5, InvoiceNumber = "inv1", NetPrice = 10},
            //            new LineItemActionSubmitModel{ JobId  = 1,BranchId =2, ProductCode = "Prod2",Reason = JobDetailReason.Administration,Source = JobDetailSource.Delivery,Quantity = 1,  InvoiceNumber = "inv1",NetPrice = 10},
            //            new LineItemActionSubmitModel{ JobId  = 2,BranchId =2, ProductCode = "Prod3",Reason = JobDetailReason.AccumulatedDamages,Source = JobDetailSource.Customer,Quantity = 3, InvoiceNumber = "inv2",NetPrice = 10}
            //        };
            //    }

            //    private DeliveryLineCredit[] GetDeliveryLineCredits()
            //    {
            //        return new DeliveryLineCredit[]
            //        {
            //            new DeliveryLineCredit {JobId = 1,Reason = (int) JobDetailReason.RecallProduct,Source = (int) JobDetailSource.Assembler,Quantity = 5,ProductCode ="Prod1"},
            //            new DeliveryLineCredit {JobId = 1,Reason = (int) JobDetailReason.Administration,Source = (int) JobDetailSource.Delivery, Quantity = 1,ProductCode ="Prod2"},
            //            new DeliveryLineCredit {JobId = 2,Reason = (int) JobDetailReason.AccumulatedDamages,Source = (int) JobDetailSource.Customer,Quantity = 3,ProductCode ="Prod3"},

            //        };

            //    }

            //    private SubmitActionModel submitAction;
            //    private List<DeliveryLineCredit> job1Credits;
            //    private List<DeliveryLineCredit> job2Credits;
            //    readonly CreditTransaction creditTransaction1 = new CreditTransaction();
            //    readonly CreditTransaction creditTransaction2 = new CreditTransaction();
            //    private LineItemActionSubmitModel[] actions;

            //    public void CommonCreditSetup()
            //    {
            //        submitAction = new SubmitActionModel { Action = DeliveryAction.Credit, JobIds = new[] { 1, 2 } };
            //        actions = GetAction();
            //        var deliveryLineCredits = GetDeliveryLineCredits();
            //        job1Credits = deliveryLineCredits.Where(x => x.JobId == 1).ToList();
            //        job2Credits = deliveryLineCredits.Where(x => x.JobId == 2).ToList();


            //        userNameProvider.Setup(x => x.GetUserName()).Returns(UserName);
            //        lineItemActionRepository.Setup(x => x.GetUnsubmittedActions(DeliveryAction.Credit)).Returns(actions);
            //        validator.Setup(x => x.Validate(submitAction, actions)).Returns(new SubmitActionResult { IsValid = true });

            //        creditTransactionFactory.Setup(x => x.Build(job1Credits, 2)).Returns(creditTransaction1);
            //        creditTransactionFactory.Setup(x => x.Build(job2Credits, 2)).Returns(creditTransaction2);

            //        exceptionEventRepository.Setup(x => x.InsertCreditEventTransaction(creditTransaction1));
            //        exceptionEventRepository.Setup(x => x.InsertCreditEventTransaction(creditTransaction2));

            //        lineItemActionRepository.Setup(x => x.Update(It.IsAny<LineItemActionSubmitModel>()));
            //    }


            //    [Test]
            //    public void IfUserAboveCreditThresholdShouldCreditJobInAdamForEachJobWhenActionIsCreditAndSaveAsSubmitted()
            //    {
            //        CommonCreditSetup();

            //        var calls = 0;
            //        deliveryLineCreditMapper.Setup(x => x.Map(It.IsAny<LineItemActionSubmitModel[]>())).Callback((IEnumerable<LineItemActionSubmitModel> x) =>
            //            {
            //                calls++;
            //                var xArray = x.ToArray();
            //                if (calls == 1)
            //                {
            //                    Assert.That(xArray.Count, Is.EqualTo(2));
            //                    Assert.That(xArray[0], Is.EqualTo(actions[0]));
            //                    Assert.That(xArray[1], Is.EqualTo(actions[1]));
            //                }
            //                else
            //                {
            //                    Assert.That(xArray.Count, Is.EqualTo(1));
            //                    Assert.That(xArray[0], Is.EqualTo(actions[2]));
            //                }
            //            }
            //        ).Returns(() => calls == 1 ? job1Credits : job2Credits);

            //        userThresholdService.Setup(x => x.CanUserCredit(It.IsAny<decimal>())).Returns(new ThresholdResponse { CanUserCredit = true });

            //        var result = submitActionService.SubmitAction(submitAction);

            //        deliveryLineCreditMapper.Verify(x => x.Map(It.IsAny<LineItemActionSubmitModel[]>()), Times.Exactly(2));
            //        creditTransactionFactory.Verify(x => x.Build(job1Credits, 2), Times.Once);
            //        creditTransactionFactory.Verify(x => x.Build(job2Credits, 2), Times.Once);

            //        exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction1), Times.Once);
            //        exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction2), Times.Once);

            //        lineItemActionRepository.Verify(x => x.Update(It.IsAny<LineItemActionSubmitModel>()), Times.Exactly(3));

            //        AssertSubmittedAndApproved(actions[0], UserName);
            //        AssertSubmittedAndApproved(actions[1], UserName);
            //        AssertSubmittedAndApproved(actions[2], UserName);

            //    }

            //    [Test]
            //    public void ShouldOnlyCreditJobsBelowTheThreshold()
            //    {
            //        CommonCreditSetup();

            //        deliveryLineCreditMapper.Setup(x => x.Map(It.IsAny<LineItemActionSubmitModel[]>())).Callback((IEnumerable<LineItemActionSubmitModel> x) =>
            //            {
            //                var xArray = x.ToArray();
            //                Assert.That(xArray.Count, Is.EqualTo(1));
            //                Assert.That(xArray[0], Is.EqualTo(actions[2]));
            //            }
            //        ).Returns(job2Credits);
            //        userThresholdService.Setup(x => x.CanUserCredit(60)).Returns(new ThresholdResponse { CanUserCredit = false });
            //        userThresholdService.Setup(x => x.CanUserCredit(30)).Returns(new ThresholdResponse { CanUserCredit = true });

            //        var result = submitActionService.SubmitAction(submitAction);

            //        deliveryLineCreditMapper.Verify(x => x.Map(It.IsAny<LineItemActionSubmitModel[]>()), Times.Exactly(1));
            //        creditTransactionFactory.Verify(x => x.Build(job1Credits, 2), Times.Never);
            //        creditTransactionFactory.Verify(x => x.Build(job2Credits, 2), Times.Once);

            //        exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction1), Times.Never);
            //        exceptionEventRepository.Verify(x => x.InsertCreditEventTransaction(creditTransaction2), Times.Once);

            //        lineItemActionRepository.Verify(x => x.Update(It.IsAny<LineItemActionSubmitModel>()), Times.Exactly(3));

            //        AssertSubmitted(actions[0], UserName);
            //        AssertSubmitted(actions[1], UserName);
            //        AssertSubmittedAndApproved(actions[2], UserName);

            //    }

            //    [Test]
            //    public void ShouldNotSaveIfValidationFails()
            //    {
            //        submitAction = new SubmitActionModel { Action = DeliveryAction.Credit, JobIds = new[] { 1, 2 } };
            //        actions = GetAction();

            //        userNameProvider.Setup(x => x.GetUserName()).Returns(UserName);
            //        lineItemActionRepository.Setup(x => x.GetUnsubmittedActions(DeliveryAction.Credit)).Returns(actions);
            //        var failureResult = new SubmitActionResult { IsValid = false };
            //        validator.Setup(x => x.Validate(submitAction, actions)).Returns(failureResult);

            //        var result = submitActionService.SubmitAction(submitAction);
            //        Assert.That(result, Is.EqualTo(failureResult));
            //    }

            //    private void AssertSubmittedAndApproved(LineItemActionSubmitModel item, string userName)
            //    {
            //        Assert.That(item.SubmittedDate, Is.Not.Null);
            //        Assert.That(item.SubmittedDate.Value, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now));
            //        Assert.That(item.ActionedBy, Is.EqualTo(userName));
            //        Assert.That(item.ApprovalDate, Is.Not.Null);
            //        Assert.That(item.ApprovalDate.Value, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now));
            //        Assert.That(item.ApprovedBy, Is.EqualTo(userName));
            //        lineItemActionRepository.Verify(x => x.Update(item), Times.Once);
            //    }

            //    private void AssertSubmitted(LineItemActionSubmitModel item, string userName)
            //    {
            //        Assert.That(item.SubmittedDate, Is.Not.Null);
            //        Assert.That(item.SubmittedDate.Value, Is.InRange(DateTime.Now.AddMinutes(-1), DateTime.Now));
            //        Assert.That(item.ActionedBy, Is.EqualTo(userName));
            //        Assert.That(item.ApprovalDate, Is.Null);
            //        Assert.That(item.ApprovedBy, Is.Null);
            //        lineItemActionRepository.Verify(x => x.Update(item), Times.Once);
            //    }
        }
    }
}