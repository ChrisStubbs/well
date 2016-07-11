namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Dapper;

    public interface IDapperProxy
    {
        IDbConfiguration DbConfiguration { get; set; }

        IEnumerable<TEntity> Query<TEntity>();
        IEnumerable<TEntity> SqlQuery<TEntity>(string sql);

        void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action);

        void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, TEntity> action);

        void Execute();
        void ExecuteSql(string sql);

        IDapperProxy WithStoredProcedure(string storedProcedure);

        IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null);

    }
}