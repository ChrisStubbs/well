namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.ValueObjects;

    public interface IWellCleanUpRepository
    {
        IList<JobForClean> GetJobsAvailableForClean();

        void CleanStops();

        void CleanRouteHeader();

        void CleanActivities();

        void CleanJobs(IList<int> jobIds);

        void CleanRoutes();

        void UpdateStatistics();

        void CleanExceptionEvents();
    }
}