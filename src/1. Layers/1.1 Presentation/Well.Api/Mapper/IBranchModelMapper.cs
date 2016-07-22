namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public interface IBranchModelMapper
    {
        IEnumerable<BranchModel> Map(IEnumerable<Branch> branches, IEnumerable<Branch> userBranches);
    }
}