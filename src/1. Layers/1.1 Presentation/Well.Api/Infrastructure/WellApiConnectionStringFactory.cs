namespace PH.Well.Api.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class WellApiConnectionStringFactory : IConnectionStringFactory
    {
        public class BranchConnection
        {
            public BranchConnection(int branchId, string connectionString)
            {
                BranchId = branchId;
                ConnectionString = connectionString;
            }

            public int BranchId { get; set; }
            public string ConnectionString { get; set; }
        }

        private const string AppSettingFinder = "ConnectionString-";

        private IList<BranchConnection> branchConnections;

        public IList<BranchConnection> BranchConnections => branchConnections ?? (branchConnections = GetBranchConnections());

        public WellApiConnectionStringFactory()
        {
        }

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
                        branchCons.AddRange(
                            ConfigurationManager.AppSettings[appSettingKey].Split(';')
                            .Select(x => new BranchConnection(int.Parse(x), connection.ConnectionString)));
                    }
                }
            }

            return branchCons;
        }

        public string GetConnectionString(int branchId)
        {
            try
            {
                return BranchConnections.Single(x => x.BranchId == branchId).ConnectionString;
            }
            catch (Exception e)
            {

                throw new Exception($"No Connection string mapped for BranchId {branchId}", e);
            }

        }

        public IList<string> GetConnectionStrings()
        {
            return BranchConnections.Select(x => x.ConnectionString).Distinct().ToList();
        }


    }
}