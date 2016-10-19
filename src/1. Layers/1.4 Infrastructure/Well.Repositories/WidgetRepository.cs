namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain;

    using WebGrease.Css.Extensions;


    public class WidgetRepository : DapperRepository<WidgetWarning, int>, IWidgetRepository
    {
        public WidgetRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        protected override void SaveNew(WidgetWarning entity)
        {
            using (
                var transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    TimeSpan.FromMinutes(Configuration.TransactionTimeout)))
            {
                if (!entity.IsTransient()) this.Delete(entity.Id);

                entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.WidgetWarningSave)
                    .AddParameter("WidgetName", entity.WidgetName, DbType.String, size: 50)
                    .AddParameter("WarningLevel", entity.WarningLevel, DbType.Int32)
                    .AddParameter("Type", entity.Type, DbType.Int16)
                    .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                    .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                    .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                    .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
                    .Query<int>().Single();

                foreach (var branch in entity.Branches)
                {
                    this.dapperProxy.WithStoredProcedure(StoredProcedures.WidgetWarningToBranchSave)
                        .AddParameter("BranchId", branch.Id, DbType.Int32)
                        .AddParameter("WidgetId", entity.Id, DbType.Int32)
                        .Execute();
                }

                transactionScope.Complete();

            }
        }


        public IEnumerable<WidgetWarning> GetAll()
        {
            var widgetWarnings = this.dapperProxy.WithStoredProcedure(StoredProcedures.WidgetWarningGetAll).Query<WidgetWarning>();

            foreach (var warning in widgetWarnings)
            {
                var branches = this.dapperProxy.WithStoredProcedure(StoredProcedures.WidgetWarningBranchesGet)
                    .AddParameter("widgetId", warning.Id, DbType.Int32).Query<Branch>();

                branches.ForEach(x => warning.Branches.Add(x));
            }

            return widgetWarnings;
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.WidgetWarningDelete)
                .AddParameter("Id", id, DbType.Int32).Execute();
        }
    }
}
