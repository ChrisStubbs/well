namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface ICreditThresholdRepository : IRepository<CreditThreshold, int>
    {
        IEnumerable<CreditThreshold> GetAll();

        void Delete(int id);

        IEnumerable<CreditThreshold> GetByBranch(int branchId);

        void AssignPendingCreditToUser(User user, int jobId, string originator);
    }
}