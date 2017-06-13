namespace PH.Well.Repositories
{
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
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }
    }
}
