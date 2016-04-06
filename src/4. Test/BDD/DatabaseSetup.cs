namespace PH.Well.BDD
{
    using System;
    using System.Threading;
    using Framework;
    using Microsoft.SqlServer.Dac;
    using Microsoft.SqlServer.Management.Smo;

    using PH.Well.Common;
    using Configuration = Framework.Configuration;

    public class DatabaseSetup
    {
        public static void Setup()
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

            try
            {
                var dacServices = new DacServices(Configuration.DatabaseConnection);

                var dacpac = DacPackage.Load(Configuration.PathToDacpac);

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
    }
}