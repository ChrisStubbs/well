namespace PH.Well.Api
{
    using System.Web.Http;
    using Newtonsoft.Json.Serialization;
    using System.Web.Http.Cors;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var cors = new EnableCorsAttribute("*", "*", "GET", "X-Result-Count, X-Limit, X-Offset") { SupportsCredentials = true };

            config.EnableCors(cors);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}
