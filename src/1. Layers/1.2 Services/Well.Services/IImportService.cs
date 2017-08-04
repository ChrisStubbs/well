using System.Collections.Generic;
using PH.Well.Domain;

namespace PH.Well.Services
{
    public interface IImportService
    {
        void ImportStops(RouteHeader fileRouteHeader);
    }
}