namespace PH.Well.Repositories.Contracts
{
    using System;
    using DependancyEvents;

    public interface IDbChangeNotifier
    {
        event EventHandler<ChangeEventArgs> Change;
        void Dispose();
        bool Start(string dependencyCheckSql);
        bool Stop();
    }
}