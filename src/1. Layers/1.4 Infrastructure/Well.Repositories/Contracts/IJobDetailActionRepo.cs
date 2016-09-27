namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface IJobDetailActionRepo : IRepository<JobDetailAction, int>
    {
        void DeleteDrafts(int jobDetailId);
    }
}