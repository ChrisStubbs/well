namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Models;

    using PH.Well.Common.Security;
    using Repositories.Contracts;

    public class WidgetController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IUserStatsRepository userStatsRepository;

        public WidgetController(IServerErrorResponseHandler serverErrorResponseHandler,
            IUserStatsRepository userStatsRepository)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.userStatsRepository = userStatsRepository;
        }

        [Authorize(Roles = SecurityPermissions.LandingPage)]
        [Route("widgets")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                var userStats = userStatsRepository.GetByUser(UserIdentityName);

                var widgets = new List<WidgetModel>()
                {
                    new WidgetModel()
                    {
                        Name = "Exceptions",
                        Count = userStats.ExceptionCount,
                        Description = "Deliveries with short or damaged quantities",
                        SortOrder = 1,
                        Link = "/exceptions",
                        LinkText = "deliveries with exceptions",
                        WarningLevel = 10
                    },
                    new WidgetModel()
                    {
                        Name = "Assigned",
                        Count = userStats.AssignedCount,
                        Description = "Deliveries assigned to you for actioning",
                        SortOrder = 2,
                        Link = "/exceptions",
                        LinkText = "exceptions assigned to you",
                        WarningLevel = 10
                    },
                    new WidgetModel()
                    {
                        Name = "Outstanding",
                        Count = userStats.OutstandingCount,
                        Description = "Exceptions raised over 24 hours ago",
                        SortOrder = 3,
                        Link = "/exceptions",
                        LinkText = "outstanding exceptions",
                        WarningLevel = 10
                    },
                    new WidgetModel()
                    {
                        Name = "Notifications",
                        Count = userStats.NotificationsCount,
                        Description = "Unarchived notifications",
                        Link = "/notifications",
                        LinkText = "notifications",
                        SortOrder = 4,
                        WarningLevel = 10
                    }
                };
                return this.Request.CreateResponse(HttpStatusCode.OK, widgets.OrderBy(w => w.SortOrder));
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occcured when getting widgets");
            }
        }

    }
}
