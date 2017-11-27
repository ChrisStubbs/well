namespace PH.Well.Clean
{
    using System.Configuration;
    using Common;
    using Common.Contracts;
    using StructureMap;

    public class Program
    {
        public static string TargetFolder => ConfigurationManager.AppSettings["cleanFileDestinationFolder"];
        public static void Main(string[] args)
        {
            // This program will just write out a file to Adam File Folder that will trigger the clean
            // process to run from the now misnamed Adam Listener.
            // Running The Clean process whilst Epod and Route & Order files were being processed, causes
            // file imports to time out and deadlocks to occur. Need to look into the reasons for time out before re-instating 
            // the clean process into its own task.
            var container = InitIoc();
            var trigger = container.GetInstance<ITriggerCleanProcess>();
            trigger.TriggerClean(TargetFolder);
        }

        /// <summary>
        /// IOC Dependency Registration
        /// </summary>
        public static Container InitIoc()
        {
            return new Container(
                x =>
                {
                    //x.Scan(p =>
                    //{
                    //    p.AssemblyContainingType<ITriggerCleanProcess>();
                    //    p.AssemblyContainingType<ILogger>();
                    //    p.WithDefaultConventions();
                    //    p.RegisterConcreteTypesAgainstTheFirstInterface();
                    //    p.SingleImplementationsOfInterface();
                    //});
                    x.For<ILogger>().Use<NLogger>();
                    x.For<ITriggerCleanProcess>().Use<TriggerCleanProcess>();
                });
        }
    }
}
