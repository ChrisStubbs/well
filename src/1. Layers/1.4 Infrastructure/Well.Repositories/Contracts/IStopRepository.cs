﻿namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public  interface IStopRepository : IRepository<Stop, int>
    {
        IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId);

        Stop StopCreateOrUpdate(Stop stop);

        Stop GetById(int id);

        void StopAccountCreateOrUpdate(Account account);

        Stop GetByRouteNumberAndDropNumber(string routeHeaderCode, int routeHeaderId, string dropId);

        Stop GetByOrderUpdateDetails(string routeHeaderCode, string dropId, string locationId, DateTime deliveryDate);

        Stop GetByJobId(int jobId);
        void DeleteStopById(int id, WellDeleteType deleteType);
    }
}