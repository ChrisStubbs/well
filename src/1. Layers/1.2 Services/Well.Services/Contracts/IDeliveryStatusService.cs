using PH.Well.Domain;

namespace PH.Well.Services.Contracts
{
    public interface IDeliveryStatusService
    {
        void SetStatus(Job job, int branchId);
    }
}
