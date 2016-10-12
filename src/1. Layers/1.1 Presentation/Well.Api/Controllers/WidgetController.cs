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
                var pendingCreditCount = this.userStatsRepository.GetPendingCreditCountByUser(UserIdentityName);

                var widgets = new List<WidgetModel>()
                {
                    new WidgetModel()
                    {
                        Name = "Pending Authorisation",
                        Count = pendingCreditCount,
                        Description = "Credit's awaiting your authorisation",
                        SortOrder = 1,
                        Link = "/pending-credit",
                        LinkText = "pending credits",
                        WarningLevel = 100,
                        ShowOnGraph = false
                    },
                    new WidgetModel()
                    {
                        Name = "Exceptions",
                        Count = userStats.ExceptionCount,
                        Description = "Deliveries with short or damaged quantities",
                        SortOrder = 2,
                        Link = "/exceptions",
                        LinkText = "deliveries with exceptions",
                        WarningLevel = 10,
                        ShowOnGraph = true
                    },
                    new WidgetModel()
                    {
                        Name = "Assigned",
                        Count = userStats.AssignedCount,
                        Description = "Deliveries assigned to you for actioning",
                        SortOrder = 3,
                        Link = "/exceptions",
                        LinkText = "exceptions assigned to you",
                        WarningLevel = 10,
                        ShowOnGraph = true
                    },
                    new WidgetModel()
                    {
                        Name = "Outstanding",
                        Count = userStats.OutstandingCount,
                        Description = "Exceptions raised over 24 hours ago",
                        SortOrder = 4,
                        Link = "/exceptions",
                        LinkText = "outstanding exceptions",
                        WarningLevel = 10,
                        ShowOnGraph = true
                    },
                    new WidgetModel()
                    {
                        Name = "Notifications",
                        Count = userStats.NotificationsCount,
                        Description = "Unarchived notifications",
                        Link = "/notifications",
                        LinkText = "notifications",
                        SortOrder = 5,
                        WarningLevel = 10,
                        ShowOnGraph = true
                    }
                };
                return this.Request.CreateResponse(HttpStatusCode.OK, widgets.OrderBy(w => w.SortOrder));
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting widgets");
            }
        }
    }
}
