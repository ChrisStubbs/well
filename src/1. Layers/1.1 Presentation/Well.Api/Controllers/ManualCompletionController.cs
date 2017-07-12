namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Common.Contracts;
    using Domain.ValueObjects;
    using Models;
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

        public IList<JobIdResolutionStatus> Patch(ManualCompletionRequest request)
        {
            if (!request.JobIds.Any())
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return manualCompletionService.Complete(request.JobIds, request.ManualCompletionType)
                        .Select(x => new JobIdResolutionStatus(x.Id, x.ResolutionStatus)).ToList();
        }
    }
}