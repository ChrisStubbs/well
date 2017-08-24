using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
using PH.Well.Task.GlobalUplifts.Import;
using StructureMap;

namespace PH.Well.Task.GlobalUplifts.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get config
            var config = GetConfig();

            //Initialize container
            var container = InitIoc();

            var task = container.GetInstance<UpliftImportTask>();
            // Start task
            task.Execute(new UpliftImportTaskData
            {
                Directories = config.Directories,
                ArchiveDirectory = config.ArchiveDirectory
            });
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
                    x.For<IPodTransactionFactory>().Use<PodTransactionFactory>();
                    x.For<IDeliveryReadRepository>().Use<DeliveryReadRepository>();
                    x.For<IDapperReadProxy>().Use<DapperReadProxy>();
                    x.For<IDbConfiguration>().Use<WellDbConfiguration>();
                    x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                    x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                    x.For<IUpliftDataImportService>().Use<UpliftDataImportService>();
                    x.For<IGlobalUpliftTransactionFactory>().Use<GlobalUpliftTransactionFactory>();
                    x.For<PH.Common.Security.Interfaces.IUserNameProvider>().Use<UserNameProvider>();
                    x.For<UpliftImportTask>().Use<UpliftImportTask>();
                });
        }

        private static GlobalUpliftRunnerConfig GetConfig()
        {
            // May be good to add checks whether each path is directory etc..
            return new GlobalUpliftRunnerConfig
            {
                Directories = ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.InputDirectories]
                    .Split(','),
                ArchiveDirectory =
                    ConfigurationManager.AppSettings[GlobalUpliftRunnerConsts.SettingNames.ArchiveDirectory]
            };
        }

        private class GlobalUpliftRunnerConfig
        {
            public string[] Directories { get; set; }

            public string ArchiveDirectory { get; set; }
        }
    }

    public class GlobalUpliftsNameProvider : IUserNameProvider
    {
        public string GetUserName()
        {
            return "GlobalUplifts";
        }
    }

   

}
