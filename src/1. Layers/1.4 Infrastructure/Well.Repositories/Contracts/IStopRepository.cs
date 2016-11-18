namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IStopRepository : IRepository<Stop, int>
    {
        IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId);

        void StopCreateOrUpdate(Stop stop);

        Stop GetById(int id);

        void StopAccountCreateOrUpdate(Account account);

        Stop GetByRouteNumberAndDropNumber(string routeHeaderCode, int routeHeaderId, string dropId);

        Stop GetByOrderUpdateDetails(string transportOrderReference);

        Stop GetByJobId(int jobId);

        void DeleteStopById(int id);
    }
}