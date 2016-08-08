namespace PH.Well.Adam.Events
{
    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Repositories;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    using StructureMap;

    public class Program
    {
        public static void Main(string[] args)
        {
            var container = InitIoc();

            new EventProcessor(container).Process();
        }

        private static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IExceptionEventRepository>().Use<ExceptionEventRepository>();
                    x.For<IExceptionEventService>().Use<ExceptionEventService>();
                });
        }
    }
}
