namespace PH.Well.Dashboard
{
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using PH.Well.Dashboard.App_Start;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbChangeNotifierStartup.Init();

#if DEBUG
            //Don't run signalr locally as it breaks protractor UI tests, won't be an issue once signalr is using WebSockets 
#else
            DbChangeNotifierStartup.Init(); //We can get rid of this 
#endif
        }
    }
}
