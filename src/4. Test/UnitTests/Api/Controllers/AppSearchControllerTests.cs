namespace PH.Well.UnitTests.Api.Controllers
{
    using Moq;
    using NUnit.Framework;
    using Well.Api.Controllers;
    using Well.Domain.ValueObjects;
    using Well.Services.Contracts;

    [TestFixture]
    public class AppSearchControllerTests : BaseControllerTests<AppSearchController>
    {
        private Mock<IAppSearchService> appSearchService;

        [SetUp]
        public void Setup()
        {
            appSearchService = new Mock<IAppSearchService>(MockBehavior.Strict);
            Controller = new AppSearchController(appSearchService.Object);
            SetupController();
        }

        public class TheGetMethod : AppSearchControllerTests
        {
            [Test]
            public void ShouldCallAppSearchServiceWithCorrectParametersAndReturnResult()
            {
                var searchParams = new AppSearchParameters();
                var searchResult = new AppSearchResult();

                appSearchService.Setup(x => x.GetAppSearchResult(searchParams)).Returns(searchResult);
                var result = Controller.Get(searchParams);

                Assert.AreEqual(result, searchResult);
                appSearchService.Verify(x => x.GetAppSearchResult(It.IsAny<AppSearchParameters>()), Times.Once());
                appSearchService.Verify(x => x.GetAppSearchResult(searchParams), Times.Once());
            }
        }
    }
}