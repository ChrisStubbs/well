namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using PH.Well.Domain;

    public interface IAuditRepository :IRepository<Audit, int>
    {
        IEnumerable<Audit> Get();
    }
}
