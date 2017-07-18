namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    public interface IAmendmentService
    {
        void ProcessAmendments(IEnumerable<int> jobIds);
    }
}
