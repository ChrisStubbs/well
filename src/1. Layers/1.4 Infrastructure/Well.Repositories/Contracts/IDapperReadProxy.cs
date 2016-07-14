namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Data;

    public interface IDapperReadProxy
    {
        IDapperReadProxy WithStoredProcedure(string storedProcedure);
        IEnumerable<TValueObject> Query<TValueObject>();
        IDapperReadProxy AddParameter(string name, object parameter, DbType dbType, int? size = null);
    }
}