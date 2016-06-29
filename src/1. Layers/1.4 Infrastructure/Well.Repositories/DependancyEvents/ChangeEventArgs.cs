namespace PH.Well.Repositories.DependancyEvents
{
    using System;
    using Domain.Enums;

    [Serializable]
    public sealed class ChangeEventArgs : EventArgs
    {
        public ChangeEventArgs(ChangeInfo info, ChangeSource source, ChangeType type)
        {
            this.Info = info;
            this.Source = source;
            this.Type = type;
        }

        public ChangeInfo Info { get; private set; }

        public ChangeSource Source { get; private set; }

        public ChangeType Type { get; private set; }
    }
}
