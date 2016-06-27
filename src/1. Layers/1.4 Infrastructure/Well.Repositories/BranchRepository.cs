namespace PH.Well.Repositories
{
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class BranchRepository : DapperRepository<Branch, int>, IBranchRepository
    {
        public BranchRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }
    }
}