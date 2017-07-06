using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.StructureMap;
using PH.Well.Task.Server.Hangfire.GlobalUplifts;
using PH.Well.Task.Shared.Hangfire;
using StructureMap;

namespace PH.Well.Task.Server.Hangfire
{
    class Program
    {
        static void Main(string[] args)
        {
            // If IoC container is used it should be build before server is started
            // ex using structure map https://github.com/cocowalla/Hangfire.StructureMap

            // Specify connection string name, database needs to exist
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireTest");

            // Change retry count
            GlobalConfiguration.Configuration.UseFilter(new AutomaticRetryAttribute {Attempts = 1});

            //Test recurring job
            RecurringJob.AddOrUpdate("ConsoleWriteTestJob",() => Console.WriteLine("Test job ran"), Cron.Minutely,
                queue: WellTaskHangfireConsts.DefaultQueue);

            // Configure dependencies and jobs before starting server
            var container = new Container();

            new UpliftsTaskConfigurator().Configure(container);

            GlobalConfiguration.Configuration.UseStructureMapActivator(container);

            // Create options
            var options = new BackgroundJobServerOptions
            {
                Queues = new[] {WellTaskHangfireConsts.DefaultQueue},
            };

            // When server instance is created it's also started, call to Dispose will stop server
            // http://docs.hangfire.io/en/latest/background-processing/processing-jobs-in-console-app.html
            using (var server = new BackgroundJobServer(options))
            {
                Console.WriteLine("Well Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
