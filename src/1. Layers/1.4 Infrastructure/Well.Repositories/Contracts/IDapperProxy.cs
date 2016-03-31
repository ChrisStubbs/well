namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Data;

    public interface IDapperProxy
    {
        IDapperProxy WithStoredProcedure(string storedProcedure);

        IDapperProxy WithSql(string sql);

        IDapperProxy AddParameter(string name, object parameter, DbType dbType, int? size = null);

        TEntity Query<TEntity>(string connectionString);

        IEnumerable<TEntity> QueryMany<TEntity>(string connectionString);
    }
}