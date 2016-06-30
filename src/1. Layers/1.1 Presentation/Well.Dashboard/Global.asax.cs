namespace Well.Dashboard
{
    using System.Configuration;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Microsoft.AspNet.SignalR;
    using PH.Well.Dashboard.App_Start;
    using PH.Well.Dashboard.Hubs;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories;

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
        }
    }

}
