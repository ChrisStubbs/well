using PH.Well.Domain;
using PH.Well.Domain.ValueObjects;

namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;


    public interface IJobService: IGetJobResolutionStatus
    {
        void SetInitialJobStatus(Job job);
        void SetIncompleteJobStatus(Job job);
        Job DetermineStatus(Job job, int branchId);

        /// <summary>
        /// Compute the Well status of the job and update job
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>Returns true of the status was changed</returns>
        bool ComputeWellStatus(int jobId);

        /// <summary>
        /// Compute the Well status of the job and update job
        /// </summary>
        /// <param name="job"></param>
        /// <returns>Returns true of the status was changed</returns>
        bool ComputeWellStatus(Job job);

        /// <summary>
        /// Compute the WellStatus of the job and update job and its ancestors
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        bool ComputeAndPropagateWellStatus(int jobId);

        /// <summary>
        /// Compute the WellStatus of the job and update job and its ancestors
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        bool ComputeAndPropagateWellStatus(Job job);

        string CanEdit(Job job, string userName);

        bool CanManuallyComplete(Job job, string userName);

        IEnumerable<Job> PopulateLineItemsAndRoute(IEnumerable<Job> jobs);

        void SetGrn(int jobId, string grn);

        Job PopulateLineItemsAndRoute(Job job);

        IEnumerable<Job> GetJobsWithRoute(IEnumerable<int> jobIds);

        IEnumerable<int> GetJobsIdsAssignedToCurrentUser(IEnumerable<int> jobIds);

        AssignJobResult Assign(UserJobs userJobs);

        AssignJobResult UnAssign(IEnumerable<int> jobIds);
    }
}
