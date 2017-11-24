namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.Enums;
    using PH.Well.Domain;

    public interface ICreditThresholdRepository : IRepository<CreditThreshold, int>
    {
        IEnumerable<CreditThreshold> GetAll();

        void Delete(int id, string connectionString);

        CreditThreshold GetById(int thresholdId, string connectionString);

        void PendingCreditInsert(int jobId);

        CreditThreshold GetByUserId(int userId);

        void SetForUser(int userId, int creditThresholdId, string connectionString);

        CreditThreshold GetByLevel(ThresholdLevel level, string connectionString);

        void Save(CreditThreshold creditThreshold, string connectionString);

        void Update(CreditThreshold creditThreshold, string connectionString);
    }
}
