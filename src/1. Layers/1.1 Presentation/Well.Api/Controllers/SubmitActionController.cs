namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain.ValueObjects;
    using Models;
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


    }
}
