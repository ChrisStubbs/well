namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IEpodUpdateService
    {
        void Update(List<RouteDelivery> routes);
    }
}