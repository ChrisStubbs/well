﻿namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Transactions;

    using Common.Contracts;
    using PH.Well.Domain;
    using Contracts;

    using WebGrease.Css.Extensions;
    using System.Threading.Tasks;
    using Dapper;

    public class SeasonalDateRepository : DapperRepository<SeasonalDate, int>, ISeasonalDateRepository
    {
        public SeasonalDateRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider) 
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        protected override void SaveNew(SeasonalDate entity)
        {
            using (
                var transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    TimeSpan.FromMinutes(Configuration.TransactionTimeout)))
            {
                if (!entity.IsTransient()) this.Delete(entity.Id);

                entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesSave)
                    .AddParameter("Description", entity.Description, DbType.String, size: 255)
                    .AddParameter("From", entity.From, DbType.DateTime)
                    .AddParameter("To", entity.To, DbType.DateTime)
                    .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                    .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                    .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                    .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
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
            var seasonalDates = this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesGetAll).Query<SeasonalDate>();

            foreach (var seasonalDate in seasonalDates)
            {
                var branches = this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesBranchesGet)
                    .AddParameter("seasonalDateId", seasonalDate.Id, DbType.Int32).Query<Branch>();

                branches.ForEach(x => seasonalDate.Branches.Add(x));
            }
            
            return seasonalDates;
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesDelete)
                .AddParameter("Id", id, DbType.Int32).Execute();
        }

        public IEnumerable<SeasonalDate> GetByBranchId(int branchId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.SeasonalDatesByBranchGet)
                    .AddParameter("branchId", branchId, DbType.Int32)
                    .Query<SeasonalDate>();
        }

        public Task<IEnumerable<SeasonalDate>> GetByBranchIdAsync(int branchId)
        {
            var p = new DynamicParameters();

            p.Add("branchId", branchId, DbType.Int32);

            return this.dapperProxy.QueryAsync<SeasonalDate>(p, StoredProcedures.SeasonalDatesByBranchGet);
        }
    }
}