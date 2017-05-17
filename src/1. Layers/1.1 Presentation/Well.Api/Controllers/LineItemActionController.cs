namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Domain;
    using Domain.ValueObjects;
    using Services.Contracts;

    public class LineItemActionController : ApiController
    {
        private readonly ILineItemActionService lineItemActionService;

        public LineItemActionController(ILineItemActionService lineItemActionService)
        {
            this.lineItemActionService = lineItemActionService;
        }

        public HttpResponseMessage Post(LineItemActionUpdate update)
        {
            LineItemAction item = null;

            if (update != null)
            {
                item = lineItemActionService.InsertLineItemActions(update);
            }
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }

        public HttpResponseMessage Put(LineItemActionUpdate update)
        {
            LineItemAction item = null;

            if (update != null)
            {
                item = lineItemActionService.UpdateLineItemActions(update);
            }
            return Request.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}