namespace PH.Well.FileDistributor.Infrastructure
{
    using Common;
    using Common.Contracts;
    using Contracts;

    using StructureMap;

    public static class DependancyRegister
    {
        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<ILogger>().Use<NLogger>();
                    x.For<IWebClient>().Use<WebClient>();
                    x.For<IFtpClient>().Use<FtpClient>();
                    //x.For<IUserNameProvider>().Use<FileDistributorUserNameProvider>();
                    x.For<IDeadlockRetryConfig>().Use<Configuration>();
                    x.For<IDeadlockRetryHelper>().Use<DeadlockRetryHelper>();
                    x.For<IEpodProvider>().Use<EpodFtpProvider>();
                });
        }
    }
}
