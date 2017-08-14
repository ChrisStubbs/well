namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;
    using Models;

    public interface ISingleRouteMapper
    {
        SingleRoute Map(
            List<Branch> branches, 
            RouteHeader route, 
            List<Stop> stops, 
            List<Job> jobs, 
            List<Assignee> assignee,
            IEnumerable<JobDetailLineItemTotals> jobDetailTotalsPerRouteHeader,
            Dictionary<int, string> jobPrimaryAccountNumber);
    }
}