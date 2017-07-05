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

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>();
            deliveryLineCreditMapper = new Mock<IDeliveryLineCreditMapper>(MockBehavior.Strict);
            creditTransactionFactory = new Mock<ICreditTransactionFactory>(MockBehavior.Strict);
            exceptionEventRepository = new Mock<IExceptionEventRepository>(MockBehavior.Strict);
            validator = new Mock<ISubmitActionValidation>(MockBehavior.Strict);
            actionSummaryMapper = new Mock<IActionSummaryMapper>(MockBehavior.Strict);
            jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            jobService = new Mock<IJobService>(MockBehavior.Strict);
            userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

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
        }


        public class TheSubmitActionMethod : SubmitActionServiceTests
        {
            [Test]
            public void ShouldReturnInvalidIfValidationFails()
            {
                var submitAction = new SubmitActionModel { JobIds = new[] { 1 } };
                var jobs = new List<Job>();
                jobRepository.Setup(x => x.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(jobs);
                jobService.Setup(x => x.PopulateLineItemsAndRoute(It.IsAny<IEnumerable<Job>>())).Returns(jobs);
                var submitActionResult = new SubmitActionResult { IsValid = false };
                validator.Setup(x => x.Validate(submitAction, jobs)).Returns(submitActionResult);

                var results = submitActionService.SubmitAction(submitAction);
                Assert.That(results, Is.EqualTo(submitActionResult));
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