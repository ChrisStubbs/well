namespace PH.Well.Repositories
{
    using System.Linq;
    using Contracts;
    using Domain;

    public class LineItemActionRepository
    {
        private readonly IDapperProxy dapperProxy;

        public LineItemActionRepository(IDapperProxy dapperProxy)
        {
            this.dapperProxy = dapperProxy;
        }

        // add
        // update

        //protected override void SaveNew(LineItemAction entity)
        //{
        //    entity.Id = dapperProxy.WithStoredProcedure("LineItemAction_AddByUser")
        //        .AddParameter()
        //        .Query<int>().FirstOrDefault();
        //}


        /* protected override void SaveNew(JobDetailAction entity)
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
        }*/

    }
}
