namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Repositories.Contracts;
    using Well.Domain;
    using Well.Services;

    [TestFixture]
    public class DateThresholdServiceTests
    {
        private Mock<ISeasonalDateRepository> seasonalDate;
        private Mock<IDateThresholdRepository> dateThresholdRepository;
        private Mock<ICustomerRoyaltyExceptionRepository> customerRoyaltyExceptionRepository;
        private Mock<IDbMultiConfiguration> connections;
        private DateThresholdService sut;

        [SetUp]
        public virtual void SetUp()
        {
            this.seasonalDate = new Mock<ISeasonalDateRepository>();
            this.dateThresholdRepository = new Mock<IDateThresholdRepository>();
            this.customerRoyaltyExceptionRepository = new Mock<ICustomerRoyaltyExceptionRepository>();
            this.connections = new Mock<IDbMultiConfiguration>();
            this.sut = new DateThresholdService(seasonalDate.Object, 
                dateThresholdRepository.Object, 
                customerRoyaltyExceptionRepository.Object,
                connections.Object);
        }

        public class BranchGracePeriodEndDateMethod : DateThresholdServiceTests
        {
            [SetUp]
            public override void SetUp()
            {
                base.SetUp();

                this.seasonalDate.Setup(p => p.GetByBranchId(1)).Returns(this.Branch_1_GetSeasonalDate());
                this.seasonalDate.Setup(p => p.GetByBranchId(3)).Returns(this.Branch_3_GetSeasonalDate());
                this.seasonalDate.Setup(p => p.GetByBranchId(4)).Returns(this.Branch_4_GetSeasonalDate());
                this.seasonalDate.Setup(p => p.GetByBranchId(2)).Returns(new List<SeasonalDate>());

                dateThresholdRepository.Setup(p => p.Get()).Returns(this.GetDateThreshold());
                this.sut = new DateThresholdService(seasonalDate.Object, 
                    dateThresholdRepository.Object, 
                    customerRoyaltyExceptionRepository.Object,
                    connections.Object);
            }

            private IList<DateThreshold> GetDateThreshold()
            {
                var result = new List<DateThreshold>();

                result.Add(new DateThreshold
                {
                    BranchId = 1,
                    NumberOfDays = 5
                });

                result.Add(new DateThreshold
                {
                    BranchId = 2,
                    NumberOfDays = 2
                });

                result.Add(new DateThreshold
                {
                    BranchId = 3,
                    NumberOfDays = 10
                });

                result.Add(new DateThreshold
                {
                    BranchId = 4,
                    NumberOfDays = 2
                });

                return result;
            }

            private IEnumerable<SeasonalDate> Branch_1_GetSeasonalDate()
            {
                var result = new List<SeasonalDate>(2);

                result.Add(new SeasonalDate
                {
                    Branches = new System.Collections.ObjectModel.Collection<Branch> { new Branch { Id = 1 } },
                    From = new DateTime(2000, 1, 1),
                    To = new DateTime(2000, 1, 2),
                    Id = 1
                });

                return result;
            }

            private IEnumerable<SeasonalDate> Branch_3_GetSeasonalDate()
            {
                var result = new List<SeasonalDate>(2);

                result.Add(new SeasonalDate
                {
                    Branches = new System.Collections.ObjectModel.Collection<Branch> { new Branch { Id = 3 } },
                    From = new DateTime(2000, 1, 1),
                    To = new DateTime(2000, 1, 1),
                    Id = 1
                });

                result.Add(new SeasonalDate
                {
                    Branches = new System.Collections.ObjectModel.Collection<Branch> { new Branch { Id = 3 } },
                    From = new DateTime(2000, 1, 3),
                    To = new DateTime(2000, 1, 3),
                    Id = 1
                });

                return result;
            }

            private IEnumerable<SeasonalDate> Branch_4_GetSeasonalDate()
            {
                var result = new List<SeasonalDate>(2);

                result.Add(new SeasonalDate
                {
                    Branches = new System.Collections.ObjectModel.Collection<Branch> { new Branch { Id = 3 } },
                    From = new DateTime(2000, 1, 2),
                    To = new DateTime(2000, 1, 2),
                    Id = 1
                });

                return result;
            }

            [Test]
            public void Should_Add_Days_With_Holidays()
            {
                var deliveryDate = new DateTime(1999, 12, 31);
                var expectedDate = deliveryDate.AddDays(7);

                Assert.That(sut.RouteGracePeriodEnd(deliveryDate, 1), Is.EqualTo(expectedDate));
            }

            [Test]
            public void Should_Add_Days_With_Several_Holidays()
            {
                var deliveryDate = new DateTime(1999, 12, 31);
                var expectedDate = deliveryDate.AddDays(12);

                Assert.That(sut.RouteGracePeriodEnd(deliveryDate, 3), Is.EqualTo(expectedDate));
            }

            [Test]
            public void Should_Add_Days_No_Holidays()
            {
                var deliveryDate = new DateTime(1999, 12, 31);
                var expectedDate = deliveryDate.AddDays(2);

                Assert.That(sut.RouteGracePeriodEnd(deliveryDate, 2), Is.EqualTo(expectedDate));
            }

            [Test]
            public void Should_Add_Days_Holidays_Last_Day_Of_Period()
            {
                var deliveryDate = new DateTime(1999, 12, 31);
                var expectedDate = deliveryDate.AddDays(3);

                Assert.That(sut.RouteGracePeriodEnd(deliveryDate, 4), Is.EqualTo(expectedDate));
            }

            [Test]
            public void Should_Throw_Error_No_Data_For_Branch()
            {
                Assert.Throws(typeof(Exception),
                    () => sut.RouteGracePeriodEnd(DateTime.Now, -1),
                    string.Format(DateThresholdService.ErrorMessage, -1));
            }
        }

        public class TheGracePeriodEndDateMethod : DateThresholdServiceTests
        {
            private int branchId = 1;
            private int royaltyCode = 3;

            private readonly DateThreshold branch1Threshold = new DateThreshold
            {
                BranchId = 1,
                NumberOfDays = 2
            };

            public override void SetUp()
            {
                base.SetUp();
                this.seasonalDate.Setup(x => x.GetByBranchId(branchId)).Returns(new List<SeasonalDate>());
                this.dateThresholdRepository.Setup(x => x.Get()).Returns(new List<DateThreshold> { branch1Threshold });

            }

            [Test]
            public void Should_Use_BranchDays_If_No_CustomerRoyaltyException()
            {
                this.customerRoyaltyExceptionRepository.Setup(x => x.GetCustomerRoyaltyExceptions()).Returns(new List<CustomerRoyaltyExceptionWell>());

                var routeDate = new DateTime(1999, 12, 31);
                var expectedDate = routeDate.AddDays(branch1Threshold.NumberOfDays);

                Assert.That(sut.GracePeriodEnd(routeDate, branchId, royaltyCode), Is.EqualTo(expectedDate));
            }

            [Test]
            public void Should_Use_CustomerRoyaltyException_If_GreaterThan_BranchNumberOfDays()
            {
                var royaltyException = new CustomerRoyaltyExceptionWell
                {
                    RoyaltyCode = royaltyCode,
                    ExceptionDays = (byte)(branch1Threshold.NumberOfDays + 1)
                };

                this.customerRoyaltyExceptionRepository.Setup(x => x.GetCustomerRoyaltyExceptions()).Returns(new[] { royaltyException });
                var routeDate = new DateTime(1999, 12, 31);
                var expectedDate = routeDate.AddDays(royaltyException.ExceptionDays);

                Assert.That(sut.GracePeriodEnd(routeDate, branchId, royaltyCode), Is.EqualTo(expectedDate));
            }

            [Test]
            public void Should_Use_BranchNumberOfDay_If_GreaterThan_RoyaltyException()
            {
                var royaltyException = new CustomerRoyaltyExceptionWell
                {
                    RoyaltyCode = royaltyCode,
                    ExceptionDays = (byte)(branch1Threshold.NumberOfDays - 1)
                };

                this.customerRoyaltyExceptionRepository.Setup(x => x.GetCustomerRoyaltyExceptions()).Returns(new[] { royaltyException });
                var routeDate = new DateTime(1999, 12, 31);
                var expectedDate = routeDate.AddDays(branch1Threshold.NumberOfDays);

                Assert.That(sut.GracePeriodEnd(routeDate, branchId, royaltyCode), Is.EqualTo(expectedDate));
            }
        }
    }
}
