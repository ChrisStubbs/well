using System;
using PH.Common.Storage;
using PH.Common.Storage.Config.ConfigFile;
using PH.Common.Storage.Constants.Enums;
using PH.Common.Storage.Local;
using PH.Shared.AccountService.Client.Interfaces;
using PH.Shared.EmailService.Interfaces;
using PH.Well.Common;
using PH.Well.Common.Contracts;
using PH.Well.Repositories;
using PH.Well.Repositories.Contracts;
using PH.Well.Repositories.Read;
using PH.Well.Services;
using PH.Well.Services.Contracts;
using PH.Well.Services.DeliveryActions;
using PH.Well.Services.Mappers;
using PH.Well.Task.GlobalUplifts.EpodFiles;
using PH.Well.Task.GlobalUplifts.Import;
using StructureMap;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Register any Storage providers and the config provider
            Storage.RegisterStorageProviderFactory(eStorageType.Local, new LocalStorageProviderFactory());
            Storage.RegisterStorageConfigProvider(new ConfigFileConfigProvider());

            //Initialize container
            var container = InitIoc();

            // Get config
            var config = container.GetInstance<GlobalUpliftRunnerConfig>();

            var task = container.GetInstance<UpliftImportTask>();
            // Start task
            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-BatchImporter");
            ExecuteStep("GlobalUplift-BatchImporter", () => task.Execute(new UpliftImportTaskData
            {
                Directories = config.Directories,
                ArchiveDirectory = config.ArchiveDirectory
            }));

            // Process all global uplifts from files from yesterday and today by default
            var processor = container.GetInstance<EpodGlobalUpliftProcessor>();
            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-epodProcessor");
            ExecuteStep("GlobalUplift-epodProcessor", () =>
            {
                processor.Sources = config.EpodSources;
                processor.Branches = config.Branches;
                processor.StartDate = config.TestStartDate;
                processor.EndDate = config.TestEndDate;
                processor.Run();
            });

            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-accountProcessor");
            ITaskProcessor taskProcessor = container.GetInstance<UpdateAccountDetailsProcessor>();
            ExecuteStep("GlobalUplift-accountProcessor", () => taskProcessor.Run());

            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-upliftProcessor");
            taskProcessor = container.GetInstance<GenerateGlobalUpliftEventProcessor>();
            ExecuteStep("GlobalUplift-upliftProcessor", () => taskProcessor.Run());


            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-emailProcessor");
            taskProcessor = container.GetInstance<SendBranchEmailProcessor>();
            ExecuteStep("GlobalUplift-emailProcessor", () => taskProcessor.Run());
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
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
                    x.For<IEventLogger>().Use<EventLogger>();
                    x.For<ICreditTransactionFactory>().Use<CreditTransactionFactory>();
                    x.For<IUserThresholdService>().Use<UserThresholdService>();
                    x.For<ICreditTransactionFactory>().Use<CreditTransactionFactory>();
                    x.For<ICreditThresholdRepository>().Use<CreditThresholdRepository>();
                    x.For<IJobDetailToDeliveryLineCreditMapper>().Use<JobDetailToDeliveryLineCreditMapper>();
                    x.For<IUserNameProvider>().Use<GlobalUpliftsNameProvider>();
                    x.For<PH.Common.Security.Interfaces.IUserNameProvider>().Use<GlobalUpliftsNameProvider>();
                    x.For<IPodTransactionFactory>().Use<PodTransactionFactory>();
                    x.For<IDeliveryReadRepository>().Use<DeliveryReadRepository>();
                    x.For<IDapperReadProxy>().Use<DapperReadProxy>();
                    x.For<IDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IUpliftDataImportService>().Use<UpliftDataImportService>();
                    x.For<IGlobalUpliftTransactionFactory>().Use<GlobalUpliftTransactionFactory>();
                    x.For<UpliftImportTask>().Use<UpliftImportTask>();
                    x.For<EpodGlobalUpliftProcessor>().Use<EpodGlobalUpliftProcessor>();
                    x.For<IGlobalUpliftEmailService>().Use<GlobalUpliftEmailClient>();
                    x.For<IAccountServiceClient>().Use<AccountServiceClient>();
                    x.For<GlobalUpliftRunnerConfig>().Use<GlobalUpliftRunnerConfig>().Singleton();
                });
        }

        /// <summary>
        /// Helper method that logs and encapsulates action in try catch block
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="action"></param>
        private static void ExecuteStep(string stepName, Action action)
        {
            try
            {
                Console.WriteLine($"Executing step {stepName}");
                action();
                Console.WriteLine($"Completed step {stepName}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{stepName} exception {e}");
            }
        }
    }
}
