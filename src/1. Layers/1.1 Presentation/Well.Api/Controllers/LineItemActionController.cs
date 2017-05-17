namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using Repositories;
    using Repositories.Contracts;

    public class LineItemActionController : ApiController
    {
        private readonly ILineItemActionRepository lineItemActionRepository;

        public LineItemActionController(ILineItemActionRepository lineItemActionRepository)
        {
            this.lineItemActionRepository = lineItemActionRepository;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] LineItemActionUpdate update)
        {
            if (update != null)
            {
                var action = new LineItemAction
                {
                    ExceptionType = update.ExceptionType,
                    Quantity = update.Quantity,
                    LineItemId = update.LineItemId,
                    Originator = "Customer" //will 


                };

                this.lineItemActionRepository.Save(action);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        

    }
}