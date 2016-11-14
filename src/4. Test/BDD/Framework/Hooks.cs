namespace PH.Well.BDD.Framework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Principal;
    using System.Threading;
    using Common;
    using Common.Contracts;
    using Common.Security;
    using Context;
    using Factories;
    using Microsoft.SqlServer.Dac;
    using Microsoft.SqlServer.Management.Smo;
    using Repositories;
    using Repositories.Contracts;
    using Services;
    using Services.Contracts;
    using Services.EpodImport;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class Hooks
    {
        [BeforeScenario(@"RoleBranchManager")]
        public static void SetBranchManagerRole()
        {
            SetUserRoles(new List<string> { "BranchManager" });
        }

        [BeforeScenario(@"RoleCustomerServiceManager")]
        public static void SetServiceManager()
        {
            SetUserRoles(new List<string> { "CustomerServiceManager" });
        }

        [BeforeScenario(@"RoleCustomerServiceUser")]
        public static void SetCustomerServiceUser()
        {
            SetUserRoles(new List<string> { "CustomerServiceUser" });
        }

        [BeforeScenario(@"RoleSuperUser")]
        public static void SetSuperUser()
        {
            SetUserRoles(new List<string> { "SuperUser" });
        }

        [BeforeScenario(@"WebDriverFeature")]
        public static void SetDashboardUrl()
        {
            UrlContext.CurrentUrl = Configuration.DashboardUrl;
        }

        #region WebDriver setup
        [BeforeFeature(@"WebDriverFeature")]
        public static void SetDriver()
        {
            DriverContext.CurrentDriver = DriverFactory.Create(Configuration.Driver);
        }

        [AfterFeature(@"WebDriverFeature")]
        public static void DisposeDriver()
        {
            var driver = DriverContext.CurrentDriver;

            driver.Close();
            driver.Quit();
        }
        #endregion

        [BeforeFeature]
        public static void SetupDependancies()
        {
            InitIoc();
        }

        #region Database setup

        [BeforeTestRun]
        public static void SetupDatabase()
        {
            DropDatabase();
            RunDacpac();
        }

        [AfterTestRun]
        public static void ResetPermissions()
        {
            //SetUserRoles(new List<string> { "BuyCo", "Buyer", "Finance" });
        }

        private static void DropDatabase()
        {
            try
            {
                var server = new Server(Configuration.SqlInstance);

                var database = server.Databases[Configuration.Database];

                if (database != null)
                {
                    server.KillDatabase(Configuration.Database);
                }
            }
            catch (Exception ex)
            {
                new NLogger().LogError("Cant kill the database!", ex);       
                throw;
            }
        }

        private static void RunDacpac()
        {
            try
            {
                var dacServices = new DacServices(Configuration.DatabaseConnection);

                string path = AppDomain.CurrentDomain.BaseDirectory;
                var dacPacPath = Path.GetFullPath(Path.Combine(path, Framework.Configuration.PathToDacpac));
                var dacpac = DacPackage.Load(dacPacPath);

                var deployOptions = new DacDeployOptions { BlockOnPossibleDataLoss = false, IncludeTransactionalScripts = true };

                var token = new CancellationTokenSource();

                dacServices.Deploy(dacpac, Configuration.Database, true, deployOptions, token.Token);
            }
            catch (Exception ex)
            {
                new NLogger().LogError("Cant run the dacpac!", ex);       
                throw;
            }
        }

        #endregion

        private static void InitIoc()
        {
            var container = new Container(
                                            x =>
                                            {
                                                x.For<IEpodSchemaValidator>().Use<EpodSchemaValidator>();
                                                x.For<ILogger>().Use<NLogger>();
                                                x.For<IWellDapperProxy>().Use<WellDapperProxy>();
                                                x.For<IRouteHeaderRepository>().Use<RouteHeaderRepository>();
                                                x.For<IEpodDomainImportProvider>().Use<EpodDomainImportProvider>();
                                                x.For<IWellDbConfiguration>().Use<WellDbConfiguration>();
                                                x.For<IStopRepository>().Use<StopRepository>();
                                                x.For<IEpodDomainImportService>().Use<EpodDomainImportService>();
                                                x.For<IStopRepository>().Use<StopRepository>();
                                                x.For<IJobRepository>().Use<JobRepository>();
                                                x.For<IJobDetailRepository>().Use<JobDetailRepository>();
                                                x.For<IJobDetailDamageRepository>().Use<JobDetailDamageRepository>();
                                                x.For<IWebClient>().Use<WebClient>();
                                                x.For<IFileModule>().Use<FileModule>();
                                                x.For<IFileService>().Use<FileService>();
                                                x.For<IAccountRepository>().Use<AccountRepository>();
                                                x.For<IAdamFileMonitorService>().Use<AdamFileMonitorService>();
                                                x.For<IAuditRepository>().Use<AuditRepository>();
                                                x.For<INotificationRepository>().Use<NotificationRepository>();
                                                x.For<IDapperProxy>().Use<WellDapperProxy>();
                                            });

            FeatureContextWrapper.SetContextObject(ContextDescriptors.StructureMapContainer, container);
        }

        private static void SetUserRoles(List<string> roles)
        {
            string userName = WindowsIdentity.GetCurrent().Name;

            var securityApiClient = new SecurityApiClient();
            securityApiClient.AddUserToRoles(new UserRoleRequest()
            {
                ApplicationId = Guid.Parse(Configuration.ApplicationId),
                UserIdentifier = userName,
                Roles = roles
            });
        }
    }
}