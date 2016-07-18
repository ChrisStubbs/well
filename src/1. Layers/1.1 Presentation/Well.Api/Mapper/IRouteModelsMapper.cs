using System.Collections.Generic;
using PH.Well.Api.Models;
using PH.Well.Domain;

namespace PH.Well.Api.Mapper
{
    public interface IRouteModelsMapper
    {
        IEnumerable<RouteModel> Map(IEnumerable<RouteHeader> source);
    }
}