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
    using Services.Contracts;

    public class Configuration : IDeadlockRetryConfig, IWellCleanConfig
    {

        public Configuration()
        {
            var x = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["CleanBatchSize"], out x))
            {
                CleanBatchSize = x;
            }

            x = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["WellCleanTransactionTimeoutSeconds"], out x))
            {
                WellCleanTransactionTimeoutSeconds = x;
            }

        }
        public static string RootFolder => ConfigurationManager.AppSettings["rootFolder"];
        public int MaxNoOfDeadlockRetires => int.Parse(ConfigurationManager.AppSettings["maxNoOfDeadlockRetries"]);
        public int DeadlockRetryDelayMilliseconds => int.Parse(ConfigurationManager.AppSettings["deadlockRetryDelayMilliseconds"]);

        public int CleanBatchSize { get; set; } = 1000;
        public int WellCleanTransactionTimeoutSeconds { get; set; } = 600;
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
