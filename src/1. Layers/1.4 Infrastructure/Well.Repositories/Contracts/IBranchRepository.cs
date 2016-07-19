namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IBranchRepository : IRepository<Branch, int>
    {
        IEnumerable<Branch> GetAll();
    }
}