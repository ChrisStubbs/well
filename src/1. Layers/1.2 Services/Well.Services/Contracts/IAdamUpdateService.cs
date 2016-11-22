namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IAdamUpdateService
    {
        void Update(List<RouteUpdates> routes);
    }
}