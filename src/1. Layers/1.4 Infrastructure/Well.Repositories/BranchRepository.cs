namespace PH.Well.Repositories
{
    using System.Collections.Generic;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class BranchRepository : DapperRepository<Branch, int>, IBranchRepository
    {
        public BranchRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public IEnumerable<Branch> GetAll()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.BranchesGet).Query<Branch>();
        }
    }
}