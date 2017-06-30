namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
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
            this.creditThresholdRepository = new Mock<ICreditThresholdRepository>(MockBehavior.Strict);
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.userNameProvider = new Mock<IUserNameProvider>(MockBehavior.Strict);
            this.userNameProvider.Setup(x => x.GetUserName()).Returns("foo");

            this.service = new UserThresholdService(this.creditThresholdRepository.Object, this.userRepository.Object, this.userNameProvider.Object);
        }

        public class TheCanUserCreditMethod : UserThresholdServiceTests
        {
            [Test]
            public void ShouldReturnTrueWhenUserCanCreditBelowTheThresholdAmount()
            {
                var user = new User { ThresholdLevelId = 5 };
                var threshold = new CreditThreshold { ThresholdLevelId = 5, Threshold = 100 };
                var threshold2 = new CreditThreshold { ThresholdLevelId = 50, Threshold = 101 };

                var thresholds = new List<CreditThreshold> { threshold, threshold2 };

                //var username = "foo";
                var creditValue = 100;

                this.userRepository.Setup(x => x.GetByIdentity(It.IsAny<string>())).Returns(user);
                this.creditThresholdRepository.Setup(x => x.GetAll()).Returns(thresholds);

                var thresholdResponse = this.service.CanUserCredit(creditValue);

                Assert.IsTrue(thresholdResponse.CanUserCredit);

                this.userRepository.Verify(x => x.GetByIdentity(It.IsAny<string>()), Times.Once);
                this.creditThresholdRepository.Verify(x => x.GetAll(), Times.Once);
            }

            [Test]
            public void ShouldReturnFalseWhenUserCanNotCreditAboveTheThresholdAmount()
            {
                var user = new User { ThresholdLevelId = 5 };
                var threshold = new CreditThreshold { ThresholdLevelId = 5, Threshold = 101 };
                var threshold2 = new CreditThreshold { ThresholdLevelId = 50, Threshold = 100 };

                var thresholds = new List<CreditThreshold> { threshold, threshold2 };

                var username = "foo";
                var creditValue = 102;

                this.userRepository.Setup(x => x.GetByIdentity(username)).Returns(user);
                this.creditThresholdRepository.Setup(x => x.GetAll()).Returns(thresholds);

                var thresholdResponse = this.service.CanUserCredit(creditValue);

                Assert.IsFalse(thresholdResponse.CanUserCredit);

                this.userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
                this.creditThresholdRepository.Verify(x => x.GetAll(), Times.Once);
            }
        }

        public class TheAssignPendingCreditMethod : UserThresholdServiceTests
        {
            [Test]
            public void ShouldAssignLevel2ThresholdToUser()
            {
                var branchId = 22;
                var totalThresholdAmount = 100;
                var jobId = 33;

                var level2Threshold = new CreditThreshold { ThresholdLevelId = (int)ThresholdLevel.Level2, Threshold = 100 };

                this.creditThresholdRepository.Setup(x => x.GetByBranch(branchId)).Returns(new List<CreditThreshold> { level2Threshold });

                this.creditThresholdRepository.Setup(x => x.PendingCreditInsert(jobId));

                this.userRepository.Setup(x => x.UnAssignJobToUser(jobId));

                this.service.AssignPendingCredit(branchId, totalThresholdAmount, jobId);

                this.creditThresholdRepository.Verify(x => x.GetByBranch(branchId), Times.Once);

                this.creditThresholdRepository.Verify(x => x.PendingCreditInsert(jobId), Times.Once);
            }

            public void ShouldAssignLevel1ThresholdToUser()
            {
                var level1Threshold = new CreditThreshold { ThresholdLevelId = (int)ThresholdLevel.Level1 };
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
                    LineItems = new List<LineItem> { new LineItem{
                            NetPrice = 10M,
                            LineItemActions = new List<LineItemAction>
                            {
                                new LineItemAction {DeliveryAction = DeliveryAction.Credit, Quantity  = 10}
                            } },
                        new LineItem{
                            NetPrice = 20M,
                            LineItemActions = new List<LineItemAction>
                            {
                                new LineItemAction {DeliveryAction = DeliveryAction.Credit, Quantity  = 2},
                                new LineItemAction {DeliveryAction = DeliveryAction.Close, Quantity  = 2}
                            } }
                    }
                };
            }
        }

    }
}