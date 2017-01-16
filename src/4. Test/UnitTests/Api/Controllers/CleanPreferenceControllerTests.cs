namespace PH.Well.UnitTests.Api.Controllers
{
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
    public class CleanPreferenceControllerTests : BaseControllerTests<CleanPreferenceController>
    {
        private Mock<ICleanPreferenceRepository> cleanPreferenceRepository;

        private Mock<ICleanPreferenceMapper> cleanPreferenceMapper;

        private Mock<ILogger> logger;

        private Mock<ICleanPreferenceValidator> cleanPreferenceValidator;

        [SetUp]
        public void Setup()
        {
            this.cleanPreferenceRepository = new Mock<ICleanPreferenceRepository>(MockBehavior.Strict);
            this.cleanPreferenceMapper = new Mock<ICleanPreferenceMapper>(MockBehavior.Strict);
            this.logger = new Mock<ILogger>(MockBehavior.Strict);
            this.cleanPreferenceValidator = new Mock<ICleanPreferenceValidator>(MockBehavior.Strict);

            this.cleanPreferenceRepository.SetupSet(x => x.CurrentUser = It.IsAny<string>());

            this.Controller = new CleanPreferenceController(
                this.cleanPreferenceRepository.Object, 
                this.cleanPreferenceMapper.Object, 
                this.logger.Object,
                this.cleanPreferenceValidator.Object);

            this.SetupController();
        }

        public class TheGetMethod : CleanPreferenceControllerTests
        {
            [Test]
            public void ShouldReturnAllCleanPreferences()
            {
                var cleanPreferences = new List<CleanPreference> { CleanPreferenceFactory.New.Build() };

                this.cleanPreferenceRepository.Setup(x => x.GetAll()).Returns(cleanPreferences);

                this.cleanPreferenceMapper.Setup(x => x.Map(cleanPreferences[0])).Returns(new CleanPreferenceModel());

                var response = this.Controller.Get();

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var contentResult = new List<CleanPreferenceModel>();

                response.TryGetContentValue(out contentResult);

                Assert.That(contentResult.Count, Is.EqualTo(1));
            }
        }
    }
}