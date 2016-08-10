namespace PH.Adam.Api
{
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Http.Cors;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using PartialResponse.Net.Http.Formatting;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var cors = new EnableCorsAttribute("*", "*", "GET", "X-Result-Count, X-Limit, X-Offset") { SupportsCredentials = true };

            config.EnableCors(cors);

            var t = new JsonMediaTypeFormatter { SerializerSettings = { Formatting = Formatting.None, ContractResolver = new CamelCasePropertyNamesContractResolver() } };

#if DEBUG
            t.SerializerSettings.Formatting = Formatting.Indented;
#endif

            config.Formatters.Clear();
            config.Formatters.Add(new PartialJsonMediaTypeFormatter() { IgnoreCase = true, SerializerSettings = t.SerializerSettings });
        }
    }
}
