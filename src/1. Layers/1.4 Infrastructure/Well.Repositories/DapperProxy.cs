namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Dapper;

    using PH.Well.Repositories.Contracts;

    public class DapperProxy : IDapperProxy
    {
        public string Connection { get; set; }

        DynamicParameters parameters;

        private string storedProcedure;

        public IDapperProxy WithStoredProcedure(string storedProcedure)
        {
            this.storedProcedure = storedProcedure;

            return this;
        }

        public IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null)
        {
            if (this.parameters == null) this.parameters = new DynamicParameters();

            if (size.HasValue)
            {
                this.parameters.Add(name, parameter, dbType, size: size);
            }
            else
            {
                this.parameters.Add(name, parameter, dbType);
            }

            return this;
        }

        public IEnumerable<TEntity> Query<TEntity>()
        {
            return this.QueryDapper<TEntity>();
        }

        public void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action)
        {
            using (var connection = new SqlConnection(this.Connection))
            {
                action(connection.QueryMultiple(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure));

                this.parameters = null;
            }
        }

        public void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, TEntity> action)
        {
            using (var connection = new SqlConnection(this.Connection))
            {
                action(connection.QueryMultiple(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure));

                this.parameters = null;
            }
        }

        public void Execute()
        {
            using (var connection = new SqlConnection(this.Connection))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: Configuration.TransactionTimeout);

                this.parameters = null;
            }
        }

        private IEnumerable<TEntity> QueryDapper<TEntity>()
        {
            using (var connection = new SqlConnection(this.Connection))
            {
                var result = connection.Query<TEntity>(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure, commandTimeout: Configuration.TransactionTimeout).AsQueryable();

                this.parameters = null;

                return result;
            }
        }
    }
}