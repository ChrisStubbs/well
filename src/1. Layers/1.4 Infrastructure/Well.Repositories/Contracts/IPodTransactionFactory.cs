namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IPodTransactionFactory
    {
        PodTransaction Build(Job job, int branchId);
        IEnumerable<PodDeliveryLineCredit> GetPodDeliveryLineCredits(int jobId, int jobStatus);
    }
}
