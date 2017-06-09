namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;

    public interface IRouteMapper
    {
        void Map(RouteHeader from, RouteHeader to);

        void Map(StopDTO from, Stop to);

        void Map(StopUpdate from, Stop to);

        void Map(JobDTO from, Job to);

        void Map(JobUpdate from, Job to);

        void Map(JobDetailDTO from, JobDetail to);

        void Map(JobDetailUpdate from, JobDetail to);
    }
}