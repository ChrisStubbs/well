namespace PH.Well.Repositories.DependancyEvents
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Security.Permissions;
    using Contracts;
    using Domain.Enums;

    public sealed class DbChangeNotifier : IDisposable, IDbChangeNotifier
    {
        private readonly IWellDbConfiguration wellDbConfiguration;
        private SqlConnection sqlConnection;
        private SqlCommand sqlCommand;
        private SqlDependency sqlDependency;

        public DbChangeNotifier(IWellDbConfiguration wellDbConfiguration)
        {
            this.wellDbConfiguration = wellDbConfiguration;
        }

        ~DbChangeNotifier()
        {
            this.Dispose();
        }

        public event EventHandler<ChangeEventArgs> Change;

        public Boolean Start(String dependencyCheckSql)
        {
            new SqlClientPermission(PermissionState.Unrestricted).Demand();

            bool result = SqlDependency.Start(wellDbConfiguration.DatabaseConnection);

            this.sqlConnection = new SqlConnection(wellDbConfiguration.DatabaseConnection);
            this.sqlConnection.Open();
            this.sqlCommand = this.sqlConnection.CreateCommand();
            this.sqlCommand.CommandType = CommandType.StoredProcedure;
            this.sqlCommand.CommandText = dependencyCheckSql;

            this.Setup(true);

            return result;
        }

        public Boolean Stop()
        {
            var result = false;

            if (this.sqlCommand != null)
            {
                this.sqlCommand.Notification = null;
                this.sqlCommand.Dispose();
                this.sqlCommand = null;
            }

            if (this.sqlConnection != null)
            {
                this.sqlConnection.Close();
                this.sqlConnection = null;
            }

            if (this.sqlDependency != null)
            {
                result = SqlDependency.Stop(wellDbConfiguration.DatabaseConnection);
                this.sqlDependency.OnChange -= this.OnChange;
                this.sqlDependency = null;
            }

            this.Change = null;

            return result;
        }

        private void Setup(Boolean initial)
        {
            if (initial == false)
            {
                this.sqlDependency.OnChange -= this.OnChange;
            }

            this.sqlCommand.Notification = null;
            this.sqlDependency = new SqlDependency(this.sqlCommand);
            this.sqlDependency.OnChange += this.OnChange;

            this.sqlCommand.ExecuteScalar();
        }

        private void OnChange(Object sender, SqlNotificationEventArgs e)
        {
            this.Setup(false);

            if (this.Change != null)
            {
                this.Change(sender,
                    new ChangeEventArgs((ChangeInfo) (Int32) e.Info, (ChangeSource) (Int32) e.Source,(ChangeType) (Int32) e.Type));
            }
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}
