namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Transactions;

    using Common.Contracts;
    using PH.Well.Domain;
    using Contracts;
    
    public class SeasonalDateRepository : DapperRepository<SeasonalDate, int>, ISeasonalDateRepository
    {
        public SeasonalDateRepository(ILogger logger, IDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        protected override void SaveNew(SeasonalDate entity)
        {
            using (
                var transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    TimeSpan.FromMinutes(Configuration.TransactionTimeout)))
            {
                entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesSave)
                    .AddParameter("Description", entity.Description, DbType.String, size: 255)
                    .AddParameter("From", entity.From, DbType.DateTime)
                    .AddParameter("To", entity.To, DbType.DateTime)
                    .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                    .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                    .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                    .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                    .Query<int>().Single();

                foreach (var branch in entity.Branches)
                {
                    this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesToBranchSave)
                        .AddParameter("BranchId", branch.Id, DbType.Int32)
                        .AddParameter("SeasonalDateId", entity.Id, DbType.Int32)
                        .Execute();
                }

                transactionScope.Complete();
            }
        }

        public IEnumerable<SeasonalDate> GetAll()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesGetAll).Query<SeasonalDate>();
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesDelete)
                .AddParameter("Id", id, DbType.Int32).Execute();
        }
    }
}