using System.Collections.Generic;
using PH.Well.Domain.Enums;

namespace PH.Well.Services.Contracts
{
    public interface ILookupService
    {
        IList<KeyValuePair<string, string>> GetLookup(LookupType lookupType);
    }
}
