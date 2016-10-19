namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
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

                var canCredit = this.service.CanUserCredit(username, creditValue);

                Assert.IsTrue(canCredit);

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

                var canCredit = this.service.CanUserCredit(username, creditValue);

                Assert.IsFalse(canCredit);

                this.userRepository.Verify(x => x.GetByIdentity(username), Times.Once);
                this.creditThresholdRepository.Verify(x => x.GetAll(), Times.Once);
            }
        }

        public class TheAssignPendingCreditMethod : UserThresholdServiceTests
        {
            public void ShouldAssignLevel2ThresholdToUser()
            {
                var creditEvent = new CreditEvent { BranchId = 22, TotalCreditValueForThreshold = 100 };
                var user = new User();

                var level2Threshold = new CreditThreshold { ThresholdLevelId = (int)ThresholdLevel.Level2, Threshold = 100 };

                this.creditThresholdRepository.Setup(x => x.GetByBranch(creditEvent.BranchId)).Returns(new List<CreditThreshold> { level2Threshold });

                this.userRepository.Setup(x => x.GetUserByCreditThreshold(level2Threshold)).Returns(user);

                this.creditThresholdRepository.Setup(x => x.AssignPendingCreditToUser(user, creditEvent, "foo"));

                this.service.AssignPendingCredit(creditEvent, "");
            }

            public void ShouldAssignLevel1ThresholdToUser()
            {
                var level1Threshold = new CreditThreshold { ThresholdLevelId = (int)ThresholdLevel.Level1 };
            }
        }
    }
}