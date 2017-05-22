namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface IJobDetailActionRepository : IRepository<JobDetailAction, int>
    {
        void DeleteDrafts(int jobDetailId);
    }
}