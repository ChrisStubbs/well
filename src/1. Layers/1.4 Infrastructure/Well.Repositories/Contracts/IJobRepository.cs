namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface IJobRepository : IRepository<Job, int>
    {
        Job GetById(int id);

        IEnumerable<Job> GetByIds(IEnumerable<int> jobIds);

        IEnumerable<Job> GetByRouteHeaderId(int routeHeaderId);

        IEnumerable<JobStop> GetJobStopsByRouteHeaderId(int routeHeaderId);

        Job GetJobByRefDetails(string jobTypeCode, string phAccount, string pickListRef, int stopId);

        IEnumerable<Job> GetByStopId(int id);

        void DeleteJobById(int id);

        IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId);

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

        IEnumerable<Job> GetJobsByLineItemIds(IEnumerable<int> lineItemIds);

        IEnumerable<int> GetJobsWithLineItemActions(IEnumerable<int> jobIds);

        IEnumerable<JobToBeApproved> GetJobsToBeApproved();

        Dictionary<int, string> GetPrimaryAccountNumberByRouteHeaderId(int routeHeaderId);

        IEnumerable<int> GetExistingJobsIdsIncludingSoftDeleted(int branchId, IEnumerable<Job> jobs);

        void CascadeSoftDeleteJobs(IList<int> jobIds, bool deletedByImport = false);

        void ReinstateJobsSoftDeletedByImport(IList<int> jobIds);

        void JobsSetResolutionStatusClosed(IList<int> jobIds);

        /// <summary>
        /// Update job WellStatus property
        /// </summary>
        /// <param name="job"></param>
        void UpdateWellStatus(Job job);

        /// <summary>
        /// This method returns job including only fields required for WellStatus calculation
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        Job GetForWellStatusCalculationById(int jobId);
    }
}