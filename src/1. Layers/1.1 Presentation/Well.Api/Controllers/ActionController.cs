namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain.ValueObjects;
    using Models;
    using Services.Contracts;

    public class ActionController : ApiController
    {
        private readonly IBulkActionService bulkActionService;

        public ActionController(IBulkActionService bulkActionService)
        {
            this.bulkActionService = bulkActionService;
        }

        public BulkActionResults Post(BulkActionModel action)
        {
            //TODO: The BulkActionService needs completeing!
            var results = bulkActionService.ApplyAction(action);
            return results;
        }


    }
}
