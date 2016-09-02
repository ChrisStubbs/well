namespace PH.Well.Services.Contracts
{
    using Domain;

    public interface IDeliveryService
    {
        void UpdateDeliveryLine(JobDetail jobDetail, string username);
    }
}
