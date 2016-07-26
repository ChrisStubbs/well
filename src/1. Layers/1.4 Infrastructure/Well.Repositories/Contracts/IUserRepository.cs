namespace PH.Well.Repositories.Contracts
{
    using PH.Well.Domain;

    public interface IUserRepository : IRepository<User, int>
    {
        User GetByName(string name);
    }
}