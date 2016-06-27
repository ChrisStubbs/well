namespace Well.Dashboard
{
    using System.Configuration;
    using System.Data.SqlClient;
    using Owin;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin;
    using Microsoft.Owin.Logging;
    using PH.Well.Dashboard.App_Start;
    using PH.Well.Dashboard.Hubs;
    using PH.Well.Repositories;
    using PH.Well.Repositories.DependancyEvents;


    public class MvcApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["Well"].ConnectionString;
        protected void Application_Start()
        {
            NotifyStartUp();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SqlDependency.Start(con);

        }

        protected void Application_End()
        {
            SqlDependency.Stop(con);
        }

        protected void NotifyStartUp()
        {
            var notifier = StructuremapMvc.StructureMapDependencyScope.Container.GetInstance<IChangeNotifier>(); 
            notifier.Change += this.OnChange;
            notifier.Start(con, StoredProcedures.DependancyGetExceptions);
        }


        private void OnChange(object sender, ChangeEventArgs e)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<ExceptionsHub>();
            context.Clients.All.widgetExceptions();
        }
    }

}
