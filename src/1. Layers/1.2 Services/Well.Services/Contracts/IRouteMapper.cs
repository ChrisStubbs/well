namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;

    public interface IRouteMapper
    {
        void Map(RouteHeader from, RouteHeader to);

        void Map(Stop from, Stop to);

        void Map(StopUpdate from, Stop to);

        void Map(Job from, Job to);

        void Map(JobUpdate from, Job to);

        void Map(JobDetail from, JobDetail to);

        void Map(JobDetailUpdate from, JobDetail to);
    }
}