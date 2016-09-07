namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Models;

    public class WidgetController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public WidgetController(IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("widgets")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                
                var widgets = new List<WidgetModel>()
                {
                    new WidgetModel()
                    {
                        Name = "Exceptions",
                        Count = 5,
                        Description = "Deliveries with short or damaged quantities",
                        SortOrder = 1,
                        Link = "/exceptions",
                        LinkText = "deliveries with exceptions",
                        WarningLevel = 10
                    },
                    new WidgetModel()
                    {
                        Name = "Assigned",
                        Count = 3,
                        Description = "Deliveries assigned to you for actioning",
                        SortOrder = 2,
                        Link = "/exceptions",
                        LinkText = "exceptions assigned to you",
                        WarningLevel = 10
                    },
                    new WidgetModel()
                    {
                        Name = "Outstanding",
                        Count = 10,
                        Description = "Exceptions raised over 24 hours ago",
                        SortOrder = 3,
                        Link = "/exceptions",
                        LinkText = "outstanding exceptions",
                        WarningLevel = 10
                    },
                    new WidgetModel()
                    {
                        Name = "Notifications",
                        Count = 0,
                        Description = "Unarchived notifications",
                        Link = "/notifications",
                        LinkText = "notifications",
                        SortOrder = 4,
                        WarningLevel = 10
                    }
                };
                return this.Request.CreateResponse(HttpStatusCode.OK, widgets);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occcured when getting widgets");
            }
        }

    }
}
