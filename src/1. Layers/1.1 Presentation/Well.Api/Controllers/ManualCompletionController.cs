namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Common.Contracts;
    using Services.Contracts;

    public class ManualCompletionController : ApiController
    {
        private readonly ILogger logger;
        private readonly IManualCompletionService manualCompletionService;

        public ManualCompletionController(
            ILogger logger,
            IManualCompletionService manualCompletionService)
        {
            this.logger = logger;
            this.manualCompletionService = manualCompletionService;
        }

    }
}