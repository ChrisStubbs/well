namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class SeasonalDateControllerTests : BaseControllerTests<SeasonalDateController>
    {
        private Mock<ISeasonalDateRepository> seasonalDateRepository;

        [SetUp]
        public void Setup()
        {
            this.seasonalDateRepository = new Mock<ISeasonalDateRepository>(MockBehavior.Strict);

            this.Controller = new SeasonalDateController(this.seasonalDateRepository.Object);

            this.SetupController();
        }

        public class TheGetMethod : SeasonalDateControllerTests
        {
            [Test]
            public void ShouldReturnAllSeasonalDates()
            {
                var seasonal1 = SeasonalDateFactory.New.With(x => x.BranchId = 1).With(x => x.From = DateTime.Now.AddDays(-1)).Build();
                var seasonal2 = SeasonalDateFactory.New.With(x => x.BranchId = 2).With(x => x.From = DateTime.Now).Build();

                var seasonalDates = new List<SeasonalDate> { seasonal1, seasonal2 };

                this.seasonalDateRepository.Setup(x => x.GetAll()).Returns(seasonalDates);

                var result = this.Controller.Get();

                Assert.That(
                    result.StatusCode,
                    Is.EqualTo(HttpStatusCode.OK));

                var contentResult = new List<SeasonalDate>();

                result.TryGetContentValue(out contentResult);

                Assert.That(contentResult.Count, Is.EqualTo(2));
                Assert.That(contentResult[0].BranchId, Is.EqualTo(1));
                Assert.That(contentResult[1].BranchId, Is.EqualTo(2));
            }
        }
    }
}