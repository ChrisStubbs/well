namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.ValueObjects;

    public interface IWellCleanUpRepository
    {
        IList<NonSoftDeletedRoutesJobs> GetNonSoftDeletedRoutes();
        Task SoftDelete(IList<int> jobIds, string deletedBy);
    }
}