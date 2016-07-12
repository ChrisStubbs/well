namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface IJobRepository : IRepository<Job, int>
    {
        void AddJobAttributes(Attribute attribute);
        Job GetById(int id);
        Job JobCreateOrUpdate(Job job);
    }
}