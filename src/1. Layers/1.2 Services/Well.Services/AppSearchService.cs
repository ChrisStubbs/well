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

        public AppSearchResultSummary GetAppSearchResult(AppSearchParameters searchParams)
        {
            var results = searchReadRepository.Search(searchParams).ToArray();
            return AppSearchResultSummary.Get(results, searchParams.BranchId);
        }
    }
}