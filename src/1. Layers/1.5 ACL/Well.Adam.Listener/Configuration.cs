using System;
using System.Collections.Generic;
using PH.Well.Domain.Enums;

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
        public static IEnumerable<Branch> BranchesToProcess
        {
            get
            {
                var result = new List<Branch>();
                var branchIds = ConfigurationManager.AppSettings["branchesToProcess"].Split(';');
                foreach (var branchId in branchIds)
                {
                    var branch = (Branch) int.Parse(branchId);
                    result.Add(branch);
                }

                return result;
            }
        }
    }
}
