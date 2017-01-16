namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Domain;
    using Mapper.Contracts;
    using Models;

    using PH.Well.Common.Security;
    using Repositories.Contracts;
    using Validators.Contracts;

    public class WidgetController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IUserStatsRepository userStatsRepository;
        private readonly ILogger logger;
        private readonly IWidgetRepository widgetRepository;
        private readonly IWidgetWarningMapper mapper;
        private readonly IWidgetWarningValidator validator;

        public WidgetController(IServerErrorResponseHandler serverErrorResponseHandler,
            IUserStatsRepository userStatsRepository,
            ILogger logger,
            IWidgetRepository widgetRepository,
            IWidgetWarningMapper mapper,
            IWidgetWarningValidator validator
            )
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.userStatsRepository = userStatsRepository;
            this.logger = logger;
            this.widgetRepository = widgetRepository;
            this.mapper = mapper;
            this.validator = validator;

            this.widgetRepository.CurrentUser = this.UserIdentityName;
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
                var warningLevels = this.userStatsRepository.GetWidgetWarningLevels(UserIdentityName);

                var widgets = new List<WidgetModel>()
                {
                    new WidgetModel()
                    {
                        Name = "Pending Authorisation",
                        Count = pendingCreditCount,
                        Description = "Credits awaiting your authorisation",
                        SortOrder = 1,
                        Link = "/pending-credit",
                        LinkText = "pending credits",
                        WarningLevel = 50,
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
                        WarningLevel = warningLevels.ExceptionWarningLevel ?? 50,
                        ShowOnGraph = true
                    },
                    new WidgetModel()
                    {
                        Name = "Assigned",
                        Count = userStats.AssignedCount,
                        Description = "Deliveries assigned to you for actioning",
                        SortOrder = 3,
                        Link = "/exceptions",
                        LinkText = "deliveries assigned to you",
                        WarningLevel = warningLevels.AssignedWarningLevel ?? 50,
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
                        WarningLevel = warningLevels.OutstandingWarningLevel ?? 50,
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
                        WarningLevel = warningLevels.NotificationsWarningLevel ?? 50,
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
        [Route("widgetsWarnings")]
        [HttpGet]
        public HttpResponseMessage GetWarnings()
        {
            try
            {
                var widgetWarnings = this.widgetRepository.GetAll().OrderBy(x => x.WarningLevel).ToList();
                var model = new List<WidgetWarningModel>();

                foreach (var warning in widgetWarnings)
                {
                    var mappedModel = this.mapper.Map(warning);

                    model.Add(mappedModel);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex,
                    "An error occurred when getting widget warnings");
            }
        }

        [Route("widgetWarning/{isUpdate:bool}")]
        [HttpPost]
        public HttpResponseMessage Post(WidgetWarningModel model, bool isUpdate)
        {
            try
            {
                if (!this.validator.IsValid(model, isUpdate))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK,
                        new { notAcceptable = true, errors = this.validator.Errors.ToArray() });
                }

                var widgetWarning = this.mapper.Map(model);

                this.widgetRepository.Save(widgetWarning);

                return this.Request.CreateResponse(HttpStatusCode.OK, new {success = true});
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save widget warning", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new {failure = true});
            }
        }


        [Route("widgetWarning/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                this.widgetRepository.Delete(id);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to delete widget warning(id):{id}", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }
    }
}

