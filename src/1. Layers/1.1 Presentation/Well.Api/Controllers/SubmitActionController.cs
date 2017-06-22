namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Services.Contracts;

    public class SubmitActionController : ApiController
    {
        private readonly ISubmitActionService submitActionService;

        public SubmitActionController(ISubmitActionService submitActionService)
        {
            this.submitActionService = submitActionService;
        }

        public SubmitActionResult Post(SubmitActionModel action)
        {
            var result =  submitActionService.SubmitAction(action);
            return result;
        }

        [HttpGet]
        public ActionSubmitSummary PreSubmitSummary([FromUri] int[] jobId, bool isStopLevel)
        {
            var results = submitActionService.GetSubmitSummary(new SubmitActionModel { JobIds = jobId}, isStopLevel);
            return results;
        }

    }
}
