namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IApprovalService
    {
        IList<JobToBeApproved> GetJobsToBeApproved(int branchId);
    }
}