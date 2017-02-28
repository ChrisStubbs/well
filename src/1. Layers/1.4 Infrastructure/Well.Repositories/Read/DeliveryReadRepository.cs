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

        public IEnumerable<Delivery> GetByStatus(string username, JobStatus jobStatus)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.DeliveriesGet)
                .AddParameter("username", username, DbType.String)
                .AddParameter("JobStatus", jobStatus, DbType.Int32)
                .Query<Delivery>();
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
    }
}
