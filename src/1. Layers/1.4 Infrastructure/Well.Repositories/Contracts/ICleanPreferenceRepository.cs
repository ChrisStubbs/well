namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface ICleanPreferenceRepository : IRepository<CleanPreference, int>
    {
        IEnumerable<CleanPreference> GetAll();

        CleanPreference GetByBranchId(int branchId);

        void Delete(int id);
    }
}