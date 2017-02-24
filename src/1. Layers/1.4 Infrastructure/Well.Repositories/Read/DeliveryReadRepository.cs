namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Dapper;
    using Domain;
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

        public IEnumerable<Delivery> GetCleanDeliveries(string username)
        {
            return
                this.dapperReadProxy.WithStoredProcedure(StoredProcedures.GetCleanDeliveries)
                    .AddParameter("username", username, DbType.String)
                    .Query<Delivery>();
        }

        public IEnumerable<Delivery> GetResolvedDeliveries(string username)
        {
            return
                this.dapperReadProxy.WithStoredProcedure(StoredProcedures.GetResolvedDeliveries)
                    .AddParameter("username", username, DbType.String)
                    .Query<Delivery>();
        }

        //TODO - Refactor into a single sproc with parameters to filter those pending credit
        public IEnumerable<Delivery> GetByPendingCredit(string username)
        {
            return
                this.dapperReadProxy.WithStoredProcedure(StoredProcedures.DeliveriesGetByPendingCredit)
                    .AddParameter("UserName", username, DbType.String)
                    .Query<Delivery>();
        }

        public IEnumerable<Delivery> GetExceptionDeliveries(string username, bool includePendingCredit = false)
        {
            var exceptionDeliveries = dapperReadProxy.WithStoredProcedure(StoredProcedures.GetExceptionDeliveries)
                .AddParameter("UserName", username, DbType.String)
                .Query<Delivery>().ToList();

            var deliveriesPendingCredit = GetByPendingCredit(username);

            if (includePendingCredit == false)
            {
                exceptionDeliveries.RemoveAll(d => deliveriesPendingCredit.Any(c => c.Id == d.Id));
            }

            return exceptionDeliveries;
        }

        public DeliveryDetail GetDeliveryById(int id, string username)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.DeliveryGetById)
                .AddParameter("Id", id, DbType.Int32)
                .AddParameter("UserName", username, DbType.String)
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
                line.Damages = damages.Where(d => d.JobDetailId == line.JobDetailId).ToList();
            }

            return deliveryLines;
        }

        public IEnumerable<PendingCreditDetail> GetPendingCreditDetail(int jobId)
        {
            return
                this.dapperReadProxy.WithStoredProcedure(StoredProcedures.JobDetailActionsGet)
                    .AddParameter("jobId", jobId, DbType.Int32)
                    .Query<PendingCreditDetail>();
        }
    }
}
