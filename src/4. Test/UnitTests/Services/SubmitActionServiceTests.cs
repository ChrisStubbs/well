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

            private Queue<ResolutionStatus> job1GetCurrentResolutionStatusQueue;
            private Queue<ResolutionStatus> job1GetNextResolutionStatusQueue;
            private List<ResolutionStatus> job1SaveJobResolutionStatus;

            private Queue<ResolutionStatus> job2GetCurrentResolutionStatusQueue;
            private Queue<ResolutionStatus> job2GetNextResolutionStatusQueue;
            private Queue<ResolutionStatus> job3GetCurrentResolutionStatusQueue;
            private Queue<ResolutionStatus> job3GetNextResolutionStatusQueue;

            [SetUp]
            public override void SetUp()
            {
                base.SetUp();

                job1GetCurrentResolutionStatusQueue = new Queue<ResolutionStatus>();
                job1GetNextResolutionStatusQueue = new Queue<ResolutionStatus>();
                job1SaveJobResolutionStatus = new List<ResolutionStatus>();

                job2GetCurrentResolutionStatusQueue = new Queue<ResolutionStatus>();
                job2GetNextResolutionStatusQueue = new Queue<ResolutionStatus>();
                job3GetCurrentResolutionStatusQueue = new Queue<ResolutionStatus>();
                job3GetNextResolutionStatusQueue = new Queue<ResolutionStatus>();
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

            [Test]
            public void IfJobIsValidResolutionStatusPendingAndStatusIsPendingApproval_Then_UnassignUser()
            {

                WithHappyPathSetup();
                WithJob1PendingSubmissionAndPendingApproval();
                job2GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);
                job3GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);

                var results = mockedSubmitActionService.Object.SubmitAction(submitAction);

                mockedSubmitActionService.Verify(x => x.SubmitCredits(It.IsAny<Job>()), Times.Never);
                userRepository.Verify(x=> x.UnAssignJobToUser(It.IsAny<int>()),Times.Once);
                userRepository.Verify(x => x.UnAssignJobToUser(job1.Id), Times.Once);
            }

            [Test]
            public void IfJobIsValidResolutionStatusPendingAndStatusIsPendingApproval_Then_UpdateJob()
            {

                WithHappyPathSetup();
                WithJob1PendingSubmissionAndPendingApproval();
                job2GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);
                job3GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);

                var results = mockedSubmitActionService.Object.SubmitAction(submitAction);


                jobRepository.Verify(x => x.SaveJobResolutionStatus(It.IsAny<Job>()), Times.Once);
                jobRepository.Verify(x => x.SaveJobResolutionStatus(job1), Times.Once);
                jobRepository.Verify(x => x.Update(It.IsAny<Job>()), Times.Exactly(1));
                jobRepository.Verify(x => x.Update(job1), Times.Exactly(1));
                Assert.That(job1SaveJobResolutionStatus[0], Is.EqualTo(ResolutionStatus.PendingApproval));

            }

            [Test]
            public void IfJobIsValidResolutionStatusPendingAndUserAndStatusIsApprovedCorrectResolutionStatusSavedAndJobUpdated()
            {

                WithHappyPathSetup();
                WithJob1PendingSubmissionAndApproved();
                job2GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);
                job3GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);

                var results = mockedSubmitActionService.Object.SubmitAction(submitAction);

                jobRepository.Verify(x => x.SaveJobResolutionStatus(It.IsAny<Job>()), Times.Exactly(2));
                jobRepository.Verify(x => x.SaveJobResolutionStatus(job1), Times.Exactly(2));
                jobRepository.Verify(x => x.Update(It.IsAny<Job>()), Times.Exactly(1));
                jobRepository.Verify(x => x.Update(job1), Times.Exactly(1));
                Assert.That(job1SaveJobResolutionStatus[0],Is.EqualTo(ResolutionStatus.Approved));
                Assert.That(job1SaveJobResolutionStatus[1], Is.EqualTo(ResolutionStatus.Credited));
                
            }


            [Test]
            public void IfJobIsValidPendingApprovalAndUserAndStatusIsApprovedThenCreditsSubmittedToAdam()
            {

                WithHappyPathSetup();
                WithJob1PendingApprovalAndApproved();
                job2GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);
                job3GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Closed);

                var results = mockedSubmitActionService.Object.SubmitAction(submitAction);

                mockedSubmitActionService.Verify(x => x.SubmitCredits(It.IsAny<Job>()), Times.Once);
                mockedSubmitActionService.Verify(x => x.SubmitCredits(job1), Times.Once);

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
                jobRepository.Setup(x => x.SaveJobResolutionStatus(job1)).Callback<Job>(j => job1SaveJobResolutionStatus.Add(j.ResolutionStatus));
                jobRepository.Setup(x => x.Update(It.IsAny<Job>()));
            }

            private void WithJob1PendingSubmissionAndApproved()
            {
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.PendingSubmission);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Credited);
            }

            private void WithJob1PendingSubmissionAndPendingApproval()
            {
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.PendingSubmission);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.PendingApproval);
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.PendingApproval);
            }

            private void WithJob1PendingApprovalAndApproved()
            {
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.PendingApproval);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetCurrentResolutionStatusQueue.Enqueue(ResolutionStatus.Approved);
                job1GetNextResolutionStatusQueue.Enqueue(ResolutionStatus.Credited);
            }

        }

        public class TheSubmitCreditsMethod : SubmitActionServiceTests
        {

            public void ShouldCreditJobInAdamsIfAnyCreditDeliveryActions()
            {
                var job = JobFactory.New.With(x => x.LineItems.Add(LineItemFactory.New.AddCreditAction().Build())).Build();
            }
        }
    }
}
