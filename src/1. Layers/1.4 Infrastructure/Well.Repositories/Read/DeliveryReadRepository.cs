﻿namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class DeliveryReadRepository : IDeliveryReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public DeliveryReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }

        public IEnumerable<Delivery> GetCleanDeliveries(string userName)
        {
            return GetDeliveriesByStatus(PerformanceStatus.Compl, userName);
        }

        private IEnumerable<Delivery> GetDeliveriesByStatus(PerformanceStatus status, string userName)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.DeliveriesGetByPerformanceStatus)
                .AddParameter("PerformanceStatusId", status, DbType.Int32)
                .AddParameter("UserName", userName, DbType.String)
                .Query<Delivery>();
        }

        public IEnumerable<Delivery> GetResolvedDeliveries(string userName)
        {
            return GetDeliveriesByStatus(PerformanceStatus.Resolved, userName);
        }

        public IEnumerable<Delivery> GetExceptionDeliveries(string userName)
        {
            var incompletes = GetDeliveriesByStatus(PerformanceStatus.Incom, userName);
            var authorisedBypassed = GetDeliveriesByStatus(PerformanceStatus.Abypa, userName);
            var nonAuthorisedBypassed = GetDeliveriesByStatus(PerformanceStatus.Nbypa, userName);

            var allExceptions = new List<Delivery>();

            allExceptions.AddRange(incompletes);
            allExceptions.AddRange(authorisedBypassed);
            allExceptions.AddRange(nonAuthorisedBypassed); 

            return allExceptions;
        }

    }
}