

namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;
  
    public class RouteHeaderRepository : DapperRepository<RouteHeader, int> , IRouteHeaderRepository
    {
        public RouteHeaderRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy) { }

        public RouteException GetCleanDeliveries()
        {
            return dapperProxy.Query<RouteException>(StoredProcedures.RouteHeaderGetCleanDeliveries).FirstOrDefault();
        }

        public RouteException GetExceptions()
        {     
            return dapperProxy.Query<RouteException>(StoredProcedures.RouteHeaderGetExceptions, parameters: null).FirstOrDefault();
        }


    }
}
