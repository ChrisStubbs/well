namespace PH.Well.Common
{
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using Contracts;

    public class DeadlockRetryHelper : IDeadlockRetryHelper
    {
        private readonly ILogger logger;
        private readonly IDeadlockRetryConfig config;
        public const int SqlDeadlockErrorNumber = 1205;

        public DeadlockRetryHelper(ILogger logger, IDeadlockRetryConfig config)
        {
            this.logger = logger;
            this.config = config;
        }

        public void Retry(Action repositoryMethod)
        {
           
            if (config.MaxNoOfDeadlockRetires == 0)
            {
                repositoryMethod();
                return;
            }

            var retryCount = 0;
            while (retryCount <= config.MaxNoOfDeadlockRetires)
            {
                try
                {
                    repositoryMethod();
                }
                catch (SqlException ex)
                {
                    
                    if (ex.Number == SqlDeadlockErrorNumber) // Deadlock 
                    {
                        retryCount++;

                        logger.LogDebug($"*********DEADLOCK OCCURRED WILL RETRY in {config.DeadlockRetryDelayMilliseconds} MilliSeconds *************");
                        logger.LogError("Deadlock Error", ex);
                        logger.LogDebug($"Deadlock exception retry: {retryCount}");
                        Thread.Sleep(TimeSpan.FromMilliseconds(config.DeadlockRetryDelayMilliseconds));
                    }
                    else
                        throw;
                }
            }

        }
    }


}
