namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IRouteMapper
    {
        void Map(RouteHeader from, RouteHeader to);

        void Map(Stop from, Stop to);

        void Map(Job from, Job to);

        void Map(JobDetail from, JobDetail to);
    }
}