namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;

    [TestFixture]
    public class UserThresholdServiceTests
    {
        private Mock<ICreditThresholdRepository> creditThresholdRepository;
        private Mock<IUserRepository> userRepository;

        private UserThresholdService service;

        [SetUp]
        public void Setup()
        {
            this.creditThresholdRepository = new Mock<ICreditThresholdRepository>(MockBehavior.Strict);
            this.userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            this.service = new UserThresholdService(this.creditThresholdRepository.Object, this.userRepository.Object);
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

                var username = "foo";
                var creditValue = 100;

                this.userRepository.Setup(x => x.GetByIdentity(username)).Returns(user);
                this.creditThresholdRepository.Setup(x => x.GetAll()).Returns(thresholds);

                var thresholdResponse = this.service.CanUserCredit(username, creditValue);

                Assert.IsTrue(thresholdResponse.CanUserCredit);

                this.userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
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

                var thresholdResponse = this.service.CanUserCredit(username, creditValue);

                Assert.IsFalse(thresholdResponse.CanUserCredit);

                this.userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
                this.creditThresholdRepository.Verify(x => x.GetAll(), Times.Once);
            }
        }

        public class TheAssignPendingCreditMethod : UserThresholdServiceTests
        {
            public void ShouldAssignLevel2ThresholdToUser()
            {
                var branchId = 22;
                var totalThresholdAmount = 100;
                var user = new User();

                var level2Threshold = new CreditThreshold { ThresholdLevelId = (int)ThresholdLevel.Level2, Threshold = 100 };

                this.creditThresholdRepository.Setup(x => x.GetByBranch(branchId)).Returns(new List<CreditThreshold> { level2Threshold });

                this.userRepository.Setup(x => x.GetUserByCreditThreshold(level2Threshold)).Returns(user);

                this.creditThresholdRepository.Setup(x => x.PendingCreditInsert(1, "foo"));

                this.service.AssignPendingCredit(branchId, totalThresholdAmount, 1, "");
            }

            public void ShouldAssignLevel1ThresholdToUser()
            {
                var level1Threshold = new CreditThreshold { ThresholdLevelId = (int)ThresholdLevel.Level1 };
            }
        }

        public class TheRemoveCreditEventsThatDontHaveAThresholdMethod : UserThresholdServiceTests
        {
            [Test]
            public void ShouldRemoveCreditEventsThatDontHaveThresholdSetupCorrectly()
            {
                // TODO
                /*var creditEvents = new List<CreditEvent>();

                var creditEvent = CreditEventFactory.New.Build();

                creditEvents.Add(creditEvent);

                var username = "jonny the foo";

                var user = new User { ThresholdLevelId = 1 };

                this.userRepository.Setup(x => x.GetByIdentity(username)).Returns(user);

                var thresholds = new List<CreditThreshold> { new CreditThreshold { ThresholdLevelId = 1, Threshold = 2M } };
                
                this.creditThresholdRepository.Setup(x => x.GetAll()).Returns(thresholds);

                this.creditThresholdRepository.Setup(x => x.GetByBranch(creditEvent.BranchId))

                this.service.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, username);*/
            }
        }
    }
}