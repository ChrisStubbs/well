namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class LineItemActionRepository : DapperRepository<LineItemAction, int>, ILineItemActionRepository
    {

        public LineItemActionRepository(IDapperProxy dapperProxy, ILogger logger, IUserNameProvider userNameProvider)
          : base(logger, dapperProxy, userNameProvider)
        {
        }

        // add
        protected override void SaveNew(LineItemAction entity)
        {
            entity.Id = dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionInsertByUser)
                .AddParameter("ExceptionTypeId", entity.ExceptionType, DbType.Int32)
                .AddParameter("Quantity", entity.Quantity, DbType.Int32)
                .AddParameter("SourceId", entity.Source, DbType.Int32)
                .AddParameter("ReasonId", entity.Reason, DbType.Int32)
                .AddParameter("ReplanDate", entity.ReplanDate, DbType.DateTime)
                .AddParameter("SubmittedDate", entity.SubmittedDate, DbType.DateTime)
                .AddParameter("ApprovalDate", entity.ApprovalDate, DbType.DateTime)
                .AddParameter("ApprovedBy", entity.ApprovedBy, DbType.String)
                .AddParameter("LineItemId", entity.LineItemId, DbType.Int32)
                .AddParameter("Originator", entity.Originator, DbType.String)
                .AddParameter("ActionedBy", entity.ActionedBy, DbType.String)
                .AddParameter("DeliveryActionId", entity.DeliveryAction, DbType.Int32)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        public LineItemAction GetById(int id)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionGet)
                .AddParameter("Id", id, DbType.Int32)
                .Query<LineItemAction>().SingleOrDefault();
        }

        protected override void UpdateExisting(LineItemAction entity)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionUpdate)
                .AddParameter("Id", entity.ExceptionType, DbType.Int32)
                .AddParameter("ExceptionTypeId", entity.ExceptionType, DbType.Int32)
                .AddParameter("Quantity", entity.Quantity, DbType.Int32)
                .AddParameter("SourceId", entity.Source, DbType.Int32)
                .AddParameter("ReasonId", entity.Reason, DbType.Int32)
                .AddParameter("ReplanDate", entity.ReplanDate, DbType.DateTime)
                .AddParameter("SubmittedDate", entity.SubmittedDate, DbType.DateTime)
                .AddParameter("ApprovalDate", entity.ApprovalDate, DbType.DateTime)
                .AddParameter("ApprovedBy", entity.ApprovedBy, DbType.String)
                .AddParameter("LineItemId", entity.LineItemId, DbType.Int32)
                .AddParameter("Originator", entity.Originator, DbType.String)
                .AddParameter("ActionedBy", entity.ActionedBy, DbType.String)
                .AddParameter("DeliveryActionId", entity.DeliveryAction, DbType.Int32)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .Execute();
        }

    }
}
