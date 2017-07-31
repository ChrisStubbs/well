namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    using Well.Common.Contracts;
    using Well.Domain;
    using Well.Domain.Enums;
    using Repositories.Contracts;
    using Well.Services;
    using Well.Domain.ValueObjects;

    [TestFixture]
    public class UserThresholdServiceTests
    {
        private Mock<ICreditThresholdRepository> creditThresholdRepository;
        private Mock<IUserRepository> userRepository;

        private UserThresholdService service;
        private Mock<IUserNameProvider> userNameProvider;

        [SetUp]
        public virtual void SetUp()
        {
            creditThresholdRepository = new Mock<ICreditThresholdRepository>(MockBehavior.Strict);
            userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            userNameProvider.Setup(x => x.GetUserName()).Returns("foo");

            service = new UserThresholdService(creditThresholdRepository.Object, userRepository.Object, userNameProvider.Object);
        }

        public class TheCanUserCreditMethod : UserThresholdServiceTests
        {
            [Test]
            public void ShouldReturnTrueWhenUserCanCreditBelowTheThresholdAmount()
            {
                var userThreshold = new CreditThreshold { Id = 5, Threshold = 100 };
                var threshold2 = new CreditThreshold { Id = 50, Threshold = 101 };
                var user = new User {CreditThresholdId = userThreshold.Id};
                var thresholds = new List<CreditThreshold> { userThreshold, threshold2 };

                var creditValue = 100;

                userRepository.Setup(x => x.GetByIdentity(It.IsAny<string>())).Returns(user);
                creditThresholdRepository.Setup(x => x.GetById(userThreshold.Id)).Returns(userThreshold);

                var thresholdResponse = service.CanUserCredit(creditValue);

                Assert.IsTrue(thresholdResponse.CanUserCredit);

                userRepository.Verify(x => x.GetByIdentity(It.IsAny<string>()), Times.Once);
                creditThresholdRepository.Verify(x => x.GetById(userThreshold.Id), Times.Once);
            }

            [Test]
            public void ShouldReturnFalseWhenUserCanNotCreditAboveTheThresholdAmount()
            {
                var userThreshold = new CreditThreshold { Id = 5, Threshold = 100 };
                var threshold2 = new CreditThreshold { Id = 50, Threshold = 101 };
                var user = new User { CreditThresholdId = userThreshold.Id };
                var thresholds = new List<CreditThreshold> { userThreshold, threshold2 };

                var username = "foo";
                var creditValue = 102;

                userRepository.Setup(x => x.GetByIdentity(username)).Returns(user);
                creditThresholdRepository.Setup(x => x.GetById(userThreshold.Id)).Returns(userThreshold);

                var thresholdResponse = service.CanUserCredit(creditValue);

                Assert.IsFalse(thresholdResponse.CanUserCredit);

                userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
                creditThresholdRepository.Verify(x => x.GetById(userThreshold.Id), Times.Once);
            }
        }

        public class TheUserHasRequiredCreditThresholdMethod : UserThresholdServiceTests
        {

            private Mock<UserThresholdService> stubbedUserThreshold;
            [SetUp]
            public void SetUp()
            {
                base.SetUp();
                stubbedUserThreshold = new Mock<UserThresholdService>(
                            creditThresholdRepository.Object, 
                            userRepository.Object, userNameProvider.Object) { CallBase = true };
            }

            [Test]
            public void ShouldReturnTrueIfNoCreditActions()
            {
                var job = new Job();
                Assert.That(service.UserHasRequiredCreditThreshold(job), Is.EqualTo(true));
            }

            [Test]
            public void ShouldPassCreditValueToCanUserCreditAndReturnResultOfCallTrue()
            {

                var job = GetJobWithCredit();
                stubbedUserThreshold.Setup(x => x.CanUserCredit(It.IsAny<decimal>())).Returns(new ThresholdResponse {CanUserCredit = true});

                Assert.That(stubbedUserThreshold.Object.UserHasRequiredCreditThreshold(job), Is.EqualTo(true));

                stubbedUserThreshold.Verify(x=> x.CanUserCredit(140M),Times.Once);
            }

            [Test]
            public void ShouldPassCreditValueToCanUserCreditAndReturnResultOfCallFalse()
            {

                var job = GetJobWithCredit();
                stubbedUserThreshold.Setup(x => x.CanUserCredit(It.IsAny<decimal>())).Returns(new ThresholdResponse { CanUserCredit = false });

                Assert.That(stubbedUserThreshold.Object.UserHasRequiredCreditThreshold(job), Is.EqualTo(false));

                stubbedUserThreshold.Verify(x => x.CanUserCredit(140M), Times.Once);
            }


            private Job GetJobWithCredit()
            {
                return new Job
                {
                    LineItems = new List<LineItem>
                    {
                        new LineItem
                        {
                            NetPrice = 10M,
                            LineItemActions = new List<LineItemAction>
                            {
                                new LineItemAction {DeliveryAction = DeliveryAction.Credit, Quantity = 10}
                            }
                        },
                        new LineItem
                        {
                            NetPrice = 20M,
                            LineItemActions = new List<LineItemAction>
                            {
                                new LineItemAction {DeliveryAction = DeliveryAction.Credit, Quantity = 2},
                                new LineItemAction {DeliveryAction = DeliveryAction.Close, Quantity = 2}
                            }
                        }
                    }
                };
            }
        }

    }
}