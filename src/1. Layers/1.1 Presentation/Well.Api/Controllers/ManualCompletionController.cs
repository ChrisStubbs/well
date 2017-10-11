namespace PH.Well.Api.Controllers
{
    using System;
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
        private readonly IPatchSummaryMapper summaryMapper;

        public ManualCompletionController(
            ILogger logger,
            IManualCompletionService manualCompletionService,
            IPatchSummaryMapper summaryMapper)
        {
            this.logger = logger;
            this.manualCompletionService = manualCompletionService;
            this.summaryMapper = summaryMapper;
        }

        public IEnumerable<JobIdResolutionStatus> Patch(ManualCompletionRequest request)
        {
            if (!request.JobIds.Any())
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return manualCompletionService.Complete(request.JobIds, request.ManualCompletionType)
                .Select(x => new JobIdResolutionStatus(x.Id, x.ResolutionStatus));
        }

        [HttpGet]
        [Route("ManualCompletion/Summary")]
        public PatchSummary GetSummary([FromUri]int[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return summaryMapper.Map(manualCompletionService.GetJobsAvailableForCompletion(ids));
        }
    }
}