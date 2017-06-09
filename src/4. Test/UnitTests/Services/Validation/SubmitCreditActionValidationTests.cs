namespace PH.Well.UnitTests.Services.Validation
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Common.Contracts;
    using Well.Domain.Enums;
    using Well.Domain.ValueObjects;
    using Well.Services.Validation;

    [TestFixture]
    public class SubmitCreditActionValidationTests
    {
        private Mock<IUserNameProvider> userNameProvider;
        private Mock<IUserRepository> userRepository;
        private SubmitCreditActionValidation validator;

        [SetUp]
        public void Setup()
        {
            userNameProvider = new Mock<IUserNameProvider>();
            userRepository = new Mock<IUserRepository>();
            validator = new SubmitCreditActionValidation(userNameProvider.Object, userRepository.Object);
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

                SubmitActionModel submitAction = new SubmitActionModel { JobIds = new[] { 1, 2, 3, 4, 6 }, Action = DeliveryAction.Credit };

                var result = validator.AllCreditItemsForInvoiceIncluded(submitAction, allUnsubmittedItems);

                Assert.That(result.IsValid, Is.False);
            }
        }
    }
}