namespace PH.Well.Common.Contracts
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;

    public interface IEventLogger
    {
        bool TryWriteToEventLog(EventSource source, Exception exception);

        bool TryWriteToEventLog(EventSource source, string logText, int eventId,
            EventLogEntryType entryType = EventLogEntryType.Error);
    }
}