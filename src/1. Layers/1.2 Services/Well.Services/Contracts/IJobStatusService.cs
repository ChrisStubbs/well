using PH.Well.Domain;

namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    public interface IJobService
    {
        void SetInitialJobStatus(Job job);
        void SetIncompleteJobStatus(Job job);
        Job DetermineStatus(Job job, int branchId);
        bool CanEditActions(Job job, string userName);
        IEnumerable<Job> PopulateLineItemsAndRoute(IEnumerable<Job> jobs);
        Job PopulateLineItemsAndRoute(Job job);
    }
}
