using System;

namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IStopRepository : IRepository<Stop, int>
    {
        IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId);

        IList<Stop> GetByTransportOrderReferences(IList<string> transportOrderReferences);

        IList<Stop> GetByIds(IEnumerable<int> stopIds);

        Stop GetById(int id);

        Stop GetByJobDetails(string picklist, string account);

        Stop GetByJobId(int jobId);

        void DeleteStopById(int id);

        void DeleteStopByTransportOrderReference(string transportOrderReference);

    }
}