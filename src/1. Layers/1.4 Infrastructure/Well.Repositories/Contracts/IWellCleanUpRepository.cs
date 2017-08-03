namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IWellCleanUpRepository
    {
        IList<NonSoftDeletedRoutesJobs> GetNonSoftDeletedRoutes();
        void DeleteStops(IList<int> jobIds);
        void DeleteRoutes(IList<int> jobIds);
    }
}