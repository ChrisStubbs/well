namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public interface IBulkCreditService
    {
        IEnumerable<string> BulkCredit(IEnumerable<Job> jobs, JobDetailReason reason, JobDetailSource source);
    }
}