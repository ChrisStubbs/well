using System;
using System.Collections.Generic;
using System.Linq;
namespace PH.Well.Repositories
{
    using System.Configuration;
    using System.Data;

    public abstract class BaseDbConfiguration
    {

        public int TransactionTimeout => GetIntSetting("transactionTimeout");

        public int MaxNoOfDeadlockRetries => GetIntSetting("MaxNoOfDeadlockRetries");

        public int? CommandTimeout
        {
            get
            {
                var appSetting = ConfigurationManager.AppSettings["CommandTimeoutSeconds"];
                if (string.IsNullOrWhiteSpace(appSetting)) return null;

                int value;
                if (int.TryParse(appSetting, out value) == false)
                {
                    throw new ConstraintException("AppSetting CommandTimeoutSeconds must be an integer");
                }
                return value;
            }
        }

        protected int GetIntSetting(string appSettingKey)
        {
            var appSetting = ConfigurationManager.AppSettings[appSettingKey];
            int value;
            if (string.IsNullOrWhiteSpace(appSetting) || int.TryParse(appSetting, out value) == false)
            {
                throw new ConstraintException($"AppSetting {appSettingKey} must be an integer");
            }
            return value;
        }
    }
}
