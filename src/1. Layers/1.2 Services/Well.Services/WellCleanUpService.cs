namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain.ValueObjects;
    using Repositories.Contracts;
    using System.Collections.Concurrent;

    public class WellCleanUpService : IWellCleanUpService
    {
        private readonly ILogger logger;
        private readonly IWellCleanUpRepository wellCleanUpRepository;
        private readonly IDateThresholdService dateThresholdService;
        private readonly IAmendmentService amendmentService;
        private readonly IJobRepository jobRepository;
        private readonly IWellCleanConfig configuration;

        public WellCleanUpService(
            ILogger logger,
            IWellCleanUpRepository wellCleanUpRepository,
            IDateThresholdService dateThresholdService,
            IAmendmentService amendmentService,
            IJobRepository jobRepository,
            IWellCleanConfig configuration)
        {
            this.logger = logger;
            this.wellCleanUpRepository = wellCleanUpRepository;
            this.dateThresholdService = dateThresholdService;
            this.amendmentService = amendmentService;
            this.jobRepository = jobRepository;
            this.configuration = configuration;
        }

        public async Task SoftDelete()
        {
            logger.LogDebug("Start soft delete");

            var routesData = this.GetJobsFromRoutes();

            try
            {
                var data = await this.FilterLookup(routesData);
                var jobsToDelete = data
                    .SelectMany(p => p)
                    .Select(p => p.JobId)
                    .ToList();

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    logger.LogDebug("Start generating amendments delete");
                    await amendmentService.ProcessAmendmentsAsync(jobsToDelete);
                    logger.LogDebug("Finished generating amendments");

                    logger.LogDebug("Start soft delete jobs activities and children");
                    await this.SoftDelete(jobsToDelete);

                    logger.LogDebug("Finished soft delete jobs activities and children");

                    transactionScope.Complete();
                }
            }
            catch (System.AggregateException ex)
            {
                //i have to handle the exception
                throw;
            }
            catch (Exception ex)
            {
                //i have to handle the exception
                logger.LogError("WellCleanUp Service has thrown an exception", ex);
                throw;
            }

            logger.LogDebug("Start delete completed");
        }

        private Task<List<NonSoftDeletedRoutesJobs>[]> FilterLookup(ILookup<int, NonSoftDeletedRoutesJobs> data)
        {
            return Task.WhenAll(data.Select(p => this.HandleBranchRoutes(p.ToList())).ToList());
        }

        private Task<List<NonSoftDeletedRoutesJobs>> HandleBranchRoutes(IList<NonSoftDeletedRoutesJobs> data)
        {
            return Task.Run(async () =>
            {
                var values = new ConcurrentBag<NonSoftDeletedRoutesJobs>();

                foreach (var item in data)
                {
                    DateTime compareDate;

                    if (item.JobRoyaltyCodeId.HasValue)
                    {
                        compareDate = await dateThresholdService.GracePeriodEndAsync(item.RouteDate, item.BranchId, item.JobRoyaltyCodeId.Value);
                    }
                    else
                    {
                        compareDate = await dateThresholdService.RouteGracePeriodEndAsync(item.RouteDate, item.BranchId);
                    }

                    if (compareDate <= DateTime.Now)
                    {
                        values.Add(item);
                    }
                };

                return values.ToList();
            });
        }

        private ILookup<int, NonSoftDeletedRoutesJobs> GetJobsFromRoutes()
        {
            return wellCleanUpRepository.GetNonSoftDeletedRoutes()
                .ToLookup(k => k.BranchId);
        }

        private Task SoftDelete(IList<int> jobIds)
        {
            return Task.Run(() =>
            {
                SoftDeleteInBatches(jobIds, configuration.SoftDeleteBatchSize);
            });
        }

        public void SoftDeleteInBatches(IList<int> jobIds, int batchSize)
        {
            if (batchSize <= 0)
            {
                throw  new ArgumentException("Batchsize must be greater than 0");
            }

            int offset = 0;
            int totalRecords = jobIds.Count;

            while (offset < totalRecords)
            {
                var jobIdBatch = jobIds.Skip(offset).Take(batchSize).ToList();
                DoSoftDelete(jobIdBatch);
                offset += batchSize;
            }
        }

        private void DoSoftDelete(List<int> jobIdBatch)
        {
            jobRepository.JobsSetResolutionStatusClosed(jobIdBatch);
            jobRepository.CascadeSoftDeleteJobs(jobIdBatch);
            wellCleanUpRepository.DeleteStops(jobIdBatch);
            wellCleanUpRepository.DeleteRoutes(jobIdBatch);
        }
    }
}

