namespace PH.Well.BDD.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Security.Principal;
    using System.Threading;
    using Common;
    using Common.Security;
    using Context;
    using Microsoft.SqlServer.Dac;
    using Microsoft.SqlServer.Management.Smo;
    using NLog;
    using StructureMap;
    using TechTalk.SpecFlow;

    [Binding]
    public class Hooks
    {
        [BeforeScenario(@"SomeUserRole")]
        public static void SetBuycoUrl()
        {
            //SetUserRoles(new List<string> { "SomeUserRole" });
        }

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
                                                x.For<Common.Contracts.ILogger>().Use<NLogger>();
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