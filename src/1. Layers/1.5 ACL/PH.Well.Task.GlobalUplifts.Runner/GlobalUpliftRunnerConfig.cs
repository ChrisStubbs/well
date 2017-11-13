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
        public GlobalUpliftRunnerConfig()
        {
            Directories = ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.InputDirectories]
                .Split(',');
            ArchiveDirectory =
                ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.ArchiveDirectory];
            BranchFilter =
                ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.BranchFilter];
            EpodSources = ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.EpodSources];
            GlobalUpliftEmailServiceUrl =
                ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.GlobalUpliftEmailServiceUrl];
            AccountServiceUrl =
                ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.AccountServiceUrl];
        }

        public string[] Directories { get; set; }

        public string ArchiveDirectory { get; set; }

        public string BranchFilter { private get; set; }

        public string EpodSources { get; set; }

        public string GlobalUpliftEmailServiceUrl { get; set; }

        public string AccountServiceUrl { get; set; }

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
