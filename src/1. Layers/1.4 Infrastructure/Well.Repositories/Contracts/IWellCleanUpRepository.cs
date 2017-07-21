namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IWellCleanUpRepository
    {
        IEnumerable<RouteBranch> GetNonSoftDeletedRoutes();
        IEnumerable<int> GetJobsWithNoOustandingExceptions(IEnumerable<int> routeIds);
        void SoftDeleteJobsActivitiesAndChildren(IEnumerable<int> jobIds);
    }
}