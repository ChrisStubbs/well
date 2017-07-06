using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using PH.Well.Task.GlobalUplifts;
using PH.Well.Task.GlobalUplifts.Import;
using PH.Well.Task.Shared.Hangfire;
using StructureMap;

namespace PH.Well.Task.Server.Hangfire.GlobalUplifts
{
    public class UpliftsTaskConfigurator
    {
        public void Configure(Container container)
        {
            // Think of using structure map installers if dependencies become more complex

            container.Configure(x => x.For<IUpliftDataImportService>().Use<UpliftDataImportService>());
            ConfigureJobs();
        }

        private void ConfigureJobs()
        {
            var directories = ConfigurationManager.AppSettings[UpliftsTaskConsts.Config.DataDirectories];
            var minutes = int.Parse(ConfigurationManager.AppSettings[UpliftsTaskConsts.Config.MinutesInterval]);

            RecurringJob.AddOrUpdate<UpliftsTask>(UpliftsTaskConsts.TaskNames.UpliftsTask,
                t => t.Execute(new UpliftsTaskData {Directories = new[] {directories}.ToList()}),
                Cron.MinuteInterval(minutes), queue: WellTaskHangfireConsts.DefaultQueue);
        }
    }
}
