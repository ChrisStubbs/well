namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Attribute = Domain.Attribute;

    public  interface IStopRepository : IRepository<Stop, int>
    {
        IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId);

        Stop StopCreateOrUpdate(Stop stop);
        
        void AddStopAttributes(Attribute attribute);

        Stop GetById(int id);

        void StopAccountCreateOrUpdate(Account account);

        Stop GetByRouteNumberAndDropNumber(string routeHeaderCode, int routeHeaderId, string dropId);
    }
}