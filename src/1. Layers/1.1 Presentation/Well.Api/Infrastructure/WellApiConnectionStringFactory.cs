namespace PH.Well.Api.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Models;
    using Newtonsoft.Json;

    public class WellApiConnectionStringFactory : IConnectionStringFactory
    {
        private const string AppSettingFinder = "ConnectionStringGroups";
        private IList<BranchConnection> branchConnections;

        public IList<BranchConnection> BranchConnections => branchConnections ?? (branchConnections = GetBranchConnections());

        private List<BranchConnection> GetBranchConnections()
        {
            var collection = ConfigurationManager.AppSettings;
            var connections = ConfigurationManager.ConnectionStrings;
            
            return JsonConvert.DeserializeObject<Common.BranchGroups>(collection.Cast<string>()
                .Select(key => new KeyValuePair<string, string>(key, collection[key]))
                .First(p => p.Key == AppSettingFinder).Value).Groups
                .Select(p => new BranchConnection(
                        p.BranchIds,
                        connections[p.GroupName].ConnectionString,
                        connections[$"{p.GroupName}Entities"].ConnectionString))
                .ToList();
        }

        public string DefaultConnectionString(ConnectionType type)
        {
            var key = ConfigurationManager.AppSettings["DefaultConnectionString"];
            if (type == ConnectionType.Dapper)
            {
                return ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }

            return ConfigurationManager.ConnectionStrings[$"{key}Entities"].ConnectionString;
        }

        public string GetConnectionString(int? branchId, ConnectionType type)
        {
            if (branchId.HasValue)
            {
                return this.BranchConnections
                    .Single(p => p.BranchIds.Any(c => c == branchId.Value)).Get(type);
            }

            return DefaultConnectionString(type);
        }

        public IList<string> GetConnectionStrings(ConnectionType type)
        {
            return BranchConnections
                .Select(p => p.Get(type))
                .ToList();
        }
    }
}