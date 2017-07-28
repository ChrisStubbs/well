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

    public class WellCleanUpService : IWellCleanUpService
    {
        private readonly ILogger logger;
        private readonly IWellCleanUpRepository wellCleanUpRepository;
        private readonly IDateThresholdService dateThresholdService;
        private readonly IAmendmentService amendmentService;
        private readonly IJobRepository jobRepository;
        private static string user = "WellCleanUpService";

        public WellCleanUpService(
            ILogger logger,
            IWellCleanUpRepository wellCleanUpRepository,
            IDateThresholdService dateThresholdService,
            IAmendmentService amendmentService,
            IJobRepository jobRepository)
        {
            this.logger = logger;
            this.wellCleanUpRepository = wellCleanUpRepository;
            this.dateThresholdService = dateThresholdService;
            this.amendmentService = amendmentService;
            this.jobRepository = jobRepository;
        }

        public async Task SoftDelete()
        {
            logger.LogDebug("Start soft delete");

            var routesData = this.GetJobsFromRoutes();

            try
            {
                var data = await this.FilterLookup(routesData);
                var jobsToDelete = data
                    .Select(p => p.JobId)
                    .ToList();

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    logger.LogDebug("Start generating amendments delete");
                    await amendmentService.ProcessAmendmentsAsync(jobsToDelete);
                    logger.LogDebug("Finished generating amendments");

                    logger.LogDebug("Start soft delete jobs activities and children");
                    await this.SoftDelete(jobsToDelete, user);




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
                throw;
            }

            logger.LogDebug("Start soft completed");
        }

        private Task<List<NonSoftDeletedRoutesJobs>> FilterLookup(ILookup<int, NonSoftDeletedRoutesJobs> data)
        {
            return Task.Run<List<NonSoftDeletedRoutesJobs>>(() =>
            {
                var values = new List<NonSoftDeletedRoutesJobs>();
                var tasks = new Task<List<NonSoftDeletedRoutesJobs>>[data.Count];
                var index = 0;

                foreach (var item in data)
                {
                    tasks[index] = this.HandleBranchRoutes(item.ToList());
                    
                    index++;
                }

                foreach (var t in tasks)
                {
                    values.AddRange(t.Result);
                }

                return values;
            });
        }

        private Task<List<NonSoftDeletedRoutesJobs>> HandleBranchRoutes(IList<NonSoftDeletedRoutesJobs> data)
        {
            return Task.Run(async () =>
            {
                var values = new List<NonSoftDeletedRoutesJobs>();

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
                }

                return values;
            });
        }

        private ILookup<int, NonSoftDeletedRoutesJobs> GetJobsFromRoutes()
        {
            return wellCleanUpRepository.GetNonSoftDeletedRoutes()
                .ToLookup(k => k.BranchId);
        }

        private Task SoftDelete(IList<int> jobIds, string deletedBy)
        {
            return Task.Run(() =>
            {
                jobRepository.JobsSetResolutionStatusClosed(jobIds, deletedBy);
                jobRepository.CascadeSoftDeleteJobs(jobIds, deletedBy);
                wellCleanUpRepository.DeleteStops(jobIds, deletedBy);
                wellCleanUpRepository.DeleteRoutes(jobIds, deletedBy);
            });
        }
    }
}

