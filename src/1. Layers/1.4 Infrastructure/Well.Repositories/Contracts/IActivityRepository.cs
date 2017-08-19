using System;

namespace PH.Well.Repositories.Contracts
{
    using Domain.ValueObjects;

    public interface IActivityRepository
    {
        [Obsolete("Use GetActivitySourceById instead",true)]
        ActivitySource GetActivitySourceByDocumentNumber(string documentNumber, int branchId);
        
        ActivitySource GetActivitySourceById(int id);
    }
}
