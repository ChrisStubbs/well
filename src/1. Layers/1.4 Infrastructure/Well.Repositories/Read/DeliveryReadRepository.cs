using System.Linq;

namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Contracts;
    using Dapper;
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
            var exceptionStatuses = ExceptionStatuses.Statuses;

            var allExceptions = new List<Delivery>();

            foreach (var exceptionStatus in exceptionStatuses)
            {
                allExceptions.AddRange(GetDeliveriesByStatus(exceptionStatus, userName));
            }

            return allExceptions;
        }

        public DeliveryDetail GetDeliveryById(int id)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.DeliveryGetById)
                .AddParameter("Id", id, DbType.Int32)
                .Query<DeliveryDetail>()
                .FirstOrDefault();
        }

        public IEnumerable<DeliveryLine> GetDeliveryLinesByJobId(int jobId)
        {
            var deliveryLines = new List<DeliveryLine>();
            dapperReadProxy
                .WithStoredProcedure(StoredProcedures.DeliveryLinesGetByJobId)
                .AddParameter("JobId", jobId, DbType.Int32)
                .QueryMultiple(x => deliveryLines = GetLinesFromGrid(x));

            return deliveryLines;
        }

        public List<DeliveryLine> GetLinesFromGrid(SqlMapper.GridReader grid)
        {
            var deliveryLines = grid.Read<DeliveryLine>().ToList();
            var damages = grid.Read<Damage>().ToList();

            foreach (var line in deliveryLines)
            {
                line.Damages = damages.Where(d => d.JobDetailId == line.Id).ToList();
            }

            return deliveryLines;
        }
    }
}
