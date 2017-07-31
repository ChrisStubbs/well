namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface ICreditThresholdRepository : IRepository<CreditThreshold, int>
    {
        IEnumerable<CreditThreshold> GetAll();

        void Delete(int id);

        CreditThreshold GetById(int thresholdId);

        void PendingCreditInsert(int jobId);
    }
}
