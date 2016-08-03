using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PH.Well.Dashboard.Controllers
{
    using Hubs;
    using Microsoft.AspNet.SignalR;

    public class RefreshController : ApiController
    {
        [Route("api/refresh")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<RefreshHub>();
            context.Clients.All.dataRefreshed();

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
