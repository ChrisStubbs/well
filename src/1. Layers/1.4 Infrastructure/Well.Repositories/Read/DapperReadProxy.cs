namespace PH.Well.Repositories.Read
{
    using System;
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
            return Query<TValueObject>(DbConfiguration.DatabaseConnection);
        }

        public IEnumerable<TValueObject> Query<TValueObject>(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    return connection.Query<TValueObject>(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure, commandTimeout: DbConfiguration.CommandTimeout).AsQueryable();
                }
                finally
                {
                    this.parameters = null;
                }
            }
        }

        public void QueryMultiple<TValueObject>(Func<SqlMapper.GridReader, TValueObject> action)
        {
            using (var connection = new SqlConnection(DbConfiguration.DatabaseConnection))
            {
                try
                {
                    action(connection.QueryMultiple(this.storedProcedure, this.parameters, commandType: CommandType.StoredProcedure));
                }
                finally
                {
                    this.parameters = null;
                }
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
