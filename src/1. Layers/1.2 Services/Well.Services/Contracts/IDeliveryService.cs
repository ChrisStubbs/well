namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IDeliveryService
    {
        void UpdateDeliveryLine(JobDetail jobDetail, string username);
        void UpdateDraftActions(JobDetail jobDetailUpdates, string username);
        void SubmitActions(int jobId, string username);
        void CreditLines(IEnumerable<CreditLines> creditLines, string username);
    }
}
