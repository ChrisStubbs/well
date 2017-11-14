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
            catch (Exception ex)
            {
                throw new Exception($"No Connection string mapped for BranchId {branchId}", ex);
            }
        }

        public IList<string> GetConnectionStrings()
        {
            return BranchConnections.Select(x => x.ConnectionString).Distinct().ToList();
        }


    }
}