namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Dapper;

    public interface IDapperProxy
    {
        IDbConfiguration DbConfiguration { get; set; }

        IEnumerable<TEntity> Query<TEntity>(string storedProcedure, object parameters = null);
        IEnumerable<TEntity> SqlQuery<TEntity>(string sql);

        void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action, string sql, CommandType commandType = CommandType.StoredProcedure, object parameters = null);

        void Execute(string storedProcedure, object parameters = null);
        void ExecuteSql(string sql);

    }
}