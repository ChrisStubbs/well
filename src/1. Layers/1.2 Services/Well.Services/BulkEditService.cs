namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Contracts;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class BulkEditService : IBulkEditService
    {
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IPatchSummaryMapper mapper;
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly IJobService jobService;

        public BulkEditService(
            ILogger logger,
            IJobService jobService,
            IJobRepository jobRepository,
            IPatchSummaryMapper mapper,
            ILineItemActionRepository lineItemActionRepository)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.mapper = mapper;
            this.lineItemActionRepository = lineItemActionRepository;
            this.jobService = jobService;
        }
        public PatchSummary GetByLineItems(IEnumerable<int> lineItemIds)
        {
            lineItemIds = lineItemIds.ToArray();
            var jobs = GetEditableJobsByLineItemId(lineItemIds);
            return mapper.Map(jobs, lineItemIds);
        }

        public PatchSummary GetByJobs(IEnumerable<int> jobIds)
        {
            var jobs = GetEditableJobsByJobId(jobIds);
            return mapper.Map(jobs);
        }

        public IEnumerable<Job> GetEditableJobsByLineItemId(IEnumerable<int> lineItemIds)
        {
            return GetEditableJobs(jobRepository.GetJobsByLineItemIds(lineItemIds), lineItemIds);
        }

        public IEnumerable<Job> GetEditableJobsByJobId(IEnumerable<int> jobIds)
        {
            return GetEditableJobs(jobRepository.GetByIds(jobIds));
        }

        public BulkEditResult Update(IEnumerable<Job> editableJobs, ILineItemActionResolution resolution, IEnumerable<int> lineItemIds)
        {
            editableJobs = editableJobs.ToArray();
            var result = new BulkEditResult();
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    foreach (var lineItemAction in LineItemActionsToUpdate(editableJobs, lineItemIds))
                    {
                        if (resolution.DeliveryAction == DeliveryAction.Close)
                        {
                            lineItemAction.Quantity = 0;
                        }

                        lineItemAction.DeliveryAction = resolution.DeliveryAction;
                        lineItemAction.Source = resolution.Source;
                        lineItemAction.Reason = resolution.Reason;
                        lineItemActionRepository.Update(lineItemAction);
                        result.LineItemIds.Add(lineItemAction.LineItemId);
                    }

                    foreach (var job in editableJobs)
                    {
                        job.ResolutionStatus = jobService.GetCurrentResolutionStatus(job);
                        jobRepository.Update(job);
                        result.Statuses.Add(new JobIdResolutionStatus(job.Id, job.ResolutionStatus));
                    }

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Bulk editing", ex);
                throw;
            }
            return result;
        }

        public IEnumerable<Job> GetEditableJobs(IEnumerable<Job> jobs, IEnumerable<int> lineItemIds = null)
        {
            var editableJobs = jobService.PopulateLineItemsAndRoute(jobs).ToList()
                .Where(x => x.ResolutionStatus.IsEditable() &&
                            LineItemActionsToEdit(x, lineItemIds).Any()).ToArray();

            var userJobsIds = jobService.GetJobsIdsAssignedToCurrentUser(editableJobs.Select(x => x.Id));
                
            return editableJobs.Where(x => userJobsIds.Contains(x.Id));
        }

        public virtual IEnumerable<LineItemAction> LineItemActionsToUpdate(IEnumerable<Job> jobsToEdit, IEnumerable<int> lineItemIds)
        {
            return jobsToEdit.SelectMany(x => LineItemActionsToEdit(x, lineItemIds));
        }

        private IEnumerable<LineItemAction> LineItemActionsToEdit(Job job, IEnumerable<int> lineItemIds)
        {
            var lineIds = lineItemIds?.ToArray() ?? new int[] { };

            return job.GetAllLineItemActions().Where(lia => lia.Quantity > 0 && (!lineIds.Any() || lineIds.Contains(lia.LineItemId)));

        }

    }
}