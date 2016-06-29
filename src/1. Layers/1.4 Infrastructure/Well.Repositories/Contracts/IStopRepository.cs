namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public  interface IStopRepository : IRepository<Stop, int>
    {
        IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId);
    }
}