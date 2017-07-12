namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Contracts;
    using Dapper;

    public abstract class BaseDapperProxy : IDapperProxy
    {
        public IDbConfiguration DbConfiguration { get; set; }

        DynamicParameters parameters;
        private string storedProcedure;


        public IEnumerable<TEntity> Query<TEntity>()
        {
            return this.QueryDapper<TEntity>();
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                return connection.Query<TEntity>(sql, param: null, commandType: CommandType.Text, commandTimeout: DbConfiguration.CommandTimeout).AsQueryable();
            }
        }

        public void ExecuteSql(string statement, object parameters, Action<IDataReader> callBack)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                var reader = connection.ExecuteReader(statement, parameters);
                callBack(reader);
            }
        }

        public TEntity QueryMultiple<TEntity>(Func<SqlMapper.GridReader, TEntity> action)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                var x = action(connection.QueryMultiple(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure));

                this.parameters = null;

                return x;
            }
        }

        public IEnumerable<TEntity> QueryMultiples<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                var x = action(connection.QueryMultiple(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure));

                this.parameters = null;
                return x;
            }
        }

        public void Execute()
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: Configuration.TransactionTimeout);

                this.parameters = null;
            }
        }

        public void ExecuteSql(string sql)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                connection.Execute(sql, param: null, commandType: CommandType.Text, commandTimeout: DbConfiguration.CommandTimeout);
            }
        }

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

        private IEnumerable<TEntity> QueryDapper<TEntity>()
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                var result = connection.Query<TEntity>(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure, commandTimeout: DbConfiguration.CommandTimeout).AsQueryable();

                this.parameters = null;

                return result;
            }
        }
    }
}