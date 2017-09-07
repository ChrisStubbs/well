﻿using PH.Well.Domain;

namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    public interface IJobService: IGetJobResolutionStatus
    {
        void SetInitialJobStatus(Job job);
        void SetIncompleteJobStatus(Job job);
        Job DetermineStatus(Job job, int branchId);

        /// <summary>
        /// Compute the Well status of the job from it's child LineItems and their LineItemActios
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns>Returns true of the status was changed</returns>
        bool ComputeWellStatus(int jobId);
        bool ComputeWellStatus(Job job);

        bool CanEdit(Job job, string userName);
        bool CanManuallyComplete(Job job, string userName);
        IEnumerable<Job> PopulateLineItemsAndRoute(IEnumerable<Job> jobs);
        void SetGrn(int jobId, string grn);
        Job PopulateLineItemsAndRoute(Job job);
        IEnumerable<Job> GetJobsWithRoute(IEnumerable<int> jobIds);
        IEnumerable<int> GetJobsIdsAssignedToCurrentUser(IEnumerable<int> jobIds);
    }
}
