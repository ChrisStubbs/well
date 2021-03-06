﻿using System.Linq;
using PH.Well.Services.Contracts;
using PH.Well.UnitTests.Factories;

namespace PH.Well.UnitTests.Services.Validation
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services;
    using Well.Services.Validation;

    [TestFixture]
    public class SubmitActionValidationTests
    {
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IDateThresholdService> dateThresholdService;
        private Mock<IUserRepository> userRepository;
        private SubmitActionValidation validator;
        private Mock<IJobService> jobService;

        [SetUp]
        public virtual void SetUp()
        {
            userNameProvider = new Mock<IUserNameProvider>();
            userRepository = new Mock<IUserRepository>();
            dateThresholdService = new Mock<IDateThresholdService>();
            jobService = new Mock<IJobService>();

            validator = new SubmitActionValidation(userNameProvider.Object, userRepository.Object,
                dateThresholdService.Object, jobService.Object);
        }

        public class TheValidateUserForCreditingMethod : SubmitActionValidationTests
        {
            [Test]
            public void ShouldReturnInvalidIfUserNotFound()
            {
                SubmitActionResult result = new SubmitActionResult {IsValid = true};
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                userRepository.Setup(x => x.GetByIdentity("Me")).Returns((User)null);
                validator.ValidateUserForCrediting(result);

                Assert.That(result.IsValid, Is.False);
            }

            [Test]
            public void ShouldReturnInvalidIfNoThresholdLevel()
            {
                var result = new SubmitActionResult { IsValid = true };
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                userRepository.Setup(x => x.GetByIdentity("Me")).Returns(new User { CreditThresholdId = null });
                validator.ValidateUserForCrediting(result);

                Assert.That(result.IsValid, Is.False);
            }

            [Test]
            public void ShouldReturnValidIfUserFoundAndThresholdLevelSet()
            {
                var result = new SubmitActionResult() {IsValid = true};
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                userRepository.Setup(x => x.GetByIdentity("Me")).Returns(new User { CreditThresholdId = 1 });
                validator.ValidateUserForCrediting(result);

                Assert.That(result.IsValid, Is.True);
            }
        }

        public class TheValidateMethod : SubmitActionValidationTests
        {
            private List<Job> jobs;
            private SubmitActionModel submitAction;
            private User user;
            private Mock<SubmitActionValidation> stubbedValidator;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                jobs = new List<Job>();
                submitAction = new SubmitActionModel { JobIds = new[] { 1, 2, 3 } };
                this.userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                user = new User { Id = 1, CreditThresholdId = 1 };
                stubbedValidator = new Mock<SubmitActionValidation>(userNameProvider.Object, userRepository.Object,
                        dateThresholdService.Object, jobService.Object)
                    {CallBase = true};
            }

            [Test]
            public void ShouldReturnInvalidIfUserNotFound()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns((User)null);
                var result = validator.Validate(submitAction.JobIds, jobs);
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"User not found (Me). Can not submit exceptions"));
            }

            [Test]
            public void ShouldReturnInvalidIfUserNotAssignedJobs()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob> { new UserJob { JobId = 2 } });
                var result = validator.Validate(submitAction.JobIds, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo("You may only submit jobs which are assigned to you and are Pending Submission. " +
                                                          "If both criteria are met, it is possible that this job has been re-assigned. " +
                                                          "Please refresh data and try again."));
            }

            [Test]
            public void ShouldReturnInvalidIfNotAllJobsPendingSubmission()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.ActionRequired });

                var result = validator.Validate(submitAction.JobIds, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo("There are no jobs 'Pending Submission' for the selected items"));
            }

            [Test]
            public void ShouldReturnInvalidIfAnyJobsNotPendingSubmission()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });
                this.jobs.Add(new Job { Id = 1, InvoiceNumber = "Inv1", ResolutionStatus = ResolutionStatus.ActionRequired });
                this.jobs.Add(new Job { Id = 4, InvoiceNumber = "sd", ResolutionStatus = ResolutionStatus.PendingApproval });

                var result = validator.Validate(submitAction.JobIds, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Can not submit exceptions for jobs. " +
                                                       $"The following jobs are not in Pending Submission / Pending Approval State " +
                                                       $"JobId:1 Invoice:Inv1 Status: 4 - Action Required ."));
            }

            [Test]
            public void ShouldReturnValidWithWarningIfEarliestSubmissionDateHasNotBeenReached()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(
                    new Job {ResolutionStatus = ResolutionStatus.PendingSubmission, JobRoute = new JobRoute()});

                var date = DateTime.Now;
                dateThresholdService
                    .Setup(x => x.GracePeriodEnd(It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(date.AddDays(1));

                stubbedValidator.Setup(x => x.ValidateJobsCanBeEdited(It.IsAny<IEnumerable<Job>>(),
                    It.IsAny<SubmitActionResult>()));

                var result = stubbedValidator.Object.Validate(submitAction.JobIds, jobs);

                Assert.True(result.IsValid);
                Assert.True(result.Warnings.Any());
            }

            [Test]
            public void ShouldNotCallValidateUserForCreditingIfNoCredits()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });

                var setResultValid = new Action<IEnumerable<Job>, SubmitActionResult>((j, r) => r.IsValid = true);
                stubbedValidator.Setup(x => x.HasEarliestSubmitDateBeenReached(jobs, It.IsAny<SubmitActionResult>())).Callback(setResultValid);
                stubbedValidator.Setup(x => x.ValidateJobsCanBeEdited(jobs, It.IsAny<SubmitActionResult>())).Callback(setResultValid);
                stubbedValidator.Setup(x => x.HaveItemsToCredit(jobs)).Returns(false);
                var result = stubbedValidator.Object.Validate(submitAction.JobIds, jobs);

                stubbedValidator.Verify(x => x.ValidateUserForCrediting(new SubmitActionResult()), Times.Never);
                Assert.That(result.IsValid, Is.True);
            }

            [Test]
            public void ShouldReturnInvalidIfUserNotValidForCrediting()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });

                var setResultValid = new Action<IEnumerable<Job>, SubmitActionResult>((j, r) => r.IsValid = true);
                stubbedValidator.Setup(x => x.HasEarliestSubmitDateBeenReached(jobs, It.IsAny<SubmitActionResult>())).Callback(setResultValid);
                stubbedValidator.Setup(x => x.ValidateJobsCanBeEdited(jobs, It.IsAny<SubmitActionResult>())).Callback(setResultValid);

                stubbedValidator.Setup(x => x.HaveItemsToCredit(jobs)).Returns(true);
                stubbedValidator.Setup(x => x.ValidateUserForCrediting(It.IsAny<SubmitActionResult>())).Callback<SubmitActionResult>(r =>
                {
                    r.IsValid = false;
                    r.Message = "User Not Valid";
                });

                var result = stubbedValidator.Object.Validate(submitAction.JobIds, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"User Not Valid"));
            }

            [Test]
            public void ShouldReturnCallValidateForUserValidIfOk()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });

                stubbedValidator.Setup(x => x.HasEarliestSubmitDateBeenReached(jobs, It.IsAny<SubmitActionResult>()))
                    .Callback<IList<Job>, SubmitActionResult>((j, r) => r.IsValid = true);
                stubbedValidator.Setup(x => x.ValidateJobsCanBeEdited(jobs, It.IsAny<SubmitActionResult>()))
                    .Callback<IEnumerable<Job>, SubmitActionResult>((j, r) => r.IsValid = true);
                stubbedValidator.Setup(x => x.ValidateUserForCrediting(It.IsAny<SubmitActionResult>()))
                    .Callback<SubmitActionResult>(r => r.IsValid = true);
                stubbedValidator.Setup(x => x.HaveItemsToCredit(jobs)).Returns(true);

                var result = stubbedValidator.Object.Validate(submitAction.JobIds, jobs);

                Assert.That(result.IsValid, Is.True);
            }

        }

        public class TheEarliestCreditDateForItemsHasBeenReached : SubmitActionValidationTests
        {
            private List<Job> unsubmittedJobs;
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                unsubmittedJobs = new List<Job>();
            }

            [Test]
            public void ShouldReturnValidWithWarningIfRouteDateGreaterThatEarliestCreditDate()
            {
                unsubmittedJobs.Add(new Job { Id = 1, JobRoute = new JobRoute { JobId = 1, BranchId = 1, RouteDate = DateTime.Today } });
                unsubmittedJobs.Add(new Job { Id = 2, JobRoute = new JobRoute { JobId = 2, BranchId = 2, RouteDate = DateTime.Today } });
                unsubmittedJobs.Add(new Job { Id = 3, JobRoute = new JobRoute { JobId = 3, BranchId = 1, RouteDate = DateTime.Today } });

                dateThresholdService.Setup(x => x.GracePeriodEnd(DateTime.Today, 1, 0)).Returns(DateTime.Today);
                dateThresholdService.Setup(x => x.GracePeriodEnd(DateTime.Today, 2, 0)).Returns(DateTime.Today.AddDays(1));

                var result = new SubmitActionResult {IsValid = true};
                validator.HasEarliestSubmitDateBeenReached(unsubmittedJobs.ToArray(), result);

                Assert.That(result.IsValid, Is.True);
                Assert.True(result.Warnings.Any());
            }

            [Test]
            public void ShouldReturnValidIfRouteDateEqualsThatEarliestCreditDate()
            {
                unsubmittedJobs.Add(new Job { JobRoute = new JobRoute { JobId = 1, BranchId = 1, RouteDate = DateTime.Today } });

                dateThresholdService.Setup(x => x.GracePeriodEnd(DateTime.Today, 1, 0)).Returns(DateTime.Today);
                dateThresholdService.Setup(x => x.GracePeriodEnd(DateTime.Today, 2, 0)).Returns(DateTime.Today.AddDays(1));

                var result = new SubmitActionResult { IsValid = true };
                validator.HasEarliestSubmitDateBeenReached(unsubmittedJobs.ToArray(), result);

                Assert.That(result.IsValid, Is.True);
            }

            [Test]
            public void ShouldReturnValidIfRouteDateLessThanThatEarliestCreditDate()
            {
                unsubmittedJobs.Add(new Job { JobRoute = new JobRoute { JobId = 1, BranchId = 1, RouteDate = DateTime.Today.AddDays(-1) } });

                dateThresholdService.Setup(x => x.RouteGracePeriodEnd(DateTime.Today, 1)).Returns(DateTime.Today);
                dateThresholdService.Setup(x => x.RouteGracePeriodEnd(DateTime.Today, 2)).Returns(DateTime.Today.AddDays(1));

                var result = new SubmitActionResult { IsValid = true };
                validator.HasEarliestSubmitDateBeenReached(unsubmittedJobs.ToArray(), result);

                Assert.That(result.IsValid, Is.True);
            }
        }

        public class TheHaveItemsToCreditMethod : SubmitActionValidationTests
        {
            [Test]
            public void ShouldReturnTrueIfHaveCreditItems()
            {

                var job1 = new Job { LineItems = new List<LineItem> { new LineItem { LineItemActions = new List<LineItemAction> { new LineItemAction { DeliveryAction = DeliveryAction.Credit } } } } };
                var job2 = new Job { LineItems = new List<LineItem> { new LineItem { LineItemActions = new List<LineItemAction> { new LineItemAction { DeliveryAction = DeliveryAction.Close } } } } };
                var jobs = new List<Job> { job1, job2 };

                Assert.That(validator.HaveItemsToCredit(jobs), Is.True);
            }

            [Test]
            public void ShouldReturnFalseIfNoCreditItems()
            {

                var job1 = new Job { LineItems = new List<LineItem> { new LineItem { LineItemActions = new List<LineItemAction> { new LineItemAction { DeliveryAction = DeliveryAction.Close } } } } };
                var job2 = new Job { LineItems = new List<LineItem> { new LineItem { LineItemActions = new List<LineItemAction> { new LineItemAction { DeliveryAction = DeliveryAction.Close } } } } };
                var jobs = new List<Job> { job1, job2 };

                Assert.That(validator.HaveItemsToCredit(jobs), Is.False);
            }

        }

        public class TheValidateJobsCanBeEdited : SubmitActionValidationTests
        {
            [Test(Description = "Job service should be used to detemine whether jobs can be edited or not")]
            public void ShouldUseJobServiceToValidateCanBeEdited()
            {
                var job = JobFactory.New.Build();
                validator.ValidateJobsCanBeEdited(new[] {job}, new SubmitActionResult());
                jobService.Verify(x => x.CanEdit(job, It.IsAny<string>()), Times.Once);
            }
        }
    }
}