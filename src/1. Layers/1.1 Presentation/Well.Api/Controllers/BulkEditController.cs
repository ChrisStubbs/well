namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain.ValueObjects;
    using Services.Contracts;

    public class BulkEditController : ApiController
    {
        private readonly IBulkEditService bulkEditService;

        public BulkEditController(IBulkEditService bulkEditService)
        {
            this.bulkEditService = bulkEditService;
        }

        [HttpGet]
        [Route("bulkedit/Summary/Jobs/")]
        public BulkEditSummary GetSummaryForJob([FromUri]int[] id)
        {
            if (id == null || id.Length == 0)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            return bulkEditService.GetByJobs(id);
        }

        [HttpGet]
        [Route("bulkedit/Summary/LineItems/")]
        public BulkEditSummary GetSummaryForLineItems([FromUri]int[] id)
        {
            if (id == null || id.Length == 0)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            return bulkEditService.GetByLineItems(id);
        }
    }
}