namespace PH.Well.Common.Contracts
{
    using System;
    public interface IDeadlockRetryHelper
    {
        void Retry(Action repositoryMethod);
        
    }
}
