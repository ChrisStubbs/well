namespace PH.Well.Services
{
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Repositories.Contracts;

    public class WellCleanUpService
    {
        private readonly ILogger logger;
        private readonly IWellCleanUpRepository wellCleanUpRepository;
        private readonly IDateThresholdService dateThresholdService;
        private readonly IAmendmentService amendmentService;

        public WellCleanUpService(
            ILogger logger,
            IWellCleanUpRepository wellCleanUpRepository,
            IDateThresholdService dateThresholdService,
            IAmendmentService amendmentService)
        {
            this.logger = logger;
            this.wellCleanUpRepository = wellCleanUpRepository;
            this.dateThresholdService = dateThresholdService;
            this.amendmentService = amendmentService;
        }

        public void SoftDelete()
        {
            this.logger.LogDebug("Start soft delete");

            var routesAvailableForSoftDelete = wellCleanUpRepository.GetNonSoftDeletedRoutes()
                .Where(r => r.RouteDate >= dateThresholdService.BranchGracePeriodEndDate(r.RouteDate, r.BranchId));

            var jobIdsToDelete = wellCleanUpRepository.GetJobsWithNoOustandingExceptions(routesAvailableForSoftDelete.Select(x => x.RouteId)).ToArray();

            using (var transactionScope = new TransactionScope())
            {
                logger.LogDebug("Start generating amendments delete");
                amendmentService.ProcessAmendments(jobIdsToDelete);
                logger.LogDebug("Finished generating amendments");

                logger.LogDebug("Start soft delete jobs activities and children");
                wellCleanUpRepository.SoftDeleteJobsActivitiesAndChildren(jobIdsToDelete);
                logger.LogDebug("Finished soft delete jobs activities and children");

                transactionScope.Complete();
            }

            
            

        }



    }





}

