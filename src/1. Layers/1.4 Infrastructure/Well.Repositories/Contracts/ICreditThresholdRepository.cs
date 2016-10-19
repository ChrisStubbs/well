namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;

    public interface ICreditThresholdRepository : IRepository<CreditThreshold, int>
    {
        IEnumerable<CreditThreshold> GetAll();

        void Delete(int id);

        IEnumerable<CreditThreshold> GetByBranch(int branchId);

        void AssignPendingCreditToUser(User user, CreditEvent creditEvent, string originator);
    }
}