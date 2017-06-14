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
    public class SubmitCreditActionValidationTests
    {
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IDateThresholdService> dateThresholdService;
        private Mock<IUserRepository> userRepository;
        private SubmitCreditActionValidation validator;

        [SetUp]
        public virtual void SetUp()
        {
            userNameProvider = new Mock<IUserNameProvider>();
            userRepository = new Mock<IUserRepository>();
            dateThresholdService = new Mock<IDateThresholdService>();

            validator = new SubmitCreditActionValidation(userNameProvider.Object, userRepository.Object, dateThresholdService.Object);
        }

        public class TheAllCreditItemsForInvoiceIncluded : SubmitCreditActionValidationTests
        {
            [Test]
            public void ShouldReturnInvalidResultIfNotAllInvoiceItemsIncluded()
            {
                IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems = new List<LineItemActionSubmitModel>
                {
                    new LineItemActionSubmitModel {JobId = 1, InvoiceNumber = "Inv1"},
                    new LineItemActionSubmitModel {JobId = 2, InvoiceNumber = "Inv2"},
                    new LineItemActionSubmitModel {JobId = 3, InvoiceNumber = "Inv3"},
                    new LineItemActionSubmitModel {JobId = 4, InvoiceNumber = "Inv1"},
                    new LineItemActionSubmitModel {JobId = 5, InvoiceNumber = "Inv2"},
                    new LineItemActionSubmitModel {JobId = 6, InvoiceNumber = "Inv5"},
                    new LineItemActionSubmitModel {JobId = 7, InvoiceNumber = "Inv1"},
                };

                SubmitActionModel submitAction = new SubmitActionModel { JobIds = new[] { 1, 2, 3 }, Action = DeliveryAction.Credit };

                var result = validator.AllCreditItemsForInvoiceIncluded(submitAction, allUnsubmittedItems);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Not all jobs have been submitted for credit for invoice nos 'Inv1,Inv2'"));
            }

            [Test]
            public void ShouldReturnValidResultIfAllInvoiceItemsIncluded()
            {
                IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems = new List<LineItemActionSubmitModel>
                {
                    new LineItemActionSubmitModel {JobId = 1, InvoiceNumber = "Inv1"},
                    new LineItemActionSubmitModel {JobId = 2, InvoiceNumber = "Inv2"},
                    new LineItemActionSubmitModel {JobId = 3, InvoiceNumber = "Inv3"},
                    new LineItemActionSubmitModel {JobId = 4, InvoiceNumber = "Inv1"},
                    new LineItemActionSubmitModel {JobId = 5, InvoiceNumber = "Inv2"},
                    new LineItemActionSubmitModel {JobId = 6, InvoiceNumber = "Inv5"},
                };

                SubmitActionModel submitAction = new SubmitActionModel { JobIds = new[] { 1, 2, 3, 4, 5, 6 }, Action = DeliveryAction.Credit };

                var result = validator.AllCreditItemsForInvoiceIncluded(submitAction, allUnsubmittedItems);

                Assert.That(result.IsValid, Is.True);
            }
        }

        public class TheEarliestCreditDateForItemsHasBeenReached : SubmitCreditActionValidationTests
        {
            [Test]
            public void ShouldReturnInvalidIfRouteDateGreaterThatEarliestCreditDate()
            {
                IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems = new List<LineItemActionSubmitModel>
                {
                    new LineItemActionSubmitModel {JobId = 1, InvoiceNumber = "Inv1", BranchId = 1, RouteDate = DateTime.Today},
                    new LineItemActionSubmitModel {JobId = 2, InvoiceNumber = "Inv2", BranchId = 2, RouteDate = DateTime.Today},
                    new LineItemActionSubmitModel {JobId = 3, InvoiceNumber = "Inv1", BranchId = 1, RouteDate = DateTime.Today},

                };
                dateThresholdService.Setup(x => x.EarliestCreditDate(DateTime.Today, 1)).Returns(DateTime.Today);
                dateThresholdService.Setup(x => x.EarliestCreditDate(DateTime.Today, 2)).Returns(DateTime.Today.AddDays(1));

                SubmitActionModel submitAction = new SubmitActionModel { JobIds = new[] { 1, 2, 3 }, Action = DeliveryAction.Credit };

                var result = validator.EarliestCreditDateForItemsHasBeenReached(submitAction, allUnsubmittedItems);

                Assert.That(result.IsValid, Is.False);
                Assert.That(result.Message, Is.EqualTo($"Invoice nos: 'Inv2: earliest credit date: {DateTime.Today.AddDays(1)}' have not reached the earliest credit date so can not be submitted."));
            }

            [Test]
            public void ShouldReturnValidIfRouteDateEqualsThatEarliestCreditDate()
            {
                IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems = new List<LineItemActionSubmitModel>
                {
                    new LineItemActionSubmitModel {JobId = 1, InvoiceNumber = "Inv1", BranchId = 1, RouteDate = DateTime.Today},
                };
                dateThresholdService.Setup(x => x.EarliestCreditDate(DateTime.Today, 1)).Returns(DateTime.Today);
                SubmitActionModel submitAction = new SubmitActionModel { JobIds = new[] { 1 }, Action = DeliveryAction.Credit };

                var result = validator.EarliestCreditDateForItemsHasBeenReached(submitAction, allUnsubmittedItems);

                Assert.That(result.IsValid, Is.True);
            }

            [Test]
            public void ShouldReturnValidIfRouteDateLessThanThatEarliestCreditDate()
            {
                IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems = new List<LineItemActionSubmitModel>
                {
                    new LineItemActionSubmitModel {JobId = 1, InvoiceNumber = "Inv1", BranchId = 1, RouteDate = DateTime.Today.AddDays(-1)},
                };
                dateThresholdService.Setup(x => x.EarliestCreditDate(DateTime.Today, 1)).Returns(DateTime.Today);
                SubmitActionModel submitAction = new SubmitActionModel { JobIds = new[] { 1 }, Action = DeliveryAction.Credit };

                var result = validator.EarliestCreditDateForItemsHasBeenReached(submitAction, allUnsubmittedItems);

                Assert.That(result.IsValid, Is.True);
            }
        }

        public class TheValidateUserForCreditingMethod : SubmitCreditActionValidationTests
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

        public class TheValidateMethod : SubmitCreditActionValidationTests
        {
            private Mock<SubmitCreditActionValidation> stubbedValidator;
            private readonly IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems = new List<LineItemActionSubmitModel>();

            [SetUp]
            public override void SetUp()
            {
                base.SetUp();
                stubbedValidator = new Mock<SubmitCreditActionValidation>(userNameProvider.Object, userRepository.Object, dateThresholdService.Object) { CallBase = true };
            }

            [Test]
            public void ShouldReturnValidIfNotCreditAction()
            {
                var result = stubbedValidator.Object.Validate(new SubmitActionModel { Action = DeliveryAction.NotDefined }, allUnsubmittedItems);

                Assert.That(result.IsValid, Is.True);
                stubbedValidator.Verify(x=> x.ValidateUserForCrediting(),Times.Never);
                stubbedValidator.Verify(x => x.AllCreditItemsForInvoiceIncluded(It.IsAny<SubmitActionModel>(),It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Never);
                stubbedValidator.Verify(x => x.EarliestCreditDateForItemsHasBeenReached(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Never);
            }

            [Test]
            public void ShouldReturnInValidIfUserNotSetupForCredting()
            {
                var actionModel = new SubmitActionModel { Action = DeliveryAction.Credit };
                var userValidateResult = new SubmitActionResult {IsValid = false, Message = "ErrorMsg"};

                stubbedValidator.Setup(x => x.ValidateUserForCrediting()).Returns(userValidateResult);

                var result = stubbedValidator.Object.Validate(actionModel, allUnsubmittedItems);

                Assert.That(result, Is.EqualTo(userValidateResult));
                stubbedValidator.Verify(x => x.ValidateUserForCrediting(), Times.Once);
                stubbedValidator.Verify(x => x.AllCreditItemsForInvoiceIncluded(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Never);
                stubbedValidator.Verify(x => x.EarliestCreditDateForItemsHasBeenReached(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Never);
            }

            [Test]
            public void ShouldReturnInValidIfNotAllInvoiceItemsIncluded()
            {
                var actionModel = new SubmitActionModel { Action = DeliveryAction.Credit };
                var userValidateResult = new SubmitActionResult { IsValid = true };
                var allInvoiceValuesIncluded = new SubmitActionResult { IsValid = false, Message = "ErrorMsg" };

                stubbedValidator.Setup(x => x.ValidateUserForCrediting()).Returns(userValidateResult);
                stubbedValidator.Setup(x => x.AllCreditItemsForInvoiceIncluded(actionModel,allUnsubmittedItems)).Returns(allInvoiceValuesIncluded);

                var result = stubbedValidator.Object.Validate(actionModel, allUnsubmittedItems);

                Assert.That(result, Is.EqualTo(allInvoiceValuesIncluded));
                stubbedValidator.Verify(x => x.ValidateUserForCrediting(), Times.Once);
                stubbedValidator.Verify(x => x.AllCreditItemsForInvoiceIncluded(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Once);
                stubbedValidator.Verify(x => x.EarliestCreditDateForItemsHasBeenReached(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Never);
            }

            [Test]
            public void ShouldReturnInEarliestCreditDatesHasNotBeenReached()
            {
                var actionModel = new SubmitActionModel { Action = DeliveryAction.Credit };
                var userValidateResult = new SubmitActionResult { IsValid = true };
                var allInvoiceValuesIncluded = new SubmitActionResult { IsValid = true, Message = "ErrorMsg" };
                var earliestCreditDate = new SubmitActionResult { IsValid = false, Message = "ErrorMsg" };

                stubbedValidator.Setup(x => x.ValidateUserForCrediting()).Returns(userValidateResult);
                stubbedValidator.Setup(x => x.AllCreditItemsForInvoiceIncluded(actionModel, allUnsubmittedItems)).Returns(allInvoiceValuesIncluded);
                stubbedValidator.Setup(x => x.EarliestCreditDateForItemsHasBeenReached(actionModel, allUnsubmittedItems)).Returns(earliestCreditDate);

                var result = stubbedValidator.Object.Validate(actionModel, allUnsubmittedItems);

              
                Assert.That(result, Is.EqualTo(earliestCreditDate));
                stubbedValidator.Verify(x => x.ValidateUserForCrediting(), Times.Once);
                stubbedValidator.Verify(x => x.AllCreditItemsForInvoiceIncluded(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Once);
                stubbedValidator.Verify(x => x.EarliestCreditDateForItemsHasBeenReached(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Once);
                stubbedValidator.Verify(x => x.EarliestCreditDateForItemsHasBeenReached(It.IsAny<SubmitActionModel>(), It.IsAny<IEnumerable<LineItemActionSubmitModel>>()), Times.Once);
            }


        }
    }
}