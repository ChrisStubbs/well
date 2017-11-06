using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    public class GlobalUpliftRunnerConfig
    {
        public string[] Directories { get; set; }

        public string ArchiveDirectory { get; set; }

        public string BranchFilter { private get; set; }

        public string EpodSources { get; set; }

        /// <summary>
        /// Return branch numbers or empty list it not specified
        /// </summary>
        public List<int> Branches
        {
            get
            {
                return this.BranchFilter.Split(new char[] { ';', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x))
                    .ToList();
            }
        }

    }

}
