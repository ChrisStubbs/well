using System;

namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using System.Data;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface IJobRepository : IRepository<Job, int>
    {
        Job GetById(int id);

        IEnumerable<Job> GetByIds(IEnumerable<int> jobIds);

        IEnumerable<Job> GetByRouteHeaderId(int routeHeaderId);
       
        Job GetJobByRefDetails(string jobTypeCode,string phAccount, string pickListRef, int stopId);

        IEnumerable<CustomerRoyaltyException> GetCustomerRoyaltyExceptions();

        IEnumerable<Job> GetByStopId(int id);

        void DeleteJobById(int id);

        IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId);

        void AddCustomerRoyaltyException(CustomerRoyaltyException royaltyException);

        void UpdateCustomerRoyaltyException(CustomerRoyaltyException royaltyException);

        CustomerRoyaltyException GetCustomerRoyaltyExceptionsByRoyalty(int royalty);

        void SaveGrn(int jobId, string grn);

        void SetJobToSubmittedStatus(int jobId);

        IEnumerable<Job> GetJobsByBranchAndInvoiceNumber(int jobId, int branchId, string invoiceNumber);

        void UpdateStatus(int jobId, JobStatus status);

        void SetJobResolutionStatus(int jobId, string status);

        IEnumerable<JobRoute> GetJobsRoute(IEnumerable<int> jobIds);

        JobRoute GetJobRoute(int jobId);

        void SaveJobResolutionStatus(Job job);

        IEnumerable<JobDetailLineItemTotals> JobDetailTotalsPerStop(int stopId);

        IEnumerable<JobDetailLineItemTotals> JobDetailTotalsPerRouteHeader(int routeHeaderId);

        IEnumerable<JobDetailLineItemTotals> JobDetailTotalsPerJobs(IEnumerable<int> jobIds);

        IEnumerable<Job> GetJobsByResolutionStatus(ResolutionStatus resolutionStatus);

    }
}