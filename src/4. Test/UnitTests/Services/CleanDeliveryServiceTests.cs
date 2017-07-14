namespace PH.Well.UnitTests.Services
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using Well.Services.Contracts;

    [TestFixture]
    public class CleanDeliveryServiceTests
    {
        private Mock<ILogger> logger;

        private Mock<IRouteHeaderRepository> routeHeaderRepository;

        private Mock<IStopRepository> stopRepository;

        private Mock<IJobRepository> jobRepository;

        private Mock<IJobDetailRepository> jobDetailRepository;

        private Mock<IRouteToRemoveRepository> routeToRemoveRepository;

        private Mock<ICleanPreferenceRepository> cleanPreferenceRepository;

        private Mock<ISeasonalDateRepository> seasonalDateRepository;

        private Mock<IAmendmentService> amendmentService;

        private CleanDeliveryService service;

        [SetUp]
        public void Setup()
        {
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.routeHeaderRepository = new Mock<IRouteHeaderRepository>(MockBehavior.Strict);
            this.stopRepository = new Mock<IStopRepository>(MockBehavior.Strict);
            this.jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
            this.jobDetailRepository = new Mock<IJobDetailRepository>(MockBehavior.Strict);
            this.routeToRemoveRepository = new Mock<IRouteToRemoveRepository>(MockBehavior.Strict);
            this.cleanPreferenceRepository = new Mock<ICleanPreferenceRepository>(MockBehavior.Strict);
            this.seasonalDateRepository = new Mock<ISeasonalDateRepository>(MockBehavior.Strict);
            this.amendmentService = new Mock<IAmendmentService>(MockBehavior.Strict);


            this.service = new CleanDeliveryService(this.logger.Object,
                this.routeHeaderRepository.Object,
                this.stopRepository.Object,
                this.jobRepository.Object,
                this.jobDetailRepository.Object,
                this.routeToRemoveRepository.Object,
                this.cleanPreferenceRepository.Object,
                this.seasonalDateRepository.Object,
                this.amendmentService.Object);
        }

        public class TheCanDeleteMethod : CleanDeliveryServiceTests
        {
            [Test]
            public void ShouldReturnFalseWhenSeasonalDateIsActive()
            {
                var dateCreated = DateTime.Now;

                var royaltyException = new CustomerRoyaltyException();
                var cleanPreference = new CleanPreference();

                var season = new SeasonalDate { From = DateTime.Now, To = DateTime.Now };

                var seasonalDates = new List<SeasonalDate> { season };
                
                Assert.IsFalse(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnWhenMultipleSeasonalDateIsActive()
            {
                var dateCreated = DateTime.Now;

                var royaltyException = new CustomerRoyaltyException();
                var cleanPreference = new CleanPreference();

                var season = new SeasonalDate { From = DateTime.Now.AddDays(-2), To = DateTime.Now.AddDays(2) };

                var seasonalDates = new List<SeasonalDate> { season };

                Assert.IsFalse(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnDefaultOfFalseWhenSeasonalDateIsNotActiveInPast()
            {
                var dateCreated = DateTime.Now;

                var royaltyException = new CustomerRoyaltyException();
                var cleanPreference = new CleanPreference();

                var season = new SeasonalDate { From = DateTime.Now.AddDays(-10), To = DateTime.Now.AddDays(-5) };

                var seasonalDates = new List<SeasonalDate> { season };

                Assert.IsFalse(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnFalseWhenSeasonalDateIsNotActiveInFuture()
            {
                var dateCreated = DateTime.Now;

                var royaltyException = new CustomerRoyaltyException();
                var cleanPreference = new CleanPreference();

                var season = new SeasonalDate { From = DateTime.Now.AddDays(10), To = DateTime.Now.AddDays(15) };

                var seasonalDates = new List<SeasonalDate> { season };

                Assert.IsFalse(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnFalseWhenRoyaltyExceptionGreaterThanNow()
            {
                var dateCreated = DateTime.Now;

                var royaltyException = new CustomerRoyaltyException { ExceptionDays = 3 };
                var cleanPreference = new CleanPreference();
                var seasonalDates = new List<SeasonalDate>();

                Assert.IsFalse(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnTrueWhenRoyaltyExceptionLessThanNow()
            {
                var dateCreated = DateTime.Now.AddDays(-4);

                var royaltyException = new CustomerRoyaltyException { ExceptionDays = 3 };
                var cleanPreference = new CleanPreference();
                var seasonalDates = new List<SeasonalDate>();

                Assert.IsTrue(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnFalseWhenCleanPreferenceIsNotActive()
            {
                var dateCreated = DateTime.Now;

                var royaltyException = new CustomerRoyaltyException();
                var cleanPreference = new CleanPreference { Days = 1 };
                var seasonalDates = new List<SeasonalDate>();

                Assert.IsFalse(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnTrueWhenCleanPreferenceIsActive()
            {
                var dateCreated = DateTime.Now.AddDays(-5);

                var royaltyException = new CustomerRoyaltyException();
                var cleanPreference = new CleanPreference { Days = 1 };
                var seasonalDates = new List<SeasonalDate>();

                Assert.IsTrue(this.service.CanDelete(royaltyException, cleanPreference, seasonalDates, dateCreated));
            }

            [Test]
            public void ShouldReturnFalseWhenDefaultCleanPreferenceIsActive()
            {
                var dateCreated = DateTime.Now;

                var royaltyException = new CustomerRoyaltyException();
                var seasonalDates = new List<SeasonalDate>();

                Assert.IsFalse(this.service.CanDelete(royaltyException, null, seasonalDates, dateCreated));
            }
        }
    }
}