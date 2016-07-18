﻿namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Attribute = Domain.Attribute;

    public interface IRouteHeaderRepository: IRepository<RouteHeader, int>
    {
        IEnumerable<RouteHeader> GetRouteHeaders();

        Routes CreateOrUpdate(Routes routes);

        Routes GetById(int id);

        Routes GetByFilename(string filename);

        RouteHeader RouteHeaderCreateOrUpdate(RouteHeader routeHeader);

        RouteHeader GetRouteHeaderById(int id);

        void AddRouteHeaderAttributes(Attribute attribute);

        RouteHeader GetRouteHeaderByRouteNumberAndDate(string routeNumber, DateTime routeDate);
    }
}
