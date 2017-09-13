namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IJobDetailDamageRepository : IRepository<JobDetailDamage, int>
    {
        void Delete(int jobDetailId);

        IEnumerable<JobDetailDamage> GetJobDamagesByJobDetailId(int jobDetailId);
    }
}