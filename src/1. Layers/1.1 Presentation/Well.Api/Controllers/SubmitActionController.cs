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
            var results = submitActionService.SubmitAction(action);
            return results;
        }

        [HttpGet]
        public ActionSubmitSummary PreSubmitSummary([FromUri] int[] jobId, DeliveryAction action, bool isStopLevel)
        {
            var results = submitActionService.GetSubmitSummary(new SubmitActionModel { JobIds = jobId, Action = action }, isStopLevel);
            return results;
        }

    }
}
