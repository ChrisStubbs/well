namespace PH.Well.Services.Contracts
{
    using Domain;

    public interface IDeliveryService
    {
        void UpdateDeliveryLine(JobDetail jobDetail, string username);

        void UpdateDraftActions(JobDetail jobDetailUpdates, string username);

        void SubmitActions(int jobId, string username);

        void SaveGrn(int jobId, string grn, int branchId, string username);
    }
}
