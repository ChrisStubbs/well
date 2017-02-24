namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Dapper;

    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class RouteToRemoveRepository : IRouteToRemoveRepository
    {
        private readonly IDapperProxy dapperProxy;

        public RouteToRemoveRepository(IDapperProxy dapperProxy)
        {
            this.dapperProxy = dapperProxy;
        }

        public IEnumerable<int> GetRouteIds()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteIdsToRemoveGet).Query<int>();
        }

        public RouteToRemove GetRouteToRemove(int routeId)
        {
            var route = new RouteToRemove();

            this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteToRemoveFullObjectGraphGet)
                .AddParameter("routeId", routeId, DbType.Int32)
                .QueryMultiple(x => route = GetRouteFromGrid(x));

            route.RouteId = routeId;

            return route;
        }

        private RouteToRemove GetRouteFromGrid(SqlMapper.GridReader grid)
        {
            var route = new RouteToRemove();
            route.RouteHeaders = grid.Read<RouteHeaderToRemove>().ToList();

            var stops = grid.Read<StopToRemove>().ToList();
            var jobs = grid.Read<JobToRemove>().ToList();
            var jobDetails = grid.Read<JobDetailToRemove>().ToList();
            var jobDamages = grid.Read<JobDamageToRemove>().ToList();

            foreach (var routeHeader in route.RouteHeaders)
            {
                routeHeader.Stops.AddRange(stops.Where(x => x.RouteHeaderId == routeHeader.RouteHeaderId));

                foreach (var stop in routeHeader.Stops)
                {
                    stop.Jobs.AddRange(jobs.Where(x => x.StopId == stop.StopId));

                    foreach (var job in stop.Jobs)
                    {
                        job.JobDetails.AddRange(jobDetails.Where(x => x.JobId == job.JobId));

                        foreach (var jobDetail in job.JobDetails)
                        {
                            jobDetail.JobDamages.AddRange(jobDamages.Where(x => x.JobDetailId == jobDetail.JobDetailId));
                        }
                    }
                }
            }

            return route;
        }
    }
}