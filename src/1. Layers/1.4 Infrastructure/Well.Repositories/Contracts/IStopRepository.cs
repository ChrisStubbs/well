using System;

namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IStopRepository : IRepository<Stop, int>
    {
        IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId);

        Stop GetById(int id);

        Stop GetByJobDetails(string picklist, string account, int branchId);

        Stop GetByOrderUpdateDetails(string transportOrderReference);

        Stop GetByJobId(int jobId);

        void DeleteStopById(int id);

        void DeleteStopByTransportOrderReference(string transportOrderReference);
    }
}