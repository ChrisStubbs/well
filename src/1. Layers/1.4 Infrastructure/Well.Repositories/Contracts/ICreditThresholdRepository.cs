namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface ICreditThresholdRepository : IRepository<CreditThreshold, int>
    {
        IEnumerable<CreditThreshold> GetAll();

        void Delete(int id);

        CreditThreshold GetById(int thresholdId, string connectionString);

        void PendingCreditInsert(int jobId);

        CreditThreshold GetByUserId(int userId);

        void SetForUser(int userId, int creditThresholdId, string connectionString);
    }
}
