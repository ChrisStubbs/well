using System.Collections.Generic;

namespace PH.Well.Api.Infrastructure
{
    using Models;

    public interface IConnectionStringFactory
    {
        string GetConnectionString(int? branchId, ConnectionType type);

        IList<string> GetConnectionStrings(ConnectionType type);

        string DefaultConnectionString(ConnectionType type);

        IList<BranchConnection> BranchConnections { get; }
    }
}