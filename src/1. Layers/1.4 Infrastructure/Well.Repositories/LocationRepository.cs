namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class LocationRepository : ILocationRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public IEnumerable<Location> GetAllLocations()
        {
            var locations = new List<Location>();
         //   dapperReadProxy.WithStoredProcedure(StoredProcedures.LocationGetAll)
         //           .QueryMultiple(x => routes = GetLocationsFromGrid(x));

            return locations;
        }

       /*
        
         public List<Route> GetReadRoutesFromGrid(SqlMapper.GridReader grid)
        {
            var readRoutes = grid.Read<Route>().ToList();
            var assignees = grid.Read<Assignee>().ToList();
            var jobIds = grid.Read<RouteJob>().ToList();

            foreach (var route in readRoutes)
            {
                route.Assignees = assignees.Where(x => x.RouteId == route.Id).ToList();
                route.JobIds = jobIds.Where(x => x.RouteId == route.Id).Select(x => x.JobId).ToList();
            }

            return readRoutes;
        }
        
        
        */




    }
}
