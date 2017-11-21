namespace PH.Well.FileDistributor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Contracts;
    using Infrastructure;
    using Newtonsoft.Json;
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
                "Processing FileDistributor imports...",
                8773,
                EventLogEntryType.Information);

            container.GetInstance<IEpodProvider>().Import();
        }
    }
}
