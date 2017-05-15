namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Dapper;

    public interface IDapperReadProxy
    {
        IDapperReadProxy WithStoredProcedure(string storedProcedure);
        IEnumerable<TValueObject> Query<TValueObject>();
        void QueryMultiple<TValueObject>(Func<SqlMapper.GridReader, TValueObject> action);
        IDapperReadProxy AddParameter(string name, object parameter, DbType dbType, int? size = null);
    }
}