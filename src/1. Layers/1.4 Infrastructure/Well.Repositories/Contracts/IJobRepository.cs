namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Data;
    using Domain;
    using Domain.ValueObjects;

    public interface IJobRepository : IRepository<Job, int>
    {
        Job GetById(int id);

        Job GetByAccountPicklistAndStopId(string accountId, string picklistId, int stopId);

        Job GetJobByRefDetails(string phAccount, string pickListRef, int stopId);

        IEnumerable<CustomerRoyaltyException> GetCustomerRoyaltyExceptions();

        IEnumerable<Job> GetByStopId(int id);

        void DeleteJobById(int id);

        IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId);

        void AddCustomerRoyaltyException(CustomerRoyaltyException royaltyException);

        void UpdateCustomerRoyaltyException(CustomerRoyaltyException royaltyException);

        CustomerRoyaltyException GetCustomerRoyaltyExceptionsByRoyalty(int royalty);

        void ResolveJobAndJobDetails(int jobId);

        void SaveGrn(int jobId, string grn);

        void SetJobToSubmittedStatus(int jobId);

        IEnumerable<Job> GetJobsByBranchAndInvoiceNumber(int branchId, string invoiceNumber);

    }
}