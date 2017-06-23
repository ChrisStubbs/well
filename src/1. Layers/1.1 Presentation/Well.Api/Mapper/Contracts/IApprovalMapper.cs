namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;
    using Models;

    public interface IApprovalMapper
    {
        IEnumerable<ApprovalModel> Map(IEnumerable<Job> jobs, IEnumerable<Assignee> assignees);
    }
}