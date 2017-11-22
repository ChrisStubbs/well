namespace PH.Well.Clean
{
    using System;
    using System.Configuration;
    using System.IO;
    using Common.Contracts;
    using StructureMap;

    public class Program
    {
        public static string TargetFolder => ConfigurationManager.AppSettings["downloadFilePath"];
        public static void Main(string[] args)
        {
            // This program will just write out a file to Adam File Folder that will trigger the clean
            // process to run from the now misnamed Adam Listener.
            // Running The Clean process whilst Epod and Route & Order files were being processed, causes
            // file imports to time out and deadlocks to occur. Need to look into the reasons for time out before re-instating 
            // the clean process into its own task.
            var container = InitIoc();
            var logger = container.GetInstance<ILogger>();
            try
            {

                var filename = Path.Combine(TargetFolder, $"CLEAN__{DateTime.Now:yyyyMMdd_HHmmssff}.txt");
                logger.LogDebug($"Writing empty trigger file {filename}");
                File.Create(filename).Dispose();
            }
            catch (Exception ex)
            {
                logger.LogError("Error writing file", ex);
                throw;
            }
            
            //Clean().Wait();
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    x.Scan(p =>
                    {
                        //p.AssemblyContainingType<IWellCleanUpService>();
                        //p.AssemblyContainingType<IStopRepository>();
                        p.AssemblyContainingType<IEventLogger>();
                        p.WithDefaultConventions();
                        p.RegisterConcreteTypesAgainstTheFirstInterface();
                        p.SingleImplementationsOfInterface();
                    });
                    //x.For<IUserNameProvider>().Use<WellCleanUserNameProvider>();
                    //x.For<IWellCleanConfig>().Use<Configuration>();
                });
        }
    }
}
