namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public interface IBulkCreditService
    {
        void BulkCredit(IEnumerable<Job> jobs, JobDetailReason reason, JobDetailSource source);
    }
}