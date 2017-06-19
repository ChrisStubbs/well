namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain.ValueObjects;
    using Services.Contracts;

    public class AppSearchController : ApiController
    {
        private readonly IAppSearchService appSearchService;

        public AppSearchController(IAppSearchService appSearchService)
        {
            this.appSearchService = appSearchService;
        }

        public AppSearchResultSummary Get([FromUri]AppSearchParameters parameters)
        {
            return appSearchService.GetAppSearchResult(parameters);
        }
    }
}
