namespace PH.Well.Repositories
{
    using System;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class LineItemActionCommentRepository : DapperRepository<LineItemActionComment, int>, ILineItemActionCommentRepository
    {
        public LineItemActionCommentRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }
        
        // add
        protected override void SaveNew(LineItemActionComment entity)
        {
            entity.Id = dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionCommentInsert)
                .AddParameter("LineItemActionId", entity.LineItemActionId, DbType.Int32)
                .AddParameter("CommentReasonId", entity.CommentReasonId, DbType.Int32)
                .AddParameter("FromQty", entity.FromQty, DbType.Int32)
                .AddParameter("ToQty", entity.ToQty, DbType.Int32)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }


        protected override void UpdateExisting(LineItemActionComment entity)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.LineItemActionCommentUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("LineItemActionId", entity.LineItemActionId, DbType.Int32)
                .AddParameter("CommentReasonId", entity.CommentReasonId, DbType.Int32)
                .AddParameter("FromQty", entity.FromQty, DbType.Int32)
                .AddParameter("ToQty", entity.ToQty, DbType.Int32)
                .AddParameter("UpdatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .AddParameter("DateDeleted", entity.DateDeleted, DbType.DateTime)
                .Execute();
        }
    }
}
