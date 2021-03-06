﻿namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
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
    public class BulkEditServiceTests
    {
        private Mock<ILogger> logger;
        private Mock<IJobRepository> jobRepository;
        private Mock<IPatchSummaryMapper> mapper;
        private Mock<ILineItemActionRepository> lineItemActionRepository;
        private Mock<IJobService> jobService;
        private BulkEditService bulkEditService;
        private Mock<IUserNameProvider> userNameProvider;
        public User User = new User { Id = 1 };
        private Mock<ILineItemActionService> lineItemActionService;

        [SetUp]
        public void Setup()
        {
            logger = new Mock<ILogger>();
            jobRepository = new Mock<IJobRepository>();
            mapper = new Mock<IPatchSummaryMapper>();
            userNameProvider = new Mock<IUserNameProvider>();
            lineItemActionService = new Mock<ILineItemActionService>();
           
            lineItemActionRepository = new Mock<ILineItemActionRepository>(MockBehavior.Strict);
            jobService = new Mock<IJobService>(MockBehavior.Strict);

            bulkEditService = new BulkEditService(
                logger.Object,
                jobService.Object,
                jobRepository.Object,
                mapper.Object,
                lineItemActionRepository.Object,
                userNameProvider.Object,
                lineItemActionService.Object
            );
        }

        public class TheGetEditableJobsMethod : BulkEditServiceTests
        {
            private List<Job> jobList;
            private Job job1;
            private Job job2;
            private Job job3;
            private Job job4;

            [SetUp]
            public void SetUp()
            {
                job1 = JobFactory.New
                    .With(x => x.Id = 1)
                    .With(x => x.ResolutionStatus = ResolutionStatus.ActionRequired)
                    .With(x => x.JobType = JobType.Alcohol)
                    .With(x => x.LineItems.Add(LineItemFactory.New.With(li => li.Id = 2020).AddNotDefinedAction().Build()))
                    .Build();

                job2 = JobFactory.New
                    .With(x => x.Id = 2)
                    .With(x => x.ResolutionStatus = ResolutionStatus.ActionRequired)
                    .With(x => x.JobType = JobType.Alcohol)
                    .With(x => x.LineItems.Add(LineItemFactory.New.AddCloseAction().Build())) // wont be included as close has 0 quantity
                    .Build();

                job3 = JobFactory.New
                    .With(x => x.Id = 3)
                    .With(x => x.ResolutionStatus = ResolutionStatus.ActionRequired)
                    .With(x => x.JobType = JobType.Alcohol)
                    .With(x => x.LineItems.Add(LineItemFactory.New.With(li => li.Id = 2021).AddCreditAction().Build()))
                    .Build();

                job4 = JobFactory.New
                    .With(x => x.Id = 4)
                    .With(x => x.ResolutionStatus = ResolutionStatus.ActionRequired)
                    .With(x => x.JobType = JobType.Alcohol)
                    .With(x => x.LineItems.Add(LineItemFactory.New.With(li => li.Id = 2022).AddCreditAction().Build()))
                    .Build();

                jobList = new List<Job> { job1, job2, job3, job4 };
            }

            [Test]
            public void ShouldReturnEditableJobs()
            {
                jobService.Setup(x => x.PopulateLineItemsAndRoute(jobList)).Returns(jobList);
                jobService.Setup(x => x.CanEdit(It.IsAny<Job>(), It.IsAny<string>())).Returns(string.Empty);
              
                var editableJobs = bulkEditService.GetEditableJobs(jobList).ToArray();

                Assert.That(editableJobs.Count(), Is.EqualTo(3));
                Assert.That(editableJobs[0], Is.EqualTo(job1));
                Assert.That(editableJobs[1], Is.EqualTo(job3));
                Assert.That(editableJobs[2], Is.EqualTo(job4));
            }

            [Test]
            public void ShouldReturnEditableJobsWhereLineItemInList()
            {
                jobService.Setup(x => x.PopulateLineItemsAndRoute(jobList)).Returns(jobList);
                var expectedJobs = new List<Job>(new[] {job1, job3});
                jobService.Setup(x => x.CanEdit(It.Is<Job>(y => expectedJobs.Contains(y)), It.IsAny<string>()))
                    .Returns(string.Empty);

                var editableJobs = bulkEditService.GetEditableJobs(jobList, new List<int> {2020, 2021}).ToArray();

                Assert.That(editableJobs.Count(), Is.EqualTo(2));
                Assert.That(editableJobs[0], Is.EqualTo(job1));
                Assert.That(editableJobs[1], Is.EqualTo(job3));
            }
        }

        public class TheUpdateMethod : BulkEditServiceTests
        {
            private Mock<BulkEditService> stubbedService;
            
            [SetUp]
            public void SetUp()
            {
                base.Setup();
                stubbedService = new Mock<BulkEditService>(
                    logger.Object,
                    jobService.Object,
                    jobRepository.Object,
                    mapper.Object,
                    lineItemActionRepository.Object,
                    userNameProvider.Object,
                    lineItemActionService.Object
                    )
                { CallBase = true };
            }

            [Test]
            public void ShouldCallUpdateOnAllLineItemsToUpdate()
            {
                var editableJobs =new []{ JobFactory.New.Build()}.ToList();
                var lineItemIds = new List<int>();
                var lia1 = LineItemActionFactory.New
                                .With(x => x.LineItemId = 1)
                                .With(x => x.Quantity = 2)
                                .With(x => x.DeliveryAction = DeliveryAction.NotDefined)
                                .With(x => x.Source = JobDetailSource.Assembler)
                                .With(x => x.Reason = JobDetailReason.BookingError).Build();
                var lia2 = LineItemActionFactory.New.With(x => x.LineItemId = 2).Build();
                var lia3 = LineItemActionFactory.New.With(x => x.LineItemId = 3).Build();

                var lineItemActions = new List<LineItemAction> { lia1, lia2, lia3 };
                var patchRequest = BulkEditPatchRequestFactory.New
                                    .With(x => x.DeliveryAction = DeliveryAction.Close)
                                    .With(x => x.Source = JobDetailSource.Customer)
                                    .With(x => x.Reason = JobDetailReason.BookingError).Build();

                stubbedService.Setup(x => x.LineItemActionsToEdit(It.IsAny<Job>(), lineItemIds))
                    .Returns(lineItemActions);

                lineItemActionRepository.Setup(x => x.Update(It.IsAny<LineItemAction>()));
                lineItemActionService.Setup(x => x.CanSetActionForJob(It.IsAny<Job>(), It.IsAny<DeliveryAction>()))
                    .Returns(true);

                jobService.Setup(x => x.GetCurrentResolutionStatus(It.IsAny<Job>()))
                    .Returns(ResolutionStatus.Closed);

                var result = stubbedService.Object.Update(editableJobs, patchRequest, lineItemIds);

                lineItemActionRepository.Verify(x => x.Update(It.IsAny<LineItemAction>()), Times.Exactly(3));
                lineItemActionRepository.Verify(x => x.Update(lia1), Times.Once());
                lineItemActionRepository.Verify(x => x.Update(lia2), Times.Once());
                lineItemActionRepository.Verify(x => x.Update(lia3), Times.Once());

                Assert.That(lia1.Quantity, Is.EqualTo(0));
                Assert.That(lia1.DeliveryAction, Is.EqualTo(patchRequest.DeliveryAction));
                Assert.That(lia1.Source, Is.EqualTo(patchRequest.Source));
                Assert.That(lia1.Reason, Is.EqualTo(patchRequest.Reason));

                Assert.That(result.LineItemIds.Count, Is.EqualTo(3));
                Assert.That(result.LineItemIds[0], Is.EqualTo(1));
                Assert.That(result.LineItemIds[1], Is.EqualTo(2));
                Assert.That(result.LineItemIds[2], Is.EqualTo(3));
            }

            [Test]
            public void ShouldGetNextResolutionStatusAndUpdateOnAllJobs()
            {
                var job1 = JobFactory.New.With(x => x.ResolutionStatus = ResolutionStatus.ActionRequired).Build();
                var job2 = JobFactory.New.With(x => x.ResolutionStatus = ResolutionStatus.ActionRequired).Build();

                var editableJobs = new List<Job> { job1, job2 };
                var lineItemIds = new List<int>();

                var lineItemActions = new List<LineItemAction> { LineItemActionFactory.New.Build() };
                var patchRequest = BulkEditPatchRequestFactory.New.Build();
                stubbedService.Setup(x => x.LineItemActionsToUpdate(editableJobs, lineItemIds)).Returns(lineItemActions);
                lineItemActionRepository.Setup(x => x.Update(It.IsAny<LineItemAction>()));
                lineItemActionService.Setup(x => x.CanSetActionForJob(It.IsAny<Job>(), It.IsAny<DeliveryAction>()))
                    .Returns(true);
                jobService.Setup(x => x.GetCurrentResolutionStatus(job1)).Returns(ResolutionStatus.Invalid);
                jobService.Setup(x => x.GetCurrentResolutionStatus(job2)).Returns(ResolutionStatus.Approved);
                jobRepository.Setup(x => x.Update(It.IsAny<Job>()));

                var result = stubbedService.Object.Update(editableJobs, patchRequest, lineItemIds);

                jobService.Verify(x => x.GetCurrentResolutionStatus(It.IsAny<Job>()), Times.Exactly(2));
                jobService.Verify(x => x.GetCurrentResolutionStatus(job1), Times.Once);
                jobService.Verify(x => x.GetCurrentResolutionStatus(job2), Times.Once);

                jobRepository.Verify(x => x.Update(It.IsAny<Job>()), Times.Exactly(2));
                jobRepository.Verify(x => x.Update(job1), Times.Exactly(1));
                jobRepository.Verify(x => x.Update(job2), Times.Exactly(1));
                
                Assert.That(job1.ResolutionStatus, Is.EqualTo(ResolutionStatus.Invalid));
                Assert.That(job2.ResolutionStatus, Is.EqualTo(ResolutionStatus.Approved));
            }
        }
    }
}