using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Repositories.Read
{
    public class LookupRepository : ILookupRepository
    {
        private readonly IDapperReadProxy dapperReadProxy;

        public LookupRepository(IDapperReadProxy dapperReadProxy)
        {
            this.dapperReadProxy = dapperReadProxy;
        }

        public IList<KeyValuePair<string, string>> ExceptionActions()
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.ExceptionAction)
                .Query<KeyValuePair<string, string>>()
                .ToList();
        }

        public IList<KeyValuePair<string, string>> ExceptionTypes()
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.ExceptionType)
                .Query<KeyValuePair<string, string>>()
                .ToList();
        }

        public IList<KeyValuePair<string, string>> JobStatus()
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.JobStatus)
                .Query<KeyValuePair<string, string>>()
                .ToList();
        }

        public IList<KeyValuePair<string, string>> JobType()
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.JobType)
                .Query<KeyValuePair<string, string>>()
                .ToList();
        }

        public IList<KeyValuePair<string, string>> Driver()
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.Driver)
                .Query<KeyValuePair<string, string>>()
                .ToList();
        }

        public IList<KeyValuePair<string, string>> CommentReason()
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.CommentReason)
                .Query<CommentReason>()
                .Where(x => !x.IsDefault)
                .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Description))
                .ToList();
        }
    }
}
