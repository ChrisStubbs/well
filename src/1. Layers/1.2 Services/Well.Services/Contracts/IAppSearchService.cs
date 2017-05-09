namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface IAppSearchService
    {
        AppSearchResult GetAppSearchResult(AppSearchParameters searchParams);
    }
}