namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Domain.Extensions;
    using Infrastructure;
    using Models;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.ValueObjects;
    using Services.Contracts;

    public class NotificationController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly INotificationService notificationService;

        private readonly IConnectionStringFactory connectionsStrings;

        public NotificationController(ILogger logger, INotificationService notificationService,
            IUserNameProvider userNameProvider,
            IConnectionStringFactory connectionsStrings)
            : base(userNameProvider)
        {
            this.logger = logger;
            this.notificationService = notificationService;

            this.connectionsStrings = connectionsStrings;
        }


        [Route("notification")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var notifications = this.notificationService.GetNotificationsAllDatabases();
            return !notifications.Any()
               ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                : this.Request.CreateResponse(HttpStatusCode.OK, notifications);
        }

        // AllowAnonymous to let ADAM post errors - do not remove
        [AllowAnonymous]
        [Route("notification/adamerror")]
        [HttpPost]
        public HttpResponseMessage Post(AdamFail failure)
        {
            try
            {
                if (failure.JobId > 0)
                {
                    var notification = failure.ToNotification();
                    string connectionString = connectionsStrings.GetConnectionString(int.Parse(notification.Branch), ConnectionType.Dapper);
                    notificationService.SaveNotification(notification, connectionString);
                    return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save ADAM failure notification ", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }

        [HttpPut]
        [Route("{branchId:int}/notification/archive/{id:int}")]
        public HttpResponseMessage Archive(int id)
        {
            try
            {
                if (id > 0)
                {
                  
                    this.notificationService.ArchiveNotification(id);
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to archive notification ", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }

        }

    }

}