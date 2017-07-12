using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;

namespace PH.Well.Repositories.Contracts
{
    public interface ICommentReasonRepository
    {
        IList<CommentReason> GetAll();
    }
}
