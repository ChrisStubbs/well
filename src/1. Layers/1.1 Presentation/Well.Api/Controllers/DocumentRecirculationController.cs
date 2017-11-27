namespace PH.Well.Api.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using Services.Contracts;

    public class DocumentRecirculationController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly IExceptionEventRepository exceptionEventRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        private readonly IDocumentRecirculationFactory documentRecirculationFactory;

        public DocumentRecirculationController(ILogger logger, IExceptionEventRepository exceptionEventRepository,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IDocumentRecirculationFactory documentRecirculationFactory,
            IUserNameProvider userNameProvider)
            : base(userNameProvider)
        {
            this.logger = logger;
            this.exceptionEventRepository = exceptionEventRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.documentRecirculationFactory = documentRecirculationFactory;
        }


        [Route("documents")]
        [HttpPost]
        public HttpResponseMessage Post(DateTime routeDeliveryDate, int routeNumber, int stopNumber, int stopId, int branchId)
        {
            try
            {
                var transaction = this.documentRecirculationFactory.Build(routeDeliveryDate, routeNumber, stopNumber, stopId, branchId);

                this.exceptionEventRepository.InsertRecirculationDocEventTransaction(transaction);
                return this.Request.CreateResponse(HttpStatusCode.Created, new {success = true});

            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save recirculation event ", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new {failure = true});
            }
        }

    }

}