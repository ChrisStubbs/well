namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Models;
    using PH.Well.Domain;

    public class BranchModelMapper : IBranchModelMapper
    {
        public IEnumerable<BranchModel> Map(IEnumerable<Branch> branches, IEnumerable<Branch> userBranches)
        {
            var branchModels = new List<BranchModel>();

            foreach (var branch in branches)
            {
                var model = new BranchModel { Id = branch.Id, Name = branch.Name };

                var userBranchId = userBranches.FirstOrDefault(x => x.Id == branch.Id);

                if (userBranchId != null) model.Selected = true;

                branchModels.Add(model);
            }

            return branchModels;
        }
    }
}