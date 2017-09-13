namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface IAppSearchService
    {
        AppSearchResultSummary GetAppSearchResult(AppSearchParameters searchParams);
    }
}