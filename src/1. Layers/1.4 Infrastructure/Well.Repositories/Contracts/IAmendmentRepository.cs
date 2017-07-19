namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IAmendmentRepository
    {
        IEnumerable<Amendment> GetAmendments(IEnumerable<int> jobIds);
    }
}
