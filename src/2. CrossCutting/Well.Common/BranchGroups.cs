using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Common
{
    public class BranchGroups
    {
        public BranchGroups()
        {
            this.Groups = new List<BranchGroup>();
        }

        public List<BranchGroup> Groups { get; set; }

        public string GetGroupNameForBranch(int branchId)
        {
            foreach (var g in this.Groups)
            {
                if (g.BranchIds.Any(p => p == branchId))
                {
                    return g.GroupName;
                }
            }

            throw new Exception("Bad configuration");
        }

        public class BranchGroup
        {
            public string GroupName { get; set; }
            public List<int> BranchIds { get; set; }
        }
    }
}
