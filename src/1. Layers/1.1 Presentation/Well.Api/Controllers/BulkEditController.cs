namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Domain;
    using Domain.ValueObjects;
    using Models;
    using Services.Contracts;

    public class BulkEditController : ApiController
    {
        private readonly IBulkEditService bulkEditService;

        public BulkEditController(IBulkEditService bulkEditService)
        {
            this.bulkEditService = bulkEditService;
        }

        public BulkEditResult Patch(BulkEditPatchRequest request)
        {

            if (!request.JobIds.Any() && !request.LineItemIds.Any())
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            var jobs = request.JobIds.Any() ?
                bulkEditService.GetEditableJobsByJobId(request.JobIds).ToArray() :
                bulkEditService.GetEditableJobsByLineItemId(request.LineItemIds).ToArray();

            if (!jobs.Any())
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return bulkEditService.Update(jobs, request, request.LineItemIds);
        }

        [HttpGet]
        [Route("bulkedit/Summary/Jobs/")]
        public PatchSummary GetSummaryForJob([FromUri]int[] id)
        {
            if (id == null || id.Length == 0)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            return bulkEditService.GetByJobs(id);
        }

        [HttpGet]
        [Route("bulkedit/Summary/LineItems/")]
        public PatchSummary GetSummaryForLineItems([FromUri]int[] id)
        {
            if (id == null || id.Length == 0)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }

            return bulkEditService.GetByLineItems(id);
        }
    }
}