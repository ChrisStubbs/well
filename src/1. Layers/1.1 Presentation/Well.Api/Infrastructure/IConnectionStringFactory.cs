using System.Collections.Generic;

namespace PH.Well.Api.Infrastructure
{
    public interface IConnectionStringFactory
    {
        string GetConnectionString(int branchId);
        IList<string> GetConnectionStrings();
    }
}