namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Contracts;
    using Dapper;

    public class DapperReadProxy : IDapperReadProxy
    {
        public readonly IDbConfiguration DbConfiguration;
        DynamicParameters parameters;
        private string storedProcedure;

        public DapperReadProxy(IDbConfiguration dbConfiguration)
        {
            this.DbConfiguration = dbConfiguration;
        }

        public IDapperReadProxy WithStoredProcedure(string storedProcedure)
        {
            this.storedProcedure = storedProcedure;

            return this;
        }

        public IEnumerable<TValueObject> Query<TValueObject>()
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                var result = connection.Query<TValueObject>(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure, commandTimeout: DbConfiguration.CommandTimeout).AsQueryable();

                this.parameters = null;

                return result;
            }
        }

        public IDapperReadProxy AddParameter(string name, object parameter, DbType dbType, int? size = null)
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
    }
}
