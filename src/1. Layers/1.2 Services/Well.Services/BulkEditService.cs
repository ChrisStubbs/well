namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class BulkEditService : IBulkEditService
    {
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IBulkEditSummaryMapper mapper;
        private readonly IJobService jobService;



        public BulkEditService(
            ILogger logger,
            IJobService jobService,
            IJobRepository jobRepository,
            IBulkEditSummaryMapper mapper)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.mapper = mapper;
            this.jobService = jobService;
        }
        public BulkEditSummary GetByLineItems(IEnumerable<int> lineItemIds)
        {
            lineItemIds = lineItemIds.ToArray();
            var jobs = GetEditableJobs(jobRepository.GetJobsByLineItemIds(lineItemIds));
            return mapper.Map(jobs, lineItemIds);
        }

        public BulkEditSummary GetByJobs(IEnumerable<int> jobIds)
        {
            var jobs = GetEditableJobs(jobRepository.GetByIds(jobIds));
            return mapper.Map(jobs);
        }

        public IEnumerable<Job> GetEditableJobs(IEnumerable<Job> job)
        {
            return jobService.PopulateLineItemsAndRoute(job).ToList()
                .Where(x => x.ResolutionStatus.IsEditable() && x.GetAllLineItemActions().Any(a => a.Quantity > 0));
        }

    }
}