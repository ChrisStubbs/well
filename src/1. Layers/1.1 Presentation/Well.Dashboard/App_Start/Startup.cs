using Microsoft.Owin;

[assembly: OwinStartup(typeof(PH.Well.Dashboard.Startup))]
namespace PH.Well.Dashboard
{
    using System;

    using Microsoft.AspNet.SignalR;
    using Microsoft.Owin.Cors;
    using Owin;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration { EnableDetailedErrors = true, EnableJSONP = true};
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}