namespace PH.Well.Services.Contracts
{
    using Domain;


    public interface IImportMapper
    {
        void MapStop(Stop source, Stop destination);
        void MapJob(Job source, Job destination);
        void MapJobDetail(JobDetail source, JobDetail destination);
    }

    public interface IAdamImportMapper : IImportMapper
    {
        RouteHeader MapRouteHeader(RouteHeader source, RouteHeader destination);

    }
    public interface IEpodImportMapper : IImportMapper
    {
        void MergeRouteHeader(RouteHeader fileRouteHeader, RouteHeader dbRouteHeader);   
    }
        
}
