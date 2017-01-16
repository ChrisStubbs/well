namespace PH.Well.UnitTests.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;

    using Moq;

    using NUnit.Framework;

    using PH.Well.Api.Controllers;
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.UnitTests.Factories;

    [TestFixture]
    public class SeasonalDateControllerTests : BaseControllerTests<SeasonalDateController>
    {
        private Mock<ILogger> logger;
        private Mock<ISeasonalDateRepository> seasonalDateRepository;
        private Mock<ISeasonalDateMapper> mapper;
        private Mock<ISeasonalDateValidator> validator;

        [SetUp]
        public void Setup()
        {
            this.seasonalDateRepository = new Mock<ISeasonalDateRepository>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.mapper = new Mock<ISeasonalDateMapper>(MockBehavior.Strict);
            this.validator = new Mock<ISeasonalDateValidator>(MockBehavior.Strict);
            this.seasonalDateRepository.SetupSet(x => x.CurrentUser = It.IsAny<string>());
            this.Controller = new SeasonalDateController(
                this.seasonalDateRepository.Object, 
                this.logger.Object, 
                this.mapper.Object,
                this.validator.Object);

            this.SetupController();
        }

        public class TheGetMethod : SeasonalDateControllerTests
        {
            [Test]
            public void ShouldReturnAllSeasonalDates()
            {
                var seasonal1 = SeasonalDateFactory.New.With(x => x.From = DateTime.Now.AddDays(-1)).Build();
                var seasonal2 = SeasonalDateFactory.New.With(x => x.From = DateTime.Now).Build();

                var seasonalDates = new List<SeasonalDate> { seasonal1, seasonal2 };

                this.seasonalDateRepository.Setup(x => x.GetAll()).Returns(seasonalDates);

                this.mapper.Setup(x => x.Map(seasonal1)).Returns(new SeasonalDateModel());
                this.mapper.Setup(x => x.Map(seasonal2)).Returns(new SeasonalDateModel());

                var result = this.Controller.Get();

                Assert.That(
                    result.StatusCode,
                    Is.EqualTo(HttpStatusCode.OK));

                var contentResult = new List<SeasonalDateModel>();

                result.TryGetContentValue(out contentResult);

                Assert.That(contentResult.Count, Is.EqualTo(2));
            }
        }

        public class TheDeleteMethod : SeasonalDateControllerTests
        {
            [Test]
            public void ShouldDeleteTheSeasonalDate()
            {
                var id = 32;

                this.seasonalDateRepository.Setup(x => x.Delete(id));

                var response = this.Controller.Delete(id);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                this.seasonalDateRepository.Verify(x => x.Delete(id), Times.Once);
            }

            [Test]
            public void ShouldLogErrorWhenThrown()
            {
                var exception = new Exception();

                var id = 32;

                this.seasonalDateRepository.Setup(x => x.Delete(id)).Throws(exception);

                this.logger.Setup(x => x.LogError("Error when trying to delete seasonal date (id):32", exception));

                var response = this.Controller.Delete(id);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                this.logger.Verify(x => x.LogError("Error when trying to delete seasonal date (id):32", exception), Times.Once);
            }
        }
    }
}