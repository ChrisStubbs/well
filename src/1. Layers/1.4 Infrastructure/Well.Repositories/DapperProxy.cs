namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Dapper;

    using PH.Well.Repositories.Contracts;

    public class DapperProxy : IDapperProxy
    {
        DynamicParameters parameters;

        private string storedProcedure;

        private string sql;

        public IDapperProxy WithStoredProcedure(string storedProcedure)
        {
            this.storedProcedure = storedProcedure;

            return this;
        }

        public IDapperProxy WithSql(string sql)
        {
            this.sql = sql;

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

        public TEntity Query<TEntity>(string connectionString)
        {
            return Query<TEntity>(connectionString, storedProcedure, parameters).Single();
        }

        public IEnumerable<TEntity> QueryMany<TEntity>(string connectionString)
        {
            return Query<TEntity>(connectionString, storedProcedure, parameters);
        }

        private IEnumerable<TEntity> Query<TEntity>(string connectionString, string storedProcedure, object parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<TEntity>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: Configuration.TransactionTimeout).AsQueryable();
            }
        }
    }
}