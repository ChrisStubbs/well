namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IDeliveryService
    {
        void UpdateDeliveryLine(JobDetail jobDetail, string username);
        void UpdateDraftActions(JobDetail jobDetailUpdates, string username);
        void SubmitActions(int jobId, string username);
        void CreditLines(IEnumerable<int> creditLines);
    }
}
