namespace PH.Well.Common
{
    using System;
    using NLog;
    using ILogger = Contracts.ILogger;

    public class NLogger : ILogger
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void LogDebug(string message)
        {
            this.logger.Debug(message);
        }

        public void LogError(string message)
        {
            this.logger.Error(message);
        }

        public void LogError(string message, Exception exception)
        {
            this.logger.Error(exception, message);
        }
    }
}