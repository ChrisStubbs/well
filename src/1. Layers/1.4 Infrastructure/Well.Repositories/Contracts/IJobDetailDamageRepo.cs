namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface IJobDetailDamageRepo : IRepository<JobDetailDamage, int>
    {
        void Delete(int jobDetailId);
    }
}