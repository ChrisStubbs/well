namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Transactions;

    using Dapper;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Contracts;
    using PH.Well.Repositories.Contracts;

    public abstract class DapperRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        public int? TotalRecords { get; private set; }

        public string CurrentUser { get; set; }

        protected readonly ILogger logger;

        protected readonly IDapperProxy dapperProxy;


        protected DapperRepository(ILogger logger, IDapperProxy dapperProxy)
        {
            this.logger = logger;
            this.dapperProxy = dapperProxy;
        }

        protected virtual void SaveNew(TEntity entity)
        {
            throw new NotSupportedException();
        }

        protected virtual void UpdateExisting(TEntity entity)
        {
            throw new NotSupportedException();
        }

        protected virtual DataTable CreateDataTable(IEnumerable<TEntity> entities)
        {
            throw new NotSupportedException();
        }

        public void Update(TEntity entity)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    entity.SetUpdatedProperties(this.CurrentUser);
                    this.UpdateExisting(entity);

                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError($"{entity.GetType().Name} : Id {entity.Id}", exception);
                throw;
            }
        }

        public void Save(TEntity entity)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    entity.SetCreatedProperties(this.CurrentUser);
                    this.SaveNew(entity);

                    transaction.Complete();
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError($"{entity.GetType().Name} : Id {entity.Id}", exception);
                throw;
            }
        }

        public void BulkCopyInsert(IEnumerable<TEntity> entities, string tableName, IEnumerable<string> mappings)
        {
            foreach (var entity in entities)
            {
                entity.SetCreatedProperties(this.CurrentUser);
            }

            using (var bulkCopy = new SqlBulkCopy(this.dapperProxy.Connection, SqlBulkCopyOptions.CheckConstraints))
            {
                bulkCopy.DestinationTableName = tableName;

                foreach (var mapping in mappings)
                {
                    bulkCopy.ColumnMappings.Add(mapping, mapping);
                }

                var dataTable = this.CreateDataTable(entities);

                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
