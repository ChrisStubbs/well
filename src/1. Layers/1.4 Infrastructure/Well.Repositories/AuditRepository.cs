namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using Domain;
    using Contracts;
    using Common.Contracts;

    public class AuditRepository : DapperRepository<Audit, int>, IAuditRepository
    {
        public AuditRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public IEnumerable<Audit> Get()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.AuditGet).Query<Audit>();
        }
    }
}
