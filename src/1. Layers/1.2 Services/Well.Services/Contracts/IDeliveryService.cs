namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IDeliveryService
    {
        IList<Delivery> GetExceptions(string username);
        IList<Delivery> GetApprovals(string username);
        void UpdateDeliveryLine(JobDetail jobDetail, string username);
        void SaveGrn(int jobId, string grn, int branchId, string username);
    }
}
