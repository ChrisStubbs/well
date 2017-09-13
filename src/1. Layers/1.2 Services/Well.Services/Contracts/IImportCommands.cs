namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IImportCommands
    {
        void UpdateExistingJob(Job fileJob, Job existingJob, RouteHeader routeHeader);
        void PostJobImport(IList<int> jobIds);
        IList<Job> GetJobsToBeDeleted(IList<JobStop> existingRouteJobIdAndStopId, IList<Job> existingJobsBothSources, IList<Stop> completedStops);
    }
}