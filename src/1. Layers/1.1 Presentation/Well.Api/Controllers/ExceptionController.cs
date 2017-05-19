namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Domain;
    using Domain.ValueObjects;
    using Mapper.Contracts;
    using Repositories.Contracts;
    using Services.Contracts;

    public class ExceptionController : ApiController
    {
        private readonly ILineItemActionService lineItemActionService;
        private readonly ILineItemSearchReadRepository lineItemSearchReadRepository;
        private readonly ILineItemExceptionMapper lineItemExceptionMapper;

        public ExceptionController(ILineItemActionService lineItemActionService, ILineItemSearchReadRepository lineItemSearchReadRepository, ILineItemExceptionMapper lineItemExceptionMapper)
        {
            this.lineItemActionService = lineItemActionService;
            this.lineItemSearchReadRepository = lineItemSearchReadRepository;
            this.lineItemExceptionMapper = lineItemExceptionMapper;
        }

        [HttpGet]
        public IList<EditLineItemException> PerLineItem([FromUri]int[] id)
        {
            var lineItems = this.lineItemSearchReadRepository.GetLineItemByIds(id);
            var result = this.lineItemExceptionMapper.Map(lineItems).ToList();
            return result;
        }

        public IList<EditLineItemException> Post(LineItemActionUpdate update)
        {
            IList<EditLineItemException> exceptions = null;

            if (update != null)
            {
                var lineItem = lineItemActionService.InsertLineItemActions(update);
                exceptions = lineItemExceptionMapper.Map(new[] { lineItem }).ToList();
            }
            return exceptions;
        }

        public IList<EditLineItemException> Put(LineItemActionUpdate update)
        {
            IList<EditLineItemException> exceptions = null;

            if (update != null)
            {
                var lineItem = lineItemActionService.UpdateLineItemActions(update);
                exceptions = lineItemExceptionMapper.Map(new[] { lineItem }).ToList();
            }
            return exceptions;
        }

    }
}
