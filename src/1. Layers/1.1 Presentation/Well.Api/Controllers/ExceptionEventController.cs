using PH.Well.Domain;
using PH.Well.Repositories;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    public class ExceptionEventController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly IExceptionEventService exceptionEventService;

        private readonly INotificationRepository notificationRepository;

        public ExceptionEventController(ILogger logger, IExceptionEventService exceptionEventService, INotificationRepository notificationRepository)
        {
            this.logger = logger;
            this.exceptionEventService = exceptionEventService;
            this.notificationRepository = notificationRepository;
        }

        // commented out to allow notifications to be generated for demo
        //[Route("credit")]
        //[HttpPost]
        //public HttpResponseMessage Credit(CreditEvent creditEvent)
        //{
        //    try
        //    {
        //        var settings = AdamSettingsFactory.GetAdamSettings((Branch)creditEvent.BranchId);

        //        var response = this.exceptionEventService.Credit(creditEvent, settings, this.UserIdentityName);

        //        if (response == AdamResponse.AdamDown) return this.Request.CreateResponse(HttpStatusCode.OK, new { adamdown = true });

        //        return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        //    }
        //    catch (Exception exception)
        //    {
        //        this.logger.LogError("Error on credit event", exception);
        //        return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
        //    }
        //}

        // credit action pointed to credit fail endpoint to generate notifications for demo
        [Route("credit")]
        [HttpPost]
        public HttpResponseMessage Post(CreditFail credit)
        {
            credit.JobId = credit.Id;
            credit.Reason = "Failed ADAM validation";
            try
            {
                if (credit.JobId > 0)
                {
                    var notification = new Notification
                    {
                        JobId = credit.JobId,
                        Reason = credit.Reason,
                        Type = (int)NotificationType.Credit,
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
    }
}