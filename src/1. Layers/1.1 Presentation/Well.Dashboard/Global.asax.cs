namespace Well.Dashboard
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using PH.Well.Dashboard.App_Start;
    using PH.Well.Domain.Enums;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DbChangeNotifierStartup.Init();
        }

        protected void Application_End()
        {

            notifier.Start(con, StoredProcedures.WidgetStatsGet);
            if (e.Type == ChangeType.Change)
            {
                var context = GlobalHost.ConnectionManager.GetHubContext<ExceptionsHub>();
                context.Clients.All.widgetExceptions();
            }
        }
    }

}
