namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAmendmentService
    {
        void ProcessAmendments(IEnumerable<int> jobIds);
        
    }
}
