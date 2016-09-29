namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;

    public class JobDetailActionRepo : DapperRepository<JobDetailAction, int>, IJobDetailActionRepo
    {
        public JobDetailActionRepo(ILogger logger, IDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public void DeleteDrafts(int jobDetailId)
        {
            dapperProxy.WithStoredProcedure("JobDetailAction_Delete")
                .AddParameter("JobDetailId", jobDetailId, DbType.Int32)
                .AddParameter("StatusId", ActionStatus.Draft, DbType.Int32)
                .Execute();
        }

        protected override void SaveNew(JobDetailAction entity)
        {
            entity.Id = dapperProxy.WithStoredProcedure("JobDetailAction_Insert")
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("Quantity", entity.Quantity, DbType.Int32)
                .AddParameter("ActionId", entity.Action, DbType.Int32)
                .AddParameter("StatusId", entity.Status, DbType.Int32)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting(JobDetailAction entity)
        {
            dapperProxy.WithStoredProcedure("JobDetailAction_Update")
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("Quantity", entity.Quantity, DbType.Int32)
                .AddParameter("ActionId", entity.Action, DbType.Int32)
                .AddParameter("StatusId", entity.Status, DbType.Int32)
                .AddParameter("UpdatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateCreated, DbType.DateTime)
                .Execute();
        }
    }
}
