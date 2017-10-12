using System.Collections.Generic;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.Repositories.Read
{
    public interface IAppSearchReadRepository
    {
        IEnumerable<AppSearchItem> Search(AppSearchParameters searchParameters);
    }
}