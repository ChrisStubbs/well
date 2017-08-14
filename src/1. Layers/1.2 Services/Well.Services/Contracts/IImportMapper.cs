namespace PH.Well.Services.Contracts
{
    using Domain;


    public interface IImportMapper
    {
        void MapStop(Stop source, Stop destination);
    }

    public interface IRouteImportMapper : IImportMapper
    {
        RouteHeader MapRouteHeader(RouteHeader source, RouteHeader destination);

    }
    public interface IEpodImportMapper : IImportMapper
    {
        void MapJob(Job source, Job destination);
        void MergeRouteHeader(RouteHeader fileRouteHeader, RouteHeader dbRouteHeader);
        void MapJobDetail(JobDetail source, JobDetail destination);
    }
        
}
