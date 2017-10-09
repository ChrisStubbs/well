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
        private readonly IUserNameProvider userNameProvider;
        private readonly ILineItemActionService lineItemActionService;
        private readonly IJobService jobService;

        private static readonly JobType[] BulkEditableJobTypes =
        {
            JobType.Alcohol,
            JobType.Ambient,
            JobType.Chilled,
            JobType.Frozen,
            JobType.Tobacco
        };

        public BulkEditService(
            ILogger logger,
            IJobService jobService,
            IJobRepository jobRepository,
            IPatchSummaryMapper mapper,
            ILineItemActionRepository lineItemActionRepository,
            IUserNameProvider userNameProvider,
            ILineItemActionService lineItemActionService)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.mapper = mapper;
            this.lineItemActionRepository = lineItemActionRepository;
            this.userNameProvider = userNameProvider;
            this.lineItemActionService = lineItemActionService;
            this.jobService = jobService;
        }

        public PatchSummary GetByLineItems(IEnumerable<int> lineItemIds, DeliveryAction deliveryAction)
        {
            lineItemIds = lineItemIds.ToArray();
            var jobs = GetEditableJobsByLineItemId(lineItemIds)
                .Where(j => lineItemActionService.CanSetActionForJob(j, deliveryAction));
            return mapper.Map(jobs, lineItemIds);
        }

        public PatchSummary GetByJobs(IEnumerable<int> jobIds, DeliveryAction deliveryAction)
        {
            var jobs = GetEditableJobsByJobId(jobIds)
                .Where(j => lineItemActionService.CanSetActionForJob(j, deliveryAction));
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
                    foreach (var job in editableJobs)
                    {
                        if (!lineItemActionService.CanSetActionForJob(job, resolution.DeliveryAction))
                        {
                            throw new InvalidOperationException(
                                $"Can not set delivery action : {resolution.DeliveryAction} for job line item exceptions");
                        }

                        foreach (var lineItemAction in LineItemActionsToEdit(job,lineItemIds))
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

                        job.ResolutionStatus = jobService.GetCurrentResolutionStatus(job);
                        jobRepository.Update(job);
                        jobRepository.SaveJobResolutionStatus(job);
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
            var username = this.userNameProvider.GetUserName();
            var editableJobs = jobService.PopulateLineItemsAndRoute(jobs).ToList()
                .Where(x =>
                    x.ResolutionStatus.IsEditable()
                    && BulkEditableJobTypes.Contains(x.JobType)
                    && LineItemActionsToEdit(x, lineItemIds).Any()).ToArray();

            return editableJobs.Where(x => string.IsNullOrEmpty(jobService.CanEdit(x, username)));
        }

        public virtual IEnumerable<LineItemAction> LineItemActionsToUpdate(IEnumerable<Job> jobsToEdit, IEnumerable<int> lineItemIds)
        {
            return jobsToEdit.SelectMany(x => LineItemActionsToEdit(x, lineItemIds));
        }

        public virtual IEnumerable<LineItemAction> LineItemActionsToEdit(Job job, IEnumerable<int> lineItemIds)
        {
            var lineIds = lineItemIds?.ToArray() ?? new int[] { };

            return job.GetAllLineItemActions().Where(lia => lia.Quantity > 0 && (!lineIds.Any() || lineIds.Contains(lia.LineItemId)));

        }

    }
}