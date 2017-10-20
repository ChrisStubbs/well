using System.Collections.Generic;
using System.Linq;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class AdamFileMonitorServiceConfig : IAdamFileMonitorServiceConfig
    {
        private readonly IEnumerable<Branch> branchesToProcess;
        public string RootFolder { get; }
        public string ArchiveFolder { get; }

        public AdamFileMonitorServiceConfig(string rootFolder,string archiveFolder,IEnumerable<Branch> branchesToProcess)
        {
            this.branchesToProcess = branchesToProcess;
            RootFolder = rootFolder;
            ArchiveFolder = archiveFolder;
        }

        public bool ProcessDataForBranch(Branch branch)
        {
            // If no branches are specified assume all should be processed
            if (branchesToProcess == null || !branchesToProcess.Any())
            {
                return true;
            }

            return branchesToProcess.Contains(branch);
        }
    }
}