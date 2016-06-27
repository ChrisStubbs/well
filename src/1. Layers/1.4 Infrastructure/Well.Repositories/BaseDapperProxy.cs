

namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Contracts;
    using Dapper;
    using Microsoft.AspNet.SignalR;

    public abstract class BaseDapperProxy
    {
        public IDbConfiguration DbConfiguration { get; set; }

        public IEnumerable<TEntity> Query<TEntity>(string storedProcedure, object parameters = null)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                return connection.Query<TEntity>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: DbConfiguration.CommandTimeout).AsQueryable();
            }
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                return connection.Query<TEntity>(sql, param: null, commandType: CommandType.Text, commandTimeout: DbConfiguration.CommandTimeout).AsQueryable();
            }
        }

        public void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action, string sql, CommandType commandType = CommandType.StoredProcedure, object parameters = null)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                action(connection.QueryMultiple(sql, parameters, commandType: commandType, commandTimeout: DbConfiguration.CommandTimeout));
            }
        }

        public void Execute(string storedProcedure, object parameters = null)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: DbConfiguration.CommandTimeout);
            }
        }

        public void ExecuteSql(string sql)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                connection.Execute(sql, param: null, commandType: CommandType.Text, commandTimeout: DbConfiguration.CommandTimeout);
            }
        }


    }
}
