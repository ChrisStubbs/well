namespace PH.Well.Repositories.Contracts
{
    using Domain;
    public interface IRouteHeaderRepository: IRepository<RouteHeader, int>
    {
        RouteException GetCleanDeliveries();

        RouteException GetExceptions();
    }
}
