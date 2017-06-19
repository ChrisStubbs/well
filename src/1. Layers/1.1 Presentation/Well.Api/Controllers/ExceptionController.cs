namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
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
        private readonly IJobRepository jobRepository;

        public ExceptionController(
            ILineItemActionService lineItemActionService,
            ILineItemSearchReadRepository lineItemSearchReadRepository,
            ILineItemExceptionMapper lineItemExceptionMapper,
            IJobRepository jobRepository)
        {
            this.lineItemActionService = lineItemActionService;
            this.lineItemSearchReadRepository = lineItemSearchReadRepository;
            this.lineItemExceptionMapper = lineItemExceptionMapper;
            this.jobRepository = jobRepository;
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
            var job = jobRepository.GetById(update.JobId);
            if (job == null)
            {
                throw new ArgumentException();
            }
            if (!job.CanEditActions)
            {
                throw new ArgumentException("Job is not in and editable state");
            }
            var lineItem = lineItemActionService.SaveLineItemActions(job, update.Id, update.LineItemActions);
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
