using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    public static class GlobalUpliftRunnerConsts
    {
        public static class SettingNames
        {
            /// <summary>
            /// Setting should contain comma separated list of directories
            /// </summary>
            public const string InputDirectories = "GlobalUplift.Task.InputDirectories";
            public const string ArchiveDirectory = "GlobalUplift.Task.ArchiveDirectory";
            public const string BranchFilter = "GlobalUplift.Task.Branches";
            public static string EpodSources = "GlobalUplift.Task.EpodSources";
        }
    }
}
