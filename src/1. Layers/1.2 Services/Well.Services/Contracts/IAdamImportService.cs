namespace PH.Well.Services.Contracts
{
    using System;
    using PH.Well.Domain;

    public interface IAdamImportService
    {
        void Import(RouteDelivery route, string fileName);

        //void ImportRouteHeader(RouteHeader header, int routeId);
    }
}