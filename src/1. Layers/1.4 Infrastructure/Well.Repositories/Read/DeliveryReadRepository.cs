namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
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

        public IEnumerable<Delivery> GetByStatus(string username, JobStatus jobStatus)
        {
            return GetByStatuses(username, new List<JobStatus>() {jobStatus});
        }

        public IEnumerable<Delivery> GetByStatuses(string username, IList<JobStatus> jobStatuses)
        {
            var deliveries = new List<Delivery>();

            var statuses = jobStatuses.Select(j => (int) j).ToList();
            dapperReadProxy.WithStoredProcedure(StoredProcedures.DeliveriesGet)
                .AddParameter("username", username, DbType.String)
                .AddParameter("JobStatuses", statuses.ToIntDataTables("JobStatuses"), DbType.Object)
                .QueryMultiple(x => deliveries = GetDeliveriesFromGrid(x));

            return deliveries;
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

        public List<Delivery> GetDeliveriesFromGrid(SqlMapper.GridReader grid)
        {
            var deliveries = grid.Read<Delivery>().ToList();
            var deliveryLines = grid.Read<DeliveryLine>().ToList();
            var damages = grid.Read<Damage>().ToList();

            foreach (var line in deliveryLines)
            {
                line.Damages = damages.Where(d => d.JobDetailId == line.JobDetailId).ToList();
            }

            foreach (var delivery in deliveries)
            {
                delivery.Lines = deliveryLines.Where(l => l.JobId == delivery.Id).ToList();
            }

            return deliveries;
        }
    }
}
