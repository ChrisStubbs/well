namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Domain;
    using Repositories.Read;

    public class LineItemSearchController : ApiController
    {
        private readonly LineItemSearchReadRepository lineItemSearchReadRepository;

        public LineItemSearchController(LineItemSearchReadRepository lineItemSearchReadRepository)
        {
            this.lineItemSearchReadRepository = lineItemSearchReadRepository;
        }

        public IEnumerable<LineItem> GetByIds([FromUri] IEnumerable<int> ids)
        {
            return this.lineItemSearchReadRepository.GetLineItemByIds(ids);
        }
    }
}