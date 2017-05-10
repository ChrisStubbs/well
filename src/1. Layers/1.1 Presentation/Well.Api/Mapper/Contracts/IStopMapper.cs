namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;
    using Models;

    public interface IStopMapper
    {
        StopModel Map(List<Branch> branches, RouteHeader route, Stop stop, List<Job> jobs, List<Assignee> assignees);
    }
}