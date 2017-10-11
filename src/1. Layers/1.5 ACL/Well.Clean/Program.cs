namespace PH.Well.Clean
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Common;
    using Common.Contracts;
    using Repositories.Contracts;
    using Services.Contracts;

    using StructureMap;

    public class Program
    {
        public static void Main(string[] args)
        {
            Clean().Wait();
        }

        private static Task Clean()
        {
            var container = InitIoc();

            var eventLogger = container.GetInstance<IEventLogger>();

            eventLogger.TryWriteToEventLog(
                EventSource.WellTaskRunner,
                "Processing clean deliveries...",
                2123,
                EventLogEntryType.Information);

            return container.GetInstance<IWellCleanUpService>().Clean();
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.Scan(p =>
                    {
                        p.AssemblyContainingType<IWellCleanUpService>();
                        p.AssemblyContainingType<IStopRepository>();
                        p.AssemblyContainingType<IEventLogger>();
                        p.WithDefaultConventions();
                        p.RegisterConcreteTypesAgainstTheFirstInterface();
                        p.SingleImplementationsOfInterface();
                    });
                    x.For<IUserNameProvider>().Use<WellCleanUserNameProvider>();
                    x.For<IWellCleanConfig>().Use<Configuration>();
                });
        }
    }
}
