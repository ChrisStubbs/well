namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IImportCommands
    {
        void UpdateExistingJob(Job fileJob, Job existingJob, RouteHeader routeHeader, bool isJobReplanned);

        void UpdateExistingJobFromReinstateJob(Job fileJob, ReinstateJob existingJob, RouteHeader routeHeader, bool isJobReplanned);

        void PostJobImport(IList<int> jobIds);

        IList<Job> GetJobsToBeDeleted(IList<JobStop> existingRouteJobIdAndStopId, IList<Tuple<int, int>> existingJobIdsBothSources, IList<Stop> completedStops);
    }
}