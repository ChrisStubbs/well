namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;
    using Models;

    public interface ISingleRouteMapper
    {
        SingleRouteView Map(List<Branch> branches, RouteHeader route, List<Stop> stops, List<Job> jobs, List<Assignee> assignee);
    }
}