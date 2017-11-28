namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Mapper.Contracts;
    using Models;
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
        private readonly INotificationRepository notificationRepository;

        public WidgetController(IServerErrorResponseHandler serverErrorResponseHandler,
            ILogger logger,
            IUserStatsRepository userStatsRepository,
            IWidgetRepository widgetRepository,
            IWidgetWarningMapper mapper,
            IWidgetWarningValidator validator,
            IUserNameProvider userNameProvider,
            INotificationRepository notificationRepository)
            : base(userNameProvider)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.logger = logger;
            this.userStatsRepository = userStatsRepository;
            this.widgetRepository = widgetRepository;
            this.mapper = mapper;
            this.validator = validator;
            this.notificationRepository = notificationRepository;
        }

        [Route("{branchId:int}/widgets")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            throw new NotImplementedException();
            //try
            //{
            //    var warningLevels = this.userStatsRepository.GetWidgetWarningLevels(UserIdentityName);
            //    var notifications = notificationRepository.GetNotifications();

            //    var widgets = new List<WidgetModel>()
            //    {
            //        new WidgetModel()
            //        {
            //            Name = "Exceptions",
            //            Description = "Deliveries with short or damaged quantities",
            //            SortOrder = 2,
            //            WarningLevel = warningLevels.ExceptionWarningLevel ?? 50,
            //            ShowOnGraph = true,
            //            Links = new List<WidgetLinkModel>()
            //            {
            //                new WidgetLinkModel()
            //                {
            //                    Count = exceptionDeliveries.Count(),
            //                    CountName = "unsubmitted-exceptions",
            //                    Link = "/exceptions",
            //                    LinkText = "unsubmitted"

            //                },
            //                new WidgetLinkModel()
            //                {
            //                    Count = approvalDeliveries.Count(),
            //                    CountName = "approval-exceptions",
            //                    Link = "/approvals",
            //                    LinkText = "pending approval"
            //                }
            //            }
            //        },
            //        new WidgetModel()
            //        {
            //            Name = "Assigned",
            //            Description = "Exceptions assigned to you for actioning",
            //            SortOrder = 3,
            //            WarningLevel = warningLevels.AssignedWarningLevel ?? 50,
            //            ShowOnGraph = true,
            //            Links = new List<WidgetLinkModel>()
            //            {
            //                new WidgetLinkModel()
            //                {
            //                    Count = exceptionDeliveries.Count(d => d.IsAssignedTo(UserIdentityName)),
            //                    CountName = "my-unsubmitted-exceptions",
            //                    Link = "/exceptions",
            //                    LinkText = "assigned unsubmitted"
            //                },
            //                new WidgetLinkModel()
            //                {
            //                    Count = approvalDeliveries.Count(d => d.IsAssignedTo(UserIdentityName)),
            //                    CountName = "my-approval-exceptions",
            //                    Link = "/approvals",
            //                    LinkText = "assigned pending approval"
            //                }
            //            }
            //        },
            //        new WidgetModel()
            //        {
            //            Name = "Outstanding",
            //            Description = "Exceptions raised over 24 hours ago",
            //            SortOrder = 4,
            //            WarningLevel = warningLevels.OutstandingWarningLevel ?? 50,
            //            ShowOnGraph = true,
            //            Links = new List<WidgetLinkModel>()
            //            {
            //                new WidgetLinkModel()
            //                {
            //                    Count = exceptionDeliveries.Count(d => d.IsOutstanding),
            //                    CountName = "outstanding-unsubmitted-exceptions",
            //                    Link = "/exceptions",
            //                    LinkText = "outstanding unsubmitted"
            //                },
            //                new WidgetLinkModel()
            //                {
            //                    Count = approvalDeliveries.Count(d => d.IsOutstanding),
            //                    CountName = "outstanding-approval-exceptions",
            //                    Link = "/approvals",
            //                    LinkText = "outstanding pending approval"
            //                }
            //            }
            //        },
            //        new WidgetModel()
            //        {
            //            Name = "Notifications",
            //            Description = "Unarchived notifications",
            //            SortOrder = 5,
            //            WarningLevel = warningLevels.NotificationsWarningLevel ?? 50,
            //            ShowOnGraph = true,
            //            Links = new List<WidgetLinkModel>()
            //            {
            //                new WidgetLinkModel()
            //                {
            //                    Count = notifications.Count(),
            //                    CountName = "notifications",
            //                    Link = "/notifications",
            //                    LinkText = "notifications"
            //                }
            //            }
            //        }
            //    };
            //    return this.Request.CreateResponse(HttpStatusCode.OK, widgets.OrderBy(w => w.SortOrder));
            //}
            //catch (Exception ex)
            //{
            //    return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting widgets");
            //}
        }

        [Route("{branchId:int}/widgetsWarnings")]
        [HttpGet]
        public HttpResponseMessage GetWarnings()
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

        [Route("{branchId:int}/widgetWarning/{isUpdate:bool}")]
        [HttpPost]
        public HttpResponseMessage Post(WidgetWarningModel model, bool isUpdate)
        {
            if (!this.validator.IsValid(model, isUpdate))
            {
                return this.Request.CreateResponse(HttpStatusCode.OK,
                    new { notAcceptable = true, errors = this.validator.Errors.ToArray() });
            }

            var widgetWarning = this.mapper.Map(model);

            this.widgetRepository.Save(widgetWarning);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }

        [Route("{branchId:int}/widgetWarning/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            this.widgetRepository.Delete(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }
    }
}

