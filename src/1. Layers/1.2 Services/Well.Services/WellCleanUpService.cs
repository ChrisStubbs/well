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
    using System.Diagnostics;
    using Common;
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
        private readonly IDbConfiguration dbConfig;

        public WellCleanUpService(
            ILogger logger,
            IWellCleanUpRepository wellCleanUpRepository,
            IDateThresholdService dateThresholdService,
            IAmendmentService amendmentService,
            IJobRepository jobRepository,
            IWellCleanConfig configuration,
            IExceptionEventRepository exceptionEventRepository,
            IDbConfiguration dbConfig)
        {
            this.logger = logger;
            this.wellCleanUpRepository = wellCleanUpRepository;
            this.dateThresholdService = dateThresholdService;
            this.amendmentService = amendmentService;
            this.jobRepository = jobRepository;
            this.configuration = configuration;
            this.exceptionEventRepository = exceptionEventRepository;
            this.dbConfig = dbConfig;
        }

        public async Task Clean()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Restart();
            logger.LogDebug("Start clean process");
            int batchSize;
            int noOfJobs;
            int totalNoOfBatches;
            var jobsForClean = this.GetJobsAvailableForClean();

            try
            {
                batchSize = 1000;
                if (configuration.CleanBatchSize > 0)
                {
                    batchSize = configuration.CleanBatchSize;
                }

                logger.LogDebug($"WellCleanTransactionTimeoutSeconds: {configuration.WellCleanTransactionTimeoutSeconds}");
                logger.LogDebug($"transactionTimeoutSeconds: {dbConfig.TransactionTimeout}");
                logger.LogDebug($"CommandTimeoutSeconds: {dbConfig.CommandTimeout}");
                logger.LogDebug($"Batch Size: {batchSize}");
                
                logger.LogDebug("Start determining jobs to clean");
                var data = await this.FilterLookup(jobsForClean);
                var jobsToDelete = data
                    .SelectMany(p => p)
                    .Select(p => p.JobId)
                    .ToList();
                logger.LogDebug("Finish determining jobs to clean");

                var batchNo = 0;
                noOfJobs = jobsToDelete.Count;
                totalNoOfBatches = (noOfJobs + batchSize - 1) / batchSize;

                logger.LogDebug($"Start cleaning in batches");
                logger.LogDebug($"No Of Jobs to clean: {noOfJobs}");
                foreach (var jobs in Utilities.Batch(jobsToDelete, batchSize))
                {
                    batchNo++;
                    logger.LogDebug($"Processing batch {batchNo} of {totalNoOfBatches}");
                    var jobList = jobs.ToList();
                    ProcessAmmendmentsAndClean(jobList);
                }

                logger.LogDebug("Start clean Exception Events");
                wellCleanUpRepository.CleanExceptionEvents();
                logger.LogDebug("Finished clean Exception Events");

                logger.LogDebug("Start update statistics");
                wellCleanUpRepository.UpdateStatistics();
                logger.LogDebug("Finished update statistics");
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

            var ts = stopWatch.Elapsed;

            var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";

            logger.LogDebug($"Clean process cleaned {noOfJobs} jobs in {totalNoOfBatches} batches of {batchSize}" +
                            $"and took {elapsedTime}");

            await Task.Run(() => Console.WriteLine("Complete"));
        }

        private void ProcessAmmendmentsAndClean(List<int> jobList)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TimeSpan(0, 0, configuration.WellCleanTransactionTimeoutSeconds)))
            {
                logger.LogDebug("Start generating amendments documents");
                amendmentService.ProcessAmendments(jobList);
                logger.LogDebug("Finished generating amendments documnets");

                logger.LogDebug("Start clean of jobs activities and children");
                CleanJobsStopsRoutesAndActivities(jobList);
                logger.LogDebug("Finished clean of jobs activities and children");
                transactionScope.Complete();
            }
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
                CleanJobsStopsRoutesAndActivities(jobIds.Skip(p * batchSize).Take(batchSize).ToList());
            }
        }

        private void CleanJobsStopsRoutesAndActivities(List<int> jobIdBatch)
        {
            logger.LogDebug("Set Resolution Status");
            jobRepository.JobsSetResolutionStatusClosed(jobIdBatch);
            logger.LogDebug("Clean Jobs");
            wellCleanUpRepository.CleanJobs(jobIdBatch);
            logger.LogDebug("Clean Stops");
            wellCleanUpRepository.CleanStops();
            logger.LogDebug("Clean Routeheader");
            wellCleanUpRepository.CleanRouteHeader();
            logger.LogDebug("Clean Routes");
            wellCleanUpRepository.CleanRoutes();
            logger.LogDebug("Clean Activities");
            wellCleanUpRepository.CleanActivities();
        }

    }
}

