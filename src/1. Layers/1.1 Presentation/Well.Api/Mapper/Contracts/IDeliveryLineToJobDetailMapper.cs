namespace PH.Well.Api.Mapper.Contracts
{
    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public interface IDeliveryLineToJobDetailMapper
    {
        void Map(DeliveryLineModel from, JobDetail to);
    }
}