namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public  interface IStopRepository : IRepository<Stop, int>
    {
        IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId);

        Stop StopCreateOrUpdate(Stop stop);
        
        void AddStopAttributes(Attribute attribute);

        Stop GetById(int id);

        void StopAccountCreateOrUpdate(Account account);
    }
}