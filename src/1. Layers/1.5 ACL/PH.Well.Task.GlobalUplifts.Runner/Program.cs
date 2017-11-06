using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PH.Common.Storage;
using PH.Common.Storage.Config.ConfigFile;
using PH.Common.Storage.Constants.Enums;
using PH.Common.Storage.Local;
using PH.Shared.EmailService.Client.Rest;
using PH.Shared.EmailService.Interfaces;
using PH.Shared.Well.TranSend.File.Search;
using PH.Well.Common;
using PH.Well.Common.Contracts;
using PH.Well.Repositories;
using PH.Well.Repositories.Contracts;
using PH.Well.Repositories.Read;
using PH.Well.Services;
using PH.Well.Services.Contracts;
using PH.Well.Services.DeliveryActions;
using PH.Well.Services.Mappers;
using PH.Well.Task.GlobalUplifts.Csv;
using PH.Well.Task.GlobalUplifts.EpodFiles;
using PH.Well.Task.GlobalUplifts.Import;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Pipeline;

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
            task.Execute(new UpliftImportTaskData
            {
                Directories = config.Directories,
                ArchiveDirectory = config.ArchiveDirectory
            });

            // Process all global uplifts from files from yesterday and today by default
            var processor = container.GetInstance<EpodGlobalUpliftProcessor>();
            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-epodProcessor");
            processor.Sources = config.EpodSources;
            processor.Branches = config.Branches;
            processor.Run();

            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-accountProcessor");
            ITaskProcessor taskProcessor = container.GetInstance<UpdateAccountDetailsProcessor>();
            taskProcessor.Run();

            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-upliftProcessor");
            taskProcessor = container.GetInstance<GenerateGlobalUpliftEventProcessor>();
            taskProcessor.Run();

            GlobalUpliftsNameProvider.SetUsername("GlobalUplift-emailProcessor");
            taskProcessor = container.GetInstance<SendBranchEmailProcessor>();
            taskProcessor.Run();

            //SearchCriteria criteria = new SearchCriteria()
            //{
            //    Branches = config.Branches,
            //    JobType = "UPL-GLO"
            //};
            //DateTime dateFrom = DateTime.Today.AddDays(-1);
            //dateFrom = new DateTime(2017, 9, 4);
            //DateTime dateTo = DateTime.Today;
            //dateTo = new DateTime(2017, 10, 9);
            //processor.ProcessGlobalUplifts(config.EpodSources, dateFrom, dateTo, criteria);
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
                    x.For<GlobalUpliftRunnerConfig>().Use<GlobalUpliftRunnerConfig>().Singleton();
                });
        }
    }
}
