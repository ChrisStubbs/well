using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            throw new NotImplementedException();
        }

        public IList<KeyValuePair<string, string>> ExceptionTypes()
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.ExceptionType)
                .Query<KeyValuePair<string, string>>()
                .ToList();
        }
    }
}
