namespace PH.Well.Common
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Text;

    using PH.Well.Common.Contracts;

    public class EventLogger : IEventLogger
    {
        private readonly ILogger logger;

        public EventLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public bool TryWriteToEventLog(EventSource source, Exception exception)
        {
            try
            {
                WriteEventLog(source.ToString(), exception.ToString(), 1, EventLogEntryType.Error);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured writing to the event log!", ex);
                return false;
            }
        }

        public bool TryWriteToEventLog(EventSource source, string logText, int eventId, EventLogEntryType entryType = EventLogEntryType.Error)
        {
            try
            {
                WriteEventLog(source.ToString(), logText, eventId, entryType);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured writing to the event log!", ex);
                return false;
            }
        }

        private void WriteEventLog(string source, string logText, int eventId, EventLogEntryType entryType)
        {
            var fullSource = $"Well.{source}";
            if (!EventLog.SourceExists(fullSource))
            {
                EventLog.CreateEventSource(fullSource, "Application");
            }

            var message = new StringBuilder(logText);

            EventLog.WriteEntry(fullSource, message.ToString(), entryType, eventId);
        }
    }
}