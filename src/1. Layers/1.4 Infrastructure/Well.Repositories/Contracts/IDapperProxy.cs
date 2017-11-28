namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Dapper;

    public interface IDapperProxy
    {
        IDbConfiguration DbConfiguration { get; set; }

        IEnumerable<TEntity> Query<TEntity>();

        IEnumerable<TEntity> Query<TEntity>(string connectionString);

        Task<IEnumerable<TEntity>> QueryAsync<TEntity>(DynamicParameters parameters, string storeProcedureName);

        IEnumerable<TEntity> SqlQuery<TEntity>(string sql);

        TEntity QueryMultiple<TEntity>(Func<SqlMapper.GridReader, TEntity> action, string connectionString);
        TEntity QueryMultiple<TEntity>(Func<SqlMapper.GridReader, TEntity> action);

        IEnumerable<TEntity> QueryMultiples<TEntity>(Func<SqlMapper.GridReader, IEnumerable<TEntity>> action);

        void Execute();

        void Execute(DynamicParameters parameters, string storeProcedureName);

        void Execute(string connectionString);

        Task ExecuteAsync();

        Task ExecuteAsync(string connectionString);

        Task ExecuteAsync(DynamicParameters parameters, string storeProcedureName);

        void ExecuteSql(string sql);

        IDapperProxy WithStoredProcedure(string storedProcedure);

        IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null);

        void ExecuteSql(string statement, object parameters, Action<IDataReader> callBack);
    }
}