namespace PH.Well.TranSend
{
    using System.Diagnostics;

    using Infrastructure;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;

    public class Program
    {
        static void Main(string[] args)
        {
            var container = DependancyRegister.InitIoc();

            var eventLogger = container.GetInstance<IEventLogger>();

            eventLogger.TryWriteToEventLog(
                EventSource.WellTaskRunner,
                "Processing transend imports...",
                8773,
                EventLogEntryType.Information);

            new Import().Process(container);      
        }
    }
}
