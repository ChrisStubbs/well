using System;

namespace PH.Well.Repositories.DependancyEvents
{
    public interface IChangeNotifier
    {
        event EventHandler<ChangeEventArgs> Change;

        void Dispose();
        bool Start(string connectionStringName, string dependencyCheckSql);
        bool Stop();
    }
}