namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IJobDetailDamageRepo : IRepository<JobDetailDamage, int>
    {
        void Delete(int jobDetailId);

        IEnumerable<JobDetailDamage> GetJobDamagesByJobDetailId(int jobDetailId);
    }
}