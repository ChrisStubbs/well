namespace PH.Well.UnitTests.Services
{
    using System.Collections.Generic;
    using Moq;
    using NUnit.Framework;
    using Repositories.Read;
    using Well.Domain.ValueObjects;
    using Well.Services;

    [TestFixture]
    public class AppSearchServiceTests
    {
        private Mock<IAppSearchReadRepository> searchReadRepository;
        private AppSearchService service;
        [SetUp]
        public void SetUp()
        {
            searchReadRepository = new Mock<IAppSearchReadRepository>(MockBehavior.Strict);
            service = new AppSearchService(searchReadRepository.Object);
        }

        public class TheGetAppSearchResultMethod : AppSearchServiceTests
        {
            [Test]
            public void ShouldCallSearchRepositorySearchMethodWithCorrectParams()
            {
                var searchParams = new AppSearchParameters();
                var searchResults = new List<AppSearchItem>();

                searchReadRepository.Setup(x => x.Search(searchParams)).Returns(searchResults);

                var result = service.GetAppSearchResult(searchParams);
                searchReadRepository.Verify(x => x.Search(It.IsAny<AppSearchParameters>()), Times.Once);
                searchReadRepository.Verify(x => x.Search(searchParams), Times.Once);
            }
        }
    }
}