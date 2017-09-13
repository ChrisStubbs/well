namespace PH.Well.Common.Contracts
{   
    public interface IDeadlockRetryConfig
    {
        int MaxNoOfDeadlockRetires { get; }
        int DeadlockRetryDelayMilliseconds { get; }
    }
}