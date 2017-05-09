namespace PH.Well.Services
{
    using System.Linq;
    using Contracts;
    using Domain.ValueObjects;
    using Repositories.Read;

    public class AppSearchService : IAppSearchService
    {
        private readonly IAppSearchReadRepository searchReadRepository;

        public AppSearchService(IAppSearchReadRepository searchReadRepository)
        {
            this.searchReadRepository = searchReadRepository;
        }


        public AppSearchResult GetAppSearchResult(AppSearchParameters searchParams)
        {

            var results = searchReadRepository.Search(searchParams).ToArray();

            var stops = results.Where(x=> x.StopId.HasValue).Select(x => x.StopId).Distinct().ToArray();
            if (stops.Length == 1)
            {
                return new AppSearchResult { StopId = stops.First() };
            }

            var routes = results.Where(x => x.RouteId.HasValue).Select(x => x.RouteId).Distinct().ToArray();
            if (routes.Length == 1)
            {
                return new AppSearchResult { RouteId = routes.First() };
            }

            return new AppSearchResult();
        }
    }
}