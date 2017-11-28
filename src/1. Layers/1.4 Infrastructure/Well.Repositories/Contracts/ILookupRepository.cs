using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;

namespace PH.Well.Repositories.Contracts
{
    public interface ILookupRepository
    {
        IList<KeyValuePair<string, string>> ExceptionTypes();
        IList<KeyValuePair<string, string>> ExceptionActions();
        IList<KeyValuePair<string, string>> JobStatus();
        IList<KeyValuePair<string, string>> JobType();
        IList<KeyValuePair<string, string>> Driver(string connectionString);
        IList<KeyValuePair<string, string>> CommentReason();
    }
}
