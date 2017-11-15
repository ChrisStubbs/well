namespace PH.Well.Api.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Models;

    public class WellApiConnectionStringFactory : IConnectionStringFactory
    {
        private const string AppSettingFinder = "ConnectionString-";
        private IList<BranchConnection> branchConnections;

        public IList<BranchConnection> BranchConnections => branchConnections ?? (branchConnections = GetBranchConnections());

        public List<BranchConnection> GetBranchConnections()
        {
            var connections = ConfigurationManager.ConnectionStrings;
            var appSettings = ConfigurationManager.AppSettings;
            var branchCons = new List<BranchConnection>();

            foreach (string appSettingKey in appSettings.AllKeys)
            {
                if (appSettingKey.StartsWith(AppSettingFinder))
                {
                    var connectionPart = appSettingKey.Remove(0, AppSettingFinder.Length);
                    var connection = connections[connectionPart];
                    if (connection != null)
                    {
                        branchCons.AddRange(GetBranchConnections(appSettingKey, connection,ConnectionType.Dapper));
                        var efConnection = connections[$"{connectionPart}Entities"];

                        if (efConnection != null)
                        {
                            branchCons.AddRange(GetBranchConnections(appSettingKey, efConnection, ConnectionType.Ef));
                        }
                    }
                }
            }

            return branchCons;
        }

        private static IEnumerable<BranchConnection> GetBranchConnections(string appSettingKey, ConnectionStringSettings connection, ConnectionType type)
        {
            return ConfigurationManager.AppSettings[appSettingKey].Split(';')
                                        .Select(x => new BranchConnection(int.Parse(x), connection.ConnectionString, type));
        }

        public string DefaultConnectionString => ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["DefaultConnectionString"]].ConnectionString;

        public string GetConnectionString(int? branchId, ConnectionType type)
        {
            try
            {
                return branchId.HasValue ? BranchConnections.Single(x => x.BranchId == branchId && x.Type == type).ConnectionString : DefaultConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception($"No Connection string mapped for BranchId {branchId}", ex);
            }
        }

        public IList<string> GetConnectionStrings(ConnectionType type)
        {
            return BranchConnections.Where(x=> x.Type == type).Select(x => x.ConnectionString).Distinct().ToList();
        }
    }
}