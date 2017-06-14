namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
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

        public EditLineItemException Patch(EditLineItemException update)
        {
            var lineItem = lineItemActionService.SaveLineItemActions(update.Id, update.LineItemActions);
            return lineItem != null ? lineItemExceptionMapper.Map(lineItem) : null;
        }

        public EditLineItemException Post(LineItemActionUpdate update)
        {
            return lineItemExceptionMapper.Map(new[] { lineItemActionService.InsertLineItemActions(update) }).First();
        }

        public EditLineItemException Put(LineItemActionUpdate update)
        {
            return lineItemExceptionMapper.Map(new[] { lineItemActionService.UpdateLineItemActions(update) }).First();
        }


    }
}
