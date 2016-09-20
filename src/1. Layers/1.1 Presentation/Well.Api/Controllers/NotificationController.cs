namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    public class NotificationController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly INotificationRepository notificationRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public NotificationController(ILogger logger, INotificationRepository notificationRepository,
            IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.notificationRepository = notificationRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("notification")]
        [HttpGet]
        public HttpResponseMessage Get()
        {

            try
            {
                var notifications = this.notificationRepository.GetNotifications().ToList();
                return !notifications.Any()
                   ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, notifications);
            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting notifications");
            }

        }

        [Route("notification/credit")]
        [HttpPost]
        public HttpResponseMessage Post(CreditFail credit)
        {
            try
            {
                if (credit.JobId > 0)
                {
                    var notification = new Notification
                    {
                        JobId = credit.JobId,
                        Reason = credit.Reason, 
                        Type = (int) NotificationType.Credit,
                        Source = "ADAMCSS"
                    };

                    this.notificationRepository.SaveNotification(notification);
                    return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save credit failure notification ", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }

        [Route("notification/archive/{id:int}")]
        [HttpPut]
        public HttpResponseMessage Archive(int id)
        {
            try
            {
                if (id > 0)
                {
                    this.notificationRepository.CurrentUser = this.UserIdentityName;
                    this.notificationRepository.ArchiveNotification(id);
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