namespace PH.Well.Api.Mapper.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public interface IBranchModelMapper
    {
        IEnumerable<BranchModel> Map(IEnumerable<Branch> branches, IEnumerable<Branch> userBranches);

        BranchDateThresholdModel MapDateThreshold(DateThreshold dateThreshold);

        DateThreshold MapDateThreshold(BranchDateThresholdModel branchDateThresholdModel);
    }
}