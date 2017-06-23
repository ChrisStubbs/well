using System.Linq;

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
        private Mock<ICreditThresholdRepository> _creditThresholdRepository;
        private SubmitActionValidation validator;
      

        [SetUp]
        public virtual void SetUp()
        {
            userNameProvider = new Mock<IUserNameProvider>();
            userRepository = new Mock<IUserRepository>();
            dateThresholdService = new Mock<IDateThresholdService>();
            _creditThresholdRepository = new Mock<ICreditThresholdRepository>();

            validator = new SubmitActionValidation(userNameProvider.Object, userRepository.Object,
                dateThresholdService.Object, _creditThresholdRepository.Object);
        }

        public class TheValidateUserForCreditingMethod : SubmitActionValidationTests
        {
            [Test]
            public void ShouldReturnInvalidIfUserNotFound()
            {
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                userRepository.Setup(x => x.GetByIdentity("Me")).Returns((User)null);

                var result = validator.ValidateUserForCrediting();

                Assert.That(result.IsValid, Is.False);
            }

            [Test]
            public void ShouldReturnInvalidIfNoThresholdLevel()
            {
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                userRepository.Setup(x => x.GetByIdentity("Me")).Returns(new User { ThresholdLevelId = null });

                var result = validator.ValidateUserForCrediting();

                Assert.That(result.IsValid, Is.False);
            }

            [Test]
            public void ShouldReturnValidIfUserFoundAndThresholdLevelSet()
            {
                userNameProvider.Setup(x => x.GetUserName()).Returns("Me");
                userRepository.Setup(x => x.GetByIdentity("Me")).Returns(new User { ThresholdLevelId = 1 });

                var result = validator.ValidateUserForCrediting();

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
                user = new User { Id = 1 ,ThresholdLevelId = 1};
                stubbedValidator = new Mock<SubmitActionValidation>(userNameProvider.Object, userRepository.Object,
                    dateThresholdService.Object, _creditThresholdRepository.Object) {CallBase = true};
            }

            [Test]
            public void ShouldReturnInvalidIfUserNotFound()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns((User)null);
                var result = validator.Validate(submitAction, jobs);
                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"User not found (Me). Can not submit actions"));
            }

            [Test]
            public void ShouldReturnInvalidIfUserNotAssignedJobs()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob> { new UserJob { JobId = 2 } });
                var result = validator.Validate(submitAction, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"User not assigned to all the items selected can not submit actions"));
            }

            [Test]
            public void ShouldReturnInvalidIfNotAllJobsPendingSubmission()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.ActionRequired });

                var result = validator.Validate(submitAction, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo("There are no jobs 'Pending Submission' for the selected items"));
            }

            [Test]
            public void ShouldReturnInvalidIfAnyJobsNotPendingSubmission()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });
                this.jobs.Add(new Job { Id = 1,InvoiceNumber="Inv1", ResolutionStatus = ResolutionStatus.ActionRequired });

                var result = validator.Validate(submitAction, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Can not submit actions for jobs. " +
                                                       $"The following jobs are not in Pending Submission State " +
                                                       $"JobId:1 Invoice:Inv1 Status: 4 - Action Required ."));
            }

            [Test]
            public void ShouldReturnInvalidIfEarliestSubmissionDateHasNotBeenReached()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });

                stubbedValidator.Setup(x => x.HasEarliestSubmitDateBeenReached(jobs)).Returns(new SubmitActionResult { IsValid = false, Message = "Error" });

                var result = stubbedValidator.Object.Validate(submitAction, jobs);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Error"));
            }

            [Test]
            public void ShouldNotCallValidateUserForCreditingIfNoCredits()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });

                stubbedValidator.Setup(x => x.HasEarliestSubmitDateBeenReached(jobs)).Returns(new SubmitActionResult { IsValid = true });
                stubbedValidator.Setup(x => x.HaveItemsToCredit(jobs)).Returns(false);
                var result = stubbedValidator.Object.Validate(submitAction, jobs);

                stubbedValidator.Verify(x => x.ValidateUserForCrediting(), Times.Never);
                Assert.That(result.IsValid, Is.True);

            }

            [Test]
            public void ShouldReturnInvalidIfUserNotValidForCrediting()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });

                stubbedValidator.Setup(x => x.HasEarliestSubmitDateBeenReached(jobs)).Returns(new SubmitActionResult { IsValid = true });
                stubbedValidator.Setup(x => x.HaveItemsToCredit(jobs)).Returns(true);
                stubbedValidator.Setup(x => x.ValidateUserForCrediting()).Returns(new SubmitActionResult { IsValid = false, Message = "User Not Valid" });

                var result = stubbedValidator.Object.Validate(submitAction, jobs);


                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"User Not Valid"));
            }

            [Test]
            public void ShouldReturnCallValidateForUserValidIfOk()
            {
                this.userRepository.Setup(x => x.GetByIdentity("Me")).Returns(user);
                this.userRepository.Setup(x => x.GetUserJobsByJobIds(submitAction.JobIds)).Returns(new List<UserJob>());
                this._creditThresholdRepository.Setup(x => x.GetById(user.ThresholdLevelId.Value))
                    .Returns(new CreditThreshold {ThresholdLevelId = user.ThresholdLevelId.Value, Threshold = 100});
                this.jobs.Add(new Job { ResolutionStatus = ResolutionStatus.PendingSubmission });

                stubbedValidator.Setup(x => x.HasEarliestSubmitDateBeenReached(jobs)).Returns(new SubmitActionResult { IsValid = true });
                stubbedValidator.Setup(x => x.HaveItemsToCredit(jobs)).Returns(true);
                stubbedValidator.Setup(x => x.ValidateUserForCrediting()).Returns(new SubmitActionResult { IsValid = true });
                
                var result = stubbedValidator.Object.Validate(submitAction, jobs);

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
            public void ShouldReturnInvalidIfRouteDateGreaterThatEarliestCreditDate()
            {
                unsubmittedJobs.Add(new Job { JobRoute = new JobRoute { JobId = 1, BranchId = 1, RouteDate = DateTime.Today } });
                unsubmittedJobs.Add(new Job { JobRoute = new JobRoute { JobId = 2, BranchId = 2, RouteDate = DateTime.Today } });
                unsubmittedJobs.Add(new Job { JobRoute = new JobRoute { JobId = 3, BranchId = 1, RouteDate = DateTime.Today } });

                dateThresholdService.Setup(x => x.EarliestSubmitDate(DateTime.Today, 1)).Returns(DateTime.Today);
                dateThresholdService.Setup(x => x.EarliestSubmitDate(DateTime.Today, 2)).Returns(DateTime.Today.AddDays(1));


                var result = validator.HasEarliestSubmitDateBeenReached(unsubmittedJobs.ToArray());

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Job nos: '2: earliest credit date: {DateTime.Today.AddDays(1)}' have not reached the earliest credit date so can not be submitted."));
            }

            [Test]
            public void ShouldReturnValidIfRouteDateEqualsThatEarliestCreditDate()
            {
                unsubmittedJobs.Add(new Job { JobRoute = new JobRoute { JobId = 1, BranchId = 1, RouteDate = DateTime.Today } });

                dateThresholdService.Setup(x => x.EarliestSubmitDate(DateTime.Today, 1)).Returns(DateTime.Today);
                dateThresholdService.Setup(x => x.EarliestSubmitDate(DateTime.Today, 2)).Returns(DateTime.Today.AddDays(1));

                var result = validator.HasEarliestSubmitDateBeenReached(unsubmittedJobs.ToArray());

                Assert.That(result.IsValid, Is.True);
            }

            [Test]
            public void ShouldReturnValidIfRouteDateLessThanThatEarliestCreditDate()
            {
                unsubmittedJobs.Add(new Job { JobRoute = new JobRoute { JobId = 1, BranchId = 1, RouteDate = DateTime.Today.AddDays(-1) } });

                dateThresholdService.Setup(x => x.EarliestSubmitDate(DateTime.Today, 1)).Returns(DateTime.Today);
                dateThresholdService.Setup(x => x.EarliestSubmitDate(DateTime.Today, 2)).Returns(DateTime.Today.AddDays(1));

                var result = validator.HasEarliestSubmitDateBeenReached(unsubmittedJobs.ToArray());

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

    }
}