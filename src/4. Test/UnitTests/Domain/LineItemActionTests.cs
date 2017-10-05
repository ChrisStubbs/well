using System;
using System.Collections;
using Moq;
using NUnit.Framework;
using PH.Well.Common.Contracts;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Services.Contracts;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Domain
{
    [TestFixture]
    class LineItemActionTests
    {
        [Test]
        [TestCaseSource(typeof(LineItemActionTestsSource), nameof(LineItemActionTestsSource.TestCases))]
        public bool Should_HasChanges(LineItemAction testValue)
        {
            var sut = LineItemActionFactory.New.GenerateForHasChange().Build();

            return sut.HasChanges(testValue);
        }
    }

    class LineItemActionTestsSource
    {
        public static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(LineItemActionFactory.New.GenerateForHasChange().Build())
                    .Returns(false);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ExceptionType = Well.Domain.Enums.ExceptionType.Bypass).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Quantity = 19).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Source = Well.Domain.Enums.JobDetailSource.Assembler).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Reason = Well.Domain.Enums.JobDetailReason.BookingError).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ReplanDate = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.SubmittedDate = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ApprovalDate = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ApprovedBy = "").Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.ActionedBy = null).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.Originator = Well.Domain.Enums.Originator.Customer).Build())
                    .Returns(true);
                yield return new TestCaseData(
                    LineItemActionFactory.New
                        .GenerateForHasChange()
                        .With(p => p.DeliveryAction = Well.Domain.Enums.DeliveryAction.Close).Build())
                    .Returns(true);
            }
        }
    }

    [TestFixture]
    public class LineItemActionServiceTests
    {
        private LineItemActionService service;
        private Mock<ILineItemActionRepository> lineItemActionRepository;
        private Mock<ILineItemSearchReadRepository> lineItemActionSearchRepo;
        private Mock<ILineItemActionCommentRepository> lineItemActionComment;
        private Mock<IJobRepository> jobRepository;
        private Mock<IJobService> jobService;
        private Mock<ICommentReasonRepository> commentReasonRepository;

        [SetUp]
        public void Setup()
        {
            jobRepository = new Mock<IJobRepository>();
            lineItemActionRepository = new Mock<ILineItemActionRepository>();
            lineItemActionSearchRepo = new Mock<ILineItemSearchReadRepository>();
            lineItemActionComment = new Mock<ILineItemActionCommentRepository>();
            jobService = new Mock<IJobService>();
            commentReasonRepository = new Mock<ICommentReasonRepository>();

            service = new LineItemActionService(lineItemActionRepository.Object, lineItemActionSearchRepo.Object,
                lineItemActionComment.Object, jobRepository.Object, jobService.Object,
                jobService.Object, commentReasonRepository.Object);
        }

        [Test]
        public void ShouldThrowExceptionIfNotActionableJob()
        {
            var job = JobFactory.New.With(x => x.JobType = JobType.NotDefined).Build();
            Assert.Throws<ArgumentOutOfRangeException>(() => service.CanSetActionForJob(job, DeliveryAction.Credit));
        }


        [Test]
        public void ShouldNotAllowToPODNonPODJob()
        {
            var job = JobFactory.New.With(x => x.JobType = JobType.Alcohol).Build();
            Assert.False(service.CanSetActionForJob(job, DeliveryAction.Pod));
        }

        [Test]
        public void ShouldNotAllowToCreditPODJob()
        {
            var job = JobFactory.New.With(x => x.JobType = JobType.Alcohol)
                // the magic number 8 means its POD job
                .With(x => x.ProofOfDelivery = (int) ProofOfDelivery.CocaCola).Build();
            Assert.False(service.CanSetActionForJob(job, DeliveryAction.Credit));
        }
    }
}
