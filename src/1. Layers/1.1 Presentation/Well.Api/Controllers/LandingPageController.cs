namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Repositories.Contracts;

    public class LandingPageController : ApiController
    {

        private readonly ILogger logger;

        private readonly IWidgetStatsRepository widgetStatsRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public LandingPageController(ILogger logger, IWidgetStatsRepository widgetStatsRepository, IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.widgetStatsRepository = widgetStatsRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("getwidgetstats", Name = "GetWidgetStats")]
        [HttpGet]
        public HttpResponseMessage GetWidgetStats()
        {
            try
            {
                var widgetStats = this.widgetStatsRepository.GetWidgetStats();
                return this.Request.CreateResponse(HttpStatusCode.OK, widgetStats);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting widget stats", ex);
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

    }
}
