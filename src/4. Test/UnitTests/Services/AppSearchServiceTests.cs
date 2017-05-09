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
                var searchResults = new List<AppSearchResult>();

                searchReadRepository.Setup(x => x.Search(searchParams)).Returns(searchResults);

                var result = service.GetAppSearchResult(searchParams);
                searchReadRepository.Verify(x => x.Search(It.IsAny<AppSearchParameters>()), Times.Once);
                searchReadRepository.Verify(x=> x.Search(searchParams),Times.Once);
            }

            [Test]
            public void ShouldReturnAppSearchResultWithNullStopAndRouteIdIfMultipleRoutes()
            {
                var searchParams = new AppSearchParameters();
                var searchResults = new List<AppSearchResult>
                {
                    new AppSearchResult {RouteId = 4},
                    new AppSearchResult {RouteId = 5}
                };

                searchReadRepository.Setup(x => x.Search(searchParams)).Returns(searchResults);

                var result = service.GetAppSearchResult(searchParams);

                Assert.False(result.StopId.HasValue);
                Assert.False(result.RouteId.HasValue);
            }

            [Test]
            public void ShouldReturnAppSearchResultWithNullStopAndValidRouteIdIfSingleDistinctRouteId()
            {var searchParams = new AppSearchParameters();
                var searchResults = new List<AppSearchResult>
                {
                    new AppSearchResult {RouteId = 4,StopId = 3},
                    new AppSearchResult {RouteId = 4,StopId = 4}
                };

                searchReadRepository.Setup(x => x.Search(searchParams)).Returns(searchResults);

                var result = service.GetAppSearchResult(searchParams);

                Assert.False(result.StopId.HasValue);
                Assert.AreEqual(4,result.RouteId);
            }

            [Test]
            public void ShouldReturnAppSearchResultWithStopIdAndNullRouteIdIfSingleDistinctJobId()
            {
                var searchParams = new AppSearchParameters();
                var searchResults = new List<AppSearchResult>
                {
                    new AppSearchResult {RouteId = 4,StopId = 3}
                    
                };

                searchReadRepository.Setup(x => x.Search(searchParams)).Returns(searchResults);

                var result = service.GetAppSearchResult(searchParams);

                Assert.False(result.RouteId.HasValue);
                Assert.AreEqual(3, result.StopId);
            }
        }
    }
}