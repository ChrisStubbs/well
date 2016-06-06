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
        private DynamicParameters parameters;

        private string storedProcedure;

        public string ConnectionString { get; set; }

        public IDapperProxy WithStoredProcedure(string storedProcedure)
        {
            this.storedProcedure = storedProcedure;

            return this;
        }

        public IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null)
        {
            if (parameters == null) parameters = new DynamicParameters();

            if (size.HasValue)
            {
                parameters.Add(name, parameter, dbType, size: size);
            }
            else
            {
                parameters.Add(name, parameter, dbType);
            }

            return this;
        }

        public TEntity Query<TEntity>()
        {
            return DapperQuery<TEntity>().Single();
        }

        public IEnumerable<TEntity> QueryMany<TEntity>()
        {
            return DapperQuery<TEntity>();
        }

        public void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                action(connection.QueryMultiple(storedProcedure, parameters, commandType: CommandType.StoredProcedure));
            }
        }

        public void Execute()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: Configuration.TransactionTimeout);
            }
        }

        private IEnumerable<TEntity> DapperQuery<TEntity>()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                return connection.Query<TEntity>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: Configuration.TransactionTimeout).AsQueryable();
            }
        }
    }
}