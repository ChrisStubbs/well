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
    using Domain;

    public class WellCleanUpService : IWellCleanUpService
    {
        private readonly ILogger logger;
        private readonly IWellCleanUpRepository wellCleanUpRepository;
        private readonly IDateThresholdService dateThresholdService;
        private readonly IAmendmentService amendmentService;
        private readonly IJobRepository jobRepository;
        private readonly IWellCleanConfig configuration;
        private readonly IExceptionEventRepository exceptionEventRepository;

        public WellCleanUpService(
            ILogger logger,
            IWellCleanUpRepository wellCleanUpRepository,
            IDateThresholdService dateThresholdService,
            IAmendmentService amendmentService,
            IJobRepository jobRepository,
            IWellCleanConfig configuration,
            IExceptionEventRepository exceptionEventRepository)
        {
            this.logger = logger;
            this.wellCleanUpRepository = wellCleanUpRepository;
            this.dateThresholdService = dateThresholdService;
            this.amendmentService = amendmentService;
            this.jobRepository = jobRepository;
            this.configuration = configuration;
            this.exceptionEventRepository = exceptionEventRepository;
        }

        public async Task Clean()
        {
            logger.LogDebug("Start clean delete");

            var jobsForClean = this.GetJobsAvailableForClean();

            try
            {
                var data = await this.FilterLookup(jobsForClean);
                var jobsToDelete = data
                    .SelectMany(p => p)
                    .Select(p => p.JobId)
                    .ToList();

                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TimeSpan(0, 0, configuration.WellCleanTransactionTimeoutSeconds)))
                {
                    logger.LogDebug("Start generating amendments documents");
                    amendmentService.ProcessAmendments(jobsToDelete);
                    logger.LogDebug("Finished generating amendments documnets");

                    logger.LogDebug("Start soft delete jobs activities and children");
                    SoftDeleteInBatches(jobsToDelete, configuration.SoftDeleteBatchSize);

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

            await Task.Run(() => Console.WriteLine("bla"));
        }

        private Task<List<JobForClean>[]> FilterLookup(ILookup<int, JobForClean> data)
        {
            var nonProcessedEvents = exceptionEventRepository.GetAllUnprocessed().ToList();
            return Task.WhenAll(data.Select(p => this.HandleBranchRoutes(p.ToList(), nonProcessedEvents)).ToList());
        }

        private Task<List<JobForClean>> HandleBranchRoutes(IList<JobForClean> data, IList<ExceptionEvent> nonProcessedEvents)
        {
            return Task.Run(async () =>
            {
                var values = new ConcurrentBag<JobForClean>();
                ;
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
                        if (nonProcessedEvents.All(x => x.SourceId != item.JobId.ToString()))
                        {
                            values.Add(item);
                        }
                    }
                };

                return values.ToList();
            });
        }

        private ILookup<int, JobForClean> GetJobsAvailableForClean()
        {
            return wellCleanUpRepository.GetJobsAvailableForClean()
                .ToLookup(k => k.BranchId);
        }

        public void SoftDeleteInBatches(IList<int> jobIds, int batchSize)
        {
            if (batchSize <= 0)
            {
                batchSize = 1000;
            }

            double max = Math.Ceiling(jobIds.Count / (double)batchSize);

            foreach (var p in Enumerable.Range(0, (int)max))
            {
                DoClean(jobIds.Skip(p * batchSize).Take(batchSize).ToList());
            }
        }

        private void DoClean(List<int> jobIdBatch)
        {
            jobRepository.JobsSetResolutionStatusClosed(jobIdBatch);
            wellCleanUpRepository.CleanJobs(jobIdBatch);
            wellCleanUpRepository.CleanStops();
            wellCleanUpRepository.CleanRouteHeader();
            wellCleanUpRepository.CleanRoutes();
            wellCleanUpRepository.CleanActivities();
        }

    }
}

