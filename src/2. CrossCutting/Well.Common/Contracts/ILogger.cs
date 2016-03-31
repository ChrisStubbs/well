namespace PH.Well.Common.Contracts
{
    using System;

    public interface ILogger
    {
        void LogDebug(string message);

        void LogError(string message);

        void LogError(string message, Exception exception);
    }
}