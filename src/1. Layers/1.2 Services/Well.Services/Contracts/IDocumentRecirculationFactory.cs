namespace PH.Well.Services.Contracts
{
    using System;
    using Domain.ValueObjects;

    public interface IDocumentRecirculationFactory
    {
        DocumentRecirculationTransaction Build(DateTime routeDeliveryDate, int routeNumber, int stopNumber, int stopId, int branchId);
    }
}
