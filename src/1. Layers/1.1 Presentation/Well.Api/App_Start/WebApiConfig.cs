namespace PH.Well.Api
{
    using System.Web.Http;
    using Newtonsoft.Json.Serialization;
    using System.Web.Http.Cors;
    using System.Web.Http.ExceptionHandling;
    using Elmah.Contrib.WebApi;
    using System.Reflection;
    using System.Linq;
    using Infrastructure;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enable elmah
            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{branchId}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { controller = GetControllerNames() }
            );

            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: "{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );

            var cors = new EnableCorsAttribute("*", "*", "GET", "X-Result-Count, X-Limit, X-Offset") { SupportsCredentials = true };

            config.EnableCors(cors);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Filters.Add(new AuthorizeAttribute());
            config.Formatters.Add(new CsvMediaTypeFormatter());
        }

        private static string GetControllerNames()
        {
            var controllerNames = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(x =>
                    x.IsSubclassOf(typeof(ApiController)) &&
                    x.FullName.StartsWith(MethodBase.GetCurrentMethod().DeclaringType.Namespace + ".Controllers"))
                .ToList()
                .Select(x => x.Name.Replace("Controller", ""));

            return string.Join("|", controllerNames);
        }
    }
}
