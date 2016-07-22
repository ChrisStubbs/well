namespace PH.Well.Repositories.Read
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

        public IEnumerable<Delivery> GetCleanDeliveries()
        {
            return GetDeliveriesByStatus(PerformanceStatus.Compl);
        }

        private IEnumerable<Delivery> GetDeliveriesByStatus(PerformanceStatus status)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.DeliveriesGetByPerformanceStatus)
                .AddParameter("PerformanceStatusId", status, DbType.Int32)
                .Query<Delivery>();
        }

        public IEnumerable<Delivery> GetResolvedDeliveries()
        {
            return GetDeliveriesByStatus(PerformanceStatus.Resolved);
        }

        public IEnumerable<Delivery> GetExceptionDeliveries()
        {
            var incompletes = GetDeliveriesByStatus(PerformanceStatus.Incom);
            var authorisedBypassed = GetDeliveriesByStatus(PerformanceStatus.Abypa);
            var nonAuthorisedBypassed = GetDeliveriesByStatus(PerformanceStatus.Nbypa);

            var allExceptions = new List<Delivery>();

            allExceptions.AddRange(incompletes);
            allExceptions.AddRange(authorisedBypassed);
            allExceptions.AddRange(nonAuthorisedBypassed); 

            return allExceptions;
        }

    }
}
