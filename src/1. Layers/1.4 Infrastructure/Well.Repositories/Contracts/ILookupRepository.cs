using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Repositories.Contracts
{
    public interface ILookupRepository
    {
        IList<KeyValuePair<string, string>> ExceptionTypes();
        IList<KeyValuePair<string, string>> ExceptionActions();
    }
}
