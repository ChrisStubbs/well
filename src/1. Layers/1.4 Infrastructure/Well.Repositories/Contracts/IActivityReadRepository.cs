namespace PH.Well.Repositories.Contracts
{
    using Domain;

    public interface IActivityReadRepository
    {
        Activity GetById(int id);
    }
}
