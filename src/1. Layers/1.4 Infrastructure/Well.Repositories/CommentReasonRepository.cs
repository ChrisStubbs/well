using System.Collections.Generic;
using System.Linq;
using PH.Well.Common.Contracts;
using PH.Well.Domain;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Repositories
{
    public class CommentReasonRepository : DapperRepository<CommentReason,int>, ICommentReasonRepository
    {
        public CommentReasonRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider) : base(logger, dapperProxy, userNameProvider)
        {
        }
        
        public IList<CommentReason> GetAll()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.CommentReason)
                .Query<CommentReason>().ToList();
        }
    }
}
