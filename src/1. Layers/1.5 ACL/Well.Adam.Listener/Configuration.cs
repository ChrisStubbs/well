using System;
using System.Collections.Generic;
using System.Linq;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;

namespace PH.Well.Adam.Listener
{
    using System.Configuration;
    using System.Data;
    using Common.Contracts;

    public class Configuration : IDeadlockRetryConfig
    {
        public static string RootFolder => ConfigurationManager.AppSettings["rootFolder"];
        public int MaxNoOfDeadlockRetires => int.Parse(ConfigurationManager.AppSettings["maxNoOfDeadlockRetries"]);
        public int DeadlockRetryDelayMilliseconds => int.Parse(ConfigurationManager.AppSettings["deadlockRetryDelayMilliseconds"]);

        private static IEnumerable<Branch> branchesToProcess;
        public static IEnumerable<Branch> BranchesToProcess
        {
            get
            {
                if (branchesToProcess == null)
                {
                    var result = new List<Branch>();
                    var branchIdStrings = ConfigurationManager.AppSettings["branchesToProcess"].Split(';');
                    foreach (var branchIdString in branchIdStrings)
                    {
                        if (int.TryParse(branchIdString, out int branchId))
                        {
                            var branch = (Branch) branchId;
                            result.Add(branch);
                        }
                    }

                    branchesToProcess = result;
                }

                return branchesToProcess;
            }
        }


    }
}
