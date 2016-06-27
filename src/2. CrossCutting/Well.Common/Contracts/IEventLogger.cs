namespace PH.Well.Common.Contracts
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using PH.Well.Common;
    using EventSource = Common.EventSource;

    public interface IEventLogger
    {
        bool TryWriteToEventLog(EventSource source, Exception exception);

        bool TryWriteToEventLog(EventSource source, string logText, int eventId,
            EventLogEntryType entryType = EventLogEntryType.Error);
    }
}