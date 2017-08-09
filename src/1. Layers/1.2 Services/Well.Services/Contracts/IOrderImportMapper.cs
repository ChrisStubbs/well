namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;

    public interface IOrderImportMapper
    {

        void Map(StopUpdate from, Stop to);

        void Map(JobUpdate from, Job to);

        void Map(JobDetailUpdate from, JobDetail to);
    }
}