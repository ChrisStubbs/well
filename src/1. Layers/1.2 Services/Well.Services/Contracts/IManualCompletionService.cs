namespace PH.Well.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public interface IManualCompletionService
    {
        void ManuallyCompleteJobs(IEnumerable<int> jobIds, Action<IEnumerable<Job>> actionJobs);
        void MarkAsBypassed(IEnumerable<int> jobIds);
        void MarkAsComplete(IEnumerable<int> jobIds);
    }
}