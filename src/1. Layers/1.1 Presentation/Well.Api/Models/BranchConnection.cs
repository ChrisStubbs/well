using System;
using System.Collections.Generic;

namespace PH.Well.Api.Models
{
    public class BranchConnection
    {
        public BranchConnection(List<int> branchIds, string dapperConnection, string efConnectionStrings)
        {
            BranchIds = branchIds;
            DapperConnectionString = dapperConnection;
            EfConnectionString = efConnectionStrings;
        }

        public string Get(ConnectionType type)
        {
            if (type == ConnectionType.Dapper)
            {
                return this.DapperConnectionString;
            }

            return this.EfConnectionString;
        }

        public List<int> BranchIds { get; private set; }
        
        public string DapperConnectionString { get; private set; }

        public string EfConnectionString { get; private set; }

    }
}