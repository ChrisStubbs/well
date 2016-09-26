namespace PH.Well.Api.Mapper.Contracts
{
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public interface ICreditThresholdMapper
    {
        CreditThreshold Map(CreditThresholdModel model);

        CreditThresholdModel Map(CreditThreshold creditThreshold);
    }
}