﻿using System;
using System.Collections;
using Moq;
using NUnit.Framework;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services;
using PH.Well.Services.Contracts;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Services
{
    using Well.Common.Contracts;

    [TestFixture]
    public class JobResolutionStatusTests
    {
        private JobService sut;

        [SetUp]
        public void testSetup()
        {
            var userThreshold = new Mock<IUserThresholdService>();
            var dateThresholdService = new Mock<IDateThresholdService>();
            var jobRepository = new Mock<IJobRepository>();
            var assigneeReadRepository = new Mock<IAssigneeReadRepository>();
            var lineItemRepository = new Mock<ILineItemSearchReadRepository>();
            var userNameProvider = new Mock<IUserNameProvider>();
            var userRepository = new Mock<IUserRepository>();
            var stopService = new Mock<IStopService>();
            var activityService = new Mock<IActivityService>();
            var wellStatusAggregator = new Mock<IWellStatusAggregator>();

            this.sut = new JobService(
                jobRepository.Object,
                userThreshold.Object,
                dateThresholdService.Object,
                assigneeReadRepository.Object,
                lineItemRepository.Object,
                userNameProvider.Object,
                userRepository.Object,
                stopService.Object,
                activityService.Object, 
                wellStatusAggregator.Object
                );
        }

        [Test]
        [Description("Check if the Job is in DriverCompleted status")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void Test_ResolutionStatus_DriverCompleted()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();


            var newStatus = sut.GetCurrentResolutionStatus(job);

            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.DriverCompleted));

            job.LineItems.Add(LineItemFactory.New.AddCloseAction().Build());

            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.DriverCompleted));
        }

        [Test]
        [Description("Check if the Job is in ActionRequired status")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void Test_ResolutionStatus_ActionRequired()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.ActionRequired));

            job.LineItems[0] = LineItemFactory.New.AddCloseAction().Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.ActionRequired));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.ActionRequired));
        }

        [Test]
        [Description("Check if the Job is in PendingSubmission status")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void Test_ResolutionStatus_PendingSubmission()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the initial status*/)
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.PendingSubmission));

            job.ResolutionStatus = ResolutionStatus.PendingApproval;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingSubmission));
        }

        [Test]
        public void ShouldReturnPendingSubmissionIfAlreadyPendingSubmission()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the status*/)
                .Build();
            job.ResolutionStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(job.ResolutionStatus, Is.EqualTo(ResolutionStatus.PendingSubmission));
            Assert.That(sut.GetCurrentResolutionStatus(job), Is.EqualTo(ResolutionStatus.PendingSubmission));
        }

        [Test]
        public void ShouldReturnPendingSubmissionIfCompletedByWell()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid /*doesn't really matter the status*/)
                .Build();
            job.ResolutionStatus = ResolutionStatus.ManuallyCompleted;
            Assert.That(job.ResolutionStatus, Is.EqualTo(ResolutionStatus.ManuallyCompleted));
            Assert.That(sut.GetCurrentResolutionStatus(job), Is.EqualTo(ResolutionStatus.PendingSubmission));
        }

        [Test]
        [Description("Check if the Job is in PendingApproval status")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void Test_ResolutionStatus_PendingApproval()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingSubmission)
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));

            job.ResolutionStatus = ResolutionStatus.PendingApproval;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.PendingApproval));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.PendingApproval));
        }

        [Test]
        [Description("Check if the Job is in Approved status")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void Test_ResolutionStatus_Approved()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));

            job.ResolutionStatus = ResolutionStatus.Approved;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Approved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Approved));
        }

        [Test]
        [Description("Check if the Job is in Credited status")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void Test_ResolutionStatus_Credited()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job.ResolutionStatus = ResolutionStatus.Credited;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();

            job.ResolutionStatus = ResolutionStatus.Credited;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Credited));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Credited));
        }

        [Test]
        [Description("Check if the Job is in Resolved status")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void Test_ResolutionStatus_Resolved()
        {
            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .Build();
            var newStatus = sut.GetCurrentResolutionStatus(job);
            //no line
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();

            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));

            job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Invalid)
                .Build();
            job.ResolutionStatus = ResolutionStatus.Resolved;
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.EqualTo(ResolutionStatus.Resolved));

            job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            newStatus = sut.GetCurrentResolutionStatus(job);
            Assert.That(newStatus, Is.Not.EqualTo(ResolutionStatus.Resolved));
        }

        [Test]
        [Description("Check that only Resolve and Credited status can be closed")]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public void TryCloseJob()
        {
            var job = new Job();

            foreach(var resolutionStatus in ResolutionStatus.AllStatus)
            {
                job.ResolutionStatus = resolutionStatus;

                if (resolutionStatus == ResolutionStatus.Resolved
                    || resolutionStatus.Value == ResolutionStatus.Credited)
                {
                    Assert.That(sut.TryCloseJob(job), Is.EqualTo(job.ResolutionStatus | ResolutionStatus.Closed));
                }
                else
                {
                    Assert.That(sut.TryCloseJob(job), Is.EqualTo(job.ResolutionStatus));
                }
            }
        }
    }

    [TestFixture]
    public class JobResolutionStatusStepForwardTests
    {
        private Mock<IJobRepository> jobRepository;
        private Mock<IAssigneeReadRepository> assigneeReadRepository;
        private Mock<ILineItemSearchReadRepository> lineItemRepository;
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IUserRepository> userRepository;
        private Mock<IStopService> stopService;
        private Mock<IActivityService> activityService;
        private Mock<IWellStatusAggregator> wellStatusAggregator;


        [SetUp]
        public void SetUp()
        {
            this.jobRepository = new Mock<IJobRepository>();
            this.assigneeReadRepository = new Mock<IAssigneeReadRepository>();
            this.lineItemRepository = new Mock<ILineItemSearchReadRepository>();
            this.userNameProvider = new Mock<IUserNameProvider>();
            this.userRepository = new Mock<IUserRepository>();
            this.stopService = new Mock<IStopService>();
            this.activityService = new Mock<IActivityService>();
            this.wellStatusAggregator = new Mock<IWellStatusAggregator>();
        }

        [Test]
        [TestCaseSource(typeof(JobResolutionStatusTestsSource), nameof(JobResolutionStatusTestsSource.StepForward))]
        [Category("JobResolutionStatus")]
        [Category("JobService")]
        public ResolutionStatus JobResolutionStatusStepForward(Job job, IUserThresholdService userThresholdService, IDateThresholdService dateThresholdService)
        {
            var sut = new JobService(
                jobRepository.Object,
                userThresholdService,
                dateThresholdService,
                assigneeReadRepository.Object,
                lineItemRepository.Object,
                userNameProvider.Object,
                userRepository.Object,
                stopService.Object,
                activityService.Object,
                wellStatusAggregator.Object);

            return sut.StepForward(job);
        }

        [Test]
        public void JobResolutionStatusStepBack()
        {
            var mockUserThresholdService = new Mock<IUserThresholdService>();
            var mockDateThresholdService = new Mock<IDateThresholdService>();

            var job = JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingApproval)
                .Build();

            mockDateThresholdService.Setup(p => p.GracePeriodEnd(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>())).Returns(DateTime.Now);
            mockUserThresholdService.Setup(p => p.UserHasRequiredCreditThreshold(It.IsAny<Job>())).Returns(true);

            var sut = new JobService(
                jobRepository.Object,
                mockUserThresholdService.Object,
                mockDateThresholdService.Object,
                assigneeReadRepository.Object,
                lineItemRepository.Object,
                userNameProvider.Object,
                userRepository.Object,
                stopService.Object,
                activityService.Object,
                wellStatusAggregator.Object);

            Assert.That(sut.StepBack(job), Is.EqualTo(ResolutionStatus.ApprovalRejected));
        }
    }

    class JobResolutionStatusTestsSource
    {
        public static IEnumerable StepForward
        {
            get
            {
                yield return new TestCaseData(Imported(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.DriverCompleted)
                    .SetDescription("Job should move to DriverCompleted");

                /*** DriverCompleted ***/
                yield return new TestCaseData(DriverCompletedOutWindowComplainNoActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should move to Close");

                yield return new TestCaseData(DriverCompletedOutWindowComplainWithActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("DriverCompleted Job should move to ActionRequired");

                yield return new TestCaseData(DriverCompletedInWindowComplainWithActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("DriverCompleted Job should move to ActionRequired");

                yield return new TestCaseData(DriverCompletedInWindowComplainNoActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should stay in DriverCompleted");

                yield return new TestCaseData(DriverCompletedInWindowComplainWithCreditAction(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should stay in DriverCompleted");

                yield return new TestCaseData(DriverCompletedOutWindowComplainWithCreditAction(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should move to Close Driver Completed");

                yield return new TestCaseData(DriverCompletedInWindowComplainWithActionZeroQuantity(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should stay in DriverCompleted");

                yield return new TestCaseData(DriverCompletedOutWindowComplainWithActionZeroQuantity(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.DriverCompleted)
                    .SetDescription("DriverCompleted Job should move to Close Driver Completed");

                /*** ManuallyCompleted ***/
                yield return new TestCaseData(ManuallyCompletedOutWindowComplainNoActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.ManuallyCompleted)
                    .SetDescription("ManuallyCompleted Job should move to Close");

                yield return new TestCaseData(ManuallyCompletedOutWindowComplainWithActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("ManuallyCompleted Job should move to ActionRequired");

                yield return new TestCaseData(ManuallyCompletedInWindowComplainWithActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("ManuallyCompleted Job should move to ActionRequired");

                yield return new TestCaseData(ManuallyCompletedInWindowComplainNoActions(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ManuallyCompleted)
                    .SetDescription("ManuallyCompleted Job should stay in ManuallyCompleted");

                yield return new TestCaseData(ManuallyCompletedInWindowComplainWithCreditAction(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ManuallyCompleted)
                    .SetDescription("ManuallyCompleted Job should stay in ManuallyCompleted");

                yield return new TestCaseData(ManuallyCompletedOutWindowComplainWithCreditAction(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.ManuallyCompleted)
                    .SetDescription("ManuallyCompleted Job should move to Close Driver Completed");

                yield return new TestCaseData(ManuallyCompletedInWindowComplainWithActionZeroQuantity(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ManuallyCompleted)
                    .SetDescription("ManuallyCompleted Job should stay in ManuallyCompleted");

                /*** ActionRequired ***/
                yield return new TestCaseData(ActionRequired(true), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.ActionRequired)
                    .SetDescription("ActionRequired Job should stay in ActionRequired");

                yield return new TestCaseData(ActionRequired(false), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.PendingSubmission)
                    .SetDescription("ActionRequired Job should move to PendingSubmission");

                ///*** PendingSubmission ***/
                yield return new TestCaseData(GoodPendingSubmission(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Approved)
                    .SetDescription("PendingSubmission Job should move to Approved");

                yield return new TestCaseData(GoodPendingSubmission(), CreateUserThresholdService(false), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.PendingApproval)
                    .SetDescription("PendingSubmission Job should not move to Approved");

                yield return new TestCaseData(BadPendingSubmission(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.PendingApproval)
                    .SetDescription("PendingApproval Job should not move to Approved");

                yield return new TestCaseData(BadPendingSubmission(), CreateUserThresholdService(false), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.PendingApproval)
                    .SetDescription("PendingApproval Job should not move to Approved");

                ///*** Approval Rejected ***/
                yield return new TestCaseData(GoodApprovalRejected(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Approved)
                    .SetDescription("ApprovalRejected Job should move to Approved");

                yield return new TestCaseData(GoodApprovalRejected(), CreateUserThresholdService(false), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.ApprovalRejected)
                    .SetDescription("ApprovalRejected Job should not move to Approved");

                yield return new TestCaseData(BadApprovalRejected(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.ApprovalRejected)
                    .SetDescription("PendingApproval Job should not move to Approved");

                yield return new TestCaseData(BadApprovalRejected(), CreateUserThresholdService(false), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.ApprovalRejected)
                    .SetDescription("ApprovalRejected Job should not move to Approved");

                /*** Approved ***/
                yield return new TestCaseData(Approved(true), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Credited)
                    .SetDescription("Approved Job should not move to Credited");

                yield return new TestCaseData(Approved(false), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Resolved)
                    .SetDescription("Approved Job should not move to Resolved");

                /*** Credited ***/
                yield return new TestCaseData(CreditedOutWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.Credited)
                    .SetDescription("Credited Job should move  to Closed - Credited");

                yield return new TestCaseData(CreditedInWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.Credited)
                    .SetDescription("Credited Job should stay in Credited");

                /*** Resolved ***/
                yield return new TestCaseData(ResolvedOutWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now))
                    .Returns(ResolutionStatus.Closed | ResolutionStatus.Resolved)
                    .SetDescription("Credited Job should move to Closed - Resolved");

                yield return new TestCaseData(ResolvedInWindowComplain(), CreateUserThresholdService(true), DateThresholdService(DateTime.Now.AddDays(2)))
                    .Returns(ResolutionStatus.Resolved)
                    .SetDescription("Credited Job should stay in Resolved");
            }
        }

        internal static IUserThresholdService CreateUserThresholdService(bool withinThreshol)
        {
            var mock = new Mock<IUserThresholdService>();

            mock.Setup(p => p.UserHasRequiredCreditThreshold(It.IsAny<Job>())).Returns(withinThreshol);

            return mock.Object;
        }

        private static IDateThresholdService DateThresholdService(DateTime date)
        {
            var mock = new Mock<IDateThresholdService>();

            mock.Setup(p => p.GracePeriodEnd(It.IsAny<DateTime>(), It.IsAny<int>(),It.IsAny<int>())).Returns(date);

            return mock.Object;
        }

        private static Job DriverCompletedOutWindowComplainNoActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedOutWindowComplainWithActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedInWindowComplainWithActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedInWindowComplainNoActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedInWindowComplainWithActionZeroQuantity()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedOutWindowComplainWithActionZeroQuantity()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedInWindowComplainWithCreditAction()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job DriverCompletedOutWindowComplainWithCreditAction()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.DriverCompleted)
                .Build();
        }

        private static Job ManuallyCompletedOutWindowComplainNoActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ManuallyCompletedOutWindowComplainWithActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ManuallyCompletedInWindowComplainWithActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ManuallyCompletedInWindowComplainNoActions()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ManuallyCompletedInWindowComplainWithActionZeroQuantity()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ManuallyCompletedOutWindowComplainWithActionZeroQuantity()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ManuallyCompletedInWindowComplainWithCreditAction()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ManuallyCompletedOutWindowComplainWithCreditAction()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ManuallyCompleted)
                .Build();
        }

        private static Job ActionRequired(bool addNotDefinedAction)
        {
            var job = JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.ResolutionStatus = ResolutionStatus.ActionRequired)
                .Build();

            if (addNotDefinedAction)
            {
                job.LineItems.Add(LineItemFactory.New.AddNotDefinedAction().Build());
            }
            else
            {
                job.LineItems.Add(LineItemFactory.New.AddCreditAction().Build());
            }

            return job;
        }

        private static Job Imported()
        {
            return JobFactory.New
                .With(p => p.ResolutionStatus = ResolutionStatus.Imported)
                .Build();
        }

        private static Job GoodPendingSubmission()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingSubmission)
                .Build();
        }

        private static Job BadPendingSubmission()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddNotDefinedAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.PendingSubmission)
                .Build();
        }

        private static Job GoodApprovalRejected()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ApprovalRejected)
                .Build();
        }

        private static Job BadApprovalRejected()
        {
            return JobFactory.New
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().AddNotDefinedAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.ApprovalRejected)
                .Build();
        }

        private static Job Approved(bool withCredits)
        {
            var job = JobFactory.New
                .With(p => p.ResolutionStatus = ResolutionStatus.Approved)
                .Build();

            if (withCredits)
            {
                job.LineItems.Add(LineItemFactory.New.AddCreditAction().Build());
            }
            else
            {
                job.LineItems.Add(LineItemFactory.New.AddCloseAction().Build());
            }

            return job;
        }

        private static Job CreditedOutWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Credited)
                .Build();
        }

        private static Job CreditedInWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCreditAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Credited)
                .Build();
        }

        private static Job ResolvedOutWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now.AddDays(-30) })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Resolved)
                .Build();
        }

        private static Job ResolvedInWindowComplain()
        {
            return JobFactory.New
                .With(p => p.JobRoute = new Well.Domain.ValueObjects.JobRoute { RouteDate = DateTime.Now })
                .With(p => p.LineItems.Add(LineItemFactory.New.AddCloseAction().Build()))
                .With(p => p.ResolutionStatus = ResolutionStatus.Resolved)
                .Build();
        }
    }
}