namespace PH.Well.Dashboard.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    //[HubName("ExceptionsHub")]
    public class ExceptionsHub : Hub
    {

        public virtual Task OnConnectedTask()
        {
            var version = Context.QueryString["version"];

            if (version != "1.0")
            {
                Clients.Caller.notifyWrongVersion();
            }
            return base.OnConnected();
        }


        public static void WidgetUpdates()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ExceptionsHub>();
            context.Clients.All.widgetExceptions();
        }

    }
}