namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Dapper;

    public interface IDapperProxy
    {
        string ConnectionString { get; set; }

        IDapperProxy WithStoredProcedure(string storedProcedure);

        IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null);

        TEntity Query<TEntity>();

        IEnumerable<TEntity> QueryMany<TEntity>();

        void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action);

        void Execute();
    }
}