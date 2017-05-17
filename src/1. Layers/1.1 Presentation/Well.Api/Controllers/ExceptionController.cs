using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    using System.Linq;
    using Domain.ValueObjects;
    using Mapper.Contracts;
    using Repositories;
    using Repositories.Contracts;

    public class ExceptionController : ApiController
    {
        private readonly ILineItemSearchReadRepository lineItemSearchReadRepository;
        private readonly ILineItemExceptionMapper lineItemExceptionMapper;

        public ExceptionController(ILineItemSearchReadRepository lineItemSearchReadRepository, ILineItemExceptionMapper lineItemExceptionMapper)
        {
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

    }
}
