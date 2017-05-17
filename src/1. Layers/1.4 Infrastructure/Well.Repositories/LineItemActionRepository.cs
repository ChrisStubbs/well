namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;

    public class LineItemActionRepository : DapperRepository<LineItemAction, int>, ILineItemActionRepository
    {

        public LineItemActionRepository(IDapperProxy dapperProxy, ILogger logger, IUserNameProvider userNameProvider)
          :  base(logger, dapperProxy, userNameProvider)
        {
        }

        // add
        protected override void SaveNew(LineItemAction entity)
        {
            var exceptionTypeId = EnumExtensions.GetValueFromDescription<ExceptionType>(entity.ExceptionType);
            var sourceId = EnumExtensions.GetValueFromDescription<JobDetailSource>(entity.Source);
            var reasonId = EnumExtensions.GetValueFromDescription<JobDetailReason>(entity.Reason);

            entity.Id = dapperProxy.WithStoredProcedure("LineItemAction_AddByUser")
                .AddParameter("ExceptionTypeId", exceptionTypeId, DbType.Int32)
                .AddParameter("Quantity", entity.Quantity, DbType.Int32)
                .AddParameter("SourceId", sourceId, DbType.Int32)
                .AddParameter("ReasonId", reasonId, DbType.Int32)
                .AddParameter("ReplanDate", entity.ReplanDate, DbType.DateTime)
                .AddParameter("SubmittedDate", entity.SubmittedDate, DbType.DateTime)
                .AddParameter("ApprovalDate", entity.ApprovalDate, DbType.DateTime)
                .AddParameter("ApprovedBy", entity.ApprovedBy, DbType.String)
                .AddParameter("LineItemId", entity.LineItemId, DbType.Int32)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                 .AddParameter("LastUpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("LastUpdatedDate", entity.DateUpdated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        // update
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
