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
                    x.For<IDapperProxy>().Use<WellDapperProxy>();
                    x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IAdamRepository>().Use<AdamRepository>();
                    x.For<IExceptionEventRepository>().Use<ExceptionEventRepository>();
                    x.For<IDeliveryLineActionService>().Use<DeliveryLineActionService>();
                    x.For<IJobRepository>().Use<JobRepository>();
                    x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                    x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                    x.For<IAccountRepository>().Use<AccountRepository>();
                    x.For<IUserRepository>().Use<UserRepository>();
                });
        }
    }
}
