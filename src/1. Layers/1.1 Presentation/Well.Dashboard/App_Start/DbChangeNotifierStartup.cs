namespace PH.Well.Dashboard.App_Start
{
    using Hubs;
    using Microsoft.AspNet.SignalR;
    using Repositories;
    using Repositories.Contracts;
    using Repositories.DependancyEvents;

    public class DbChangeNotifierStartup
    {
        public static void Init()
        {
            var notifier = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<IDbChangeNotifier>();
            notifier.Change += OnChange;
            notifier.Start(StoredProcedures.WidgetStatsGet);
        }

        private static void OnChange(object sender, ChangeEventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ExceptionsHub>();
            context.Clients.All.widgetExceptions();
        }
    }
}