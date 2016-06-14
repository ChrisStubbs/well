namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Dapper;

    public interface IDapperProxy
    {
        string Connection { get; set; }

        void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action);

        void QueryMultiple<TEntity>(Func<SqlMapper.GridReader, TEntity> action);

        void Execute();

        IDapperProxy WithStoredProcedure(string storedProcedure);

        IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null);

        IEnumerable<TEntity> Query<TEntity>();
    }
}