namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using PH.Well.Domain.ValueObjects;

    public interface IJobDetailToDeliveryLineCreditMapper
    {
        List<DeliveryLineCredit> Map(IEnumerable<JobDetail> creditLines);
    }
}