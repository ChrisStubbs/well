namespace PH.Well.Services
{
    using System.Collections.Generic;
    using Contracts;
    using Domain;
    using Domain.Enums;

    public class BulkCreditService : IBulkCreditService
    {
        public void BulkCredit(IEnumerable<Job> jobs, JobDetailReason reason, JobDetailSource source)
        {
            //1. Mark all lines with credit action

            //2. Any over threshold - set status to Credit Approval

            //3. Add all which can be credited to the event table
        }

    }
}
