namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;

    public class LocationRepository : ILocationRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public LocationRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }

        // gets the activities, lineitem & lineitem actions for single location
        public Location GetLocationById(int locationId)
        {
            var location = new Location();

            dapperReadProxy.WithStoredProcedure(StoredProcedures.LocationGetById)
                  .AddParameter("locationId", locationId, DbType.Int32)
                  .QueryMultiple(x => location = GetLocationFromGrid(x));

            return location;

        }

        public Location GetLocationFromGrid(SqlMapper.GridReader grid)
        {
            var location = grid.Read<Location>().FirstOrDefault();
            var activities = grid.Read<Activity>().ToList();
            var lineItems = grid.Read<LineItem>().ToList();
            var lineItemActions = grid.Read<LineItemAction>().ToList();

            if (location != null)
            { 
                location.Activities = activities.Where(x => x.LocationId == location.Id).ToList();

                foreach (var activity in activities)
                {
                    activity.LineItems = lineItems.Where(x => x.ActivityId == activity.Id).ToList();

                    foreach (var item in lineItems)
                    {
                        item.LineItemActions = lineItemActions.Where(x => x.LineItemId == item.Id).ToList();
                    }
                }
            }

            return location;
        }
    }
}
