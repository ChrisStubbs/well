namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Domain;
    using Contracts;
    using Common.Contracts;
    using Domain.Enums;

    public class AuditRepository : DapperRepository<Audit, int>, IAuditRepository
    {
        public AuditRepository(ILogger logger, IWellDapperProxy dapperProxy, IUserNameProvider userNameProvider) 
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        public IEnumerable<Audit> Get()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.AuditGet).Query<Audit>();
        }

        protected override void SaveNew(Audit audit)
        {
            audit.Id = dapperProxy.WithStoredProcedure(StoredProcedures.AuditInsert)
                .AddParameter("Entry", audit.Entry, DbType.String)
                .AddParameter("Type", audit.Type, DbType.Int32)
                .AddParameter("InvoiceNumber", audit.InvoiceNumber, DbType.String)
                .AddParameter("AccountCode", audit.AccountCode, DbType.String)
                .AddParameter("DeliveryDate", audit.DeliveryDate, DbType.DateTime)
                .AddParameter("CreatedBy", audit.CreatedBy, DbType.String)
                .AddParameter("DateCreated", audit.DateCreated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }
    }
}
