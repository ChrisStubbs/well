namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IImportCommands
    {
        void UpdateExistingJob(Job fileJob, Job existingJob, RouteHeader routeHeader);
        void PostJobImport(IList<int> jobIds);
    }
}