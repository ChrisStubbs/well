namespace PH.Well.Adam.Events
{
    using Common;
    using Common.Contracts;
    using Repositories;
    using Repositories.Contracts;
    using Services;
    using Services.Contracts;
    using Repositories.Read;
    using Services.DeliveryActions;
    using Services.Mappers;
    using StructureMap;

    public class Program
    {
        public static void Main(string[] args)
        {
            var container = InitIoc();
            IEventProcessor eventProcessor = container.GetInstance<IEventProcessor>();
            eventProcessor.Process();
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.For<IEventProcessor>().Use<EventProcessor>();
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
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<ICreditTransactionFactory>().Use<CreditTransactionFactory>();
                    x.For<IUserThresholdService>().Use<UserThresholdService>();
                    x.For<ICreditTransactionFactory>().Use<CreditTransactionFactory>();
                    x.For<ICreditThresholdRepository>().Use<CreditThresholdRepository>();
                    x.For<IJobDetailToDeliveryLineCreditMapper>().Use<JobDetailToDeliveryLineCreditMapper>();
                    x.For<IUserNameProvider>().Use<AdamEventsUserNameProvider>();
                    x.For<PH.Common.Security.Interfaces.IUserNameProvider>().Use<AdamEventsUserNameProvider>();
                    x.For<IPodTransactionFactory>().Use<PodTransactionFactory>();
                    x.For<IDeliveryReadRepository>().Use<DeliveryReadRepository>();
                    x.For<IDapperReadProxy>().Use<DapperReadProxy>();
                    x.For<IDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IJobService>().Use<JobService>();
                    x.For<IDateThresholdService>().Use<DateThresholdService>();
                    x.For<IAssigneeReadRepository>().Use<AssigneeReadRepository>();
                    x.For<ILineItemSearchReadRepository>().Use<LineItemSearchReadRepository>();
                    x.For<IUserThresholdService>().Use<UserThresholdService>();
                    x.For<IAdamRepository>().Use<AdamRepository>();
                    x.For<IGlobalUpliftTransactionFactory>().Use<GlobalUpliftTransactionFactory>();
                    x.For<ISeasonalDateRepository>().Use<SeasonalDateRepository>();
                    x.For<IDateThresholdRepository>().Use<DateThresholdRepository>();
                    x.For<ICustomerRoyaltyExceptionRepository>().Use<CustomerRoyaltyExceptionRepository>();
                });
        }
    }

    public class AdamEventsUserNameProvider : IUserNameProvider, PH.Common.Security.Interfaces.IUserNameProvider
    {
        public string GetUserName()
        {
            return "AdamEventUpdater";
        }

        public string ChangeUserName(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}
