namespace PH.Well.Api.Mapper
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Domain;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using PH.Well.Domain.Enums;
    using System.Linq;
    using Repositories.Contracts;

    public class LineItemExceptionMapper : ILineItemExceptionMapper
    {
        private readonly IJobRepository jobRepository;
        private readonly IAccountRepository accountRepository;

        public LineItemExceptionMapper(IJobRepository jobRepository, IAccountRepository accountRepository)
        {
            this.jobRepository = jobRepository;
            this.accountRepository = accountRepository;
        }

        public IEnumerable<EditLineItemException> Map(IEnumerable<LineItem> lineItems)
        {
            lineItems = lineItems.ToArray();
            var result = new List<EditLineItemException>();
            var jobs = jobRepository.GetByIds(lineItems.Select(x => x.JobId).Distinct()).ToArray();

            foreach (var line in lineItems)
            {
                var job = jobs.First(x => x.Id == line.JobId);
                var jobDetail = job.JobDetails.First(x => x.LineItemId == line.Id);
                result.Add(MapEditLineItemException(line, job, jobDetail));
            }

            return result;
        }

        public EditLineItemException Map(LineItem lineItem)
        {
            var job = jobRepository.GetById(lineItem.JobId);
            var jobDetail = job.JobDetails.First(x => x.LineItemId == lineItem.Id);
            return MapEditLineItemException(lineItem, job, jobDetail);
        }

        private EditLineItemException MapEditLineItemException(LineItem line, Job job, JobDetail jobDetail)
        {
            var editLineItemException = new EditLineItemException
            {
                AccountCode = job.PhAccount,
                JobId = job.Id,
                ResolutionStatus = job.ResolutionStatus?.Description,
                Id = line.Id,
                Invoice = job.InvoiceNumber,
                Type = job.JobType,
                ProductNumber = line.ProductCode,
                Product = line.ProductDescription,
                DriverReason = line.DriverReason,
                Value = (decimal)jobDetail.SkuGoodsValue,
                Invoiced = line.OriginalDespatchQuantity,
                Delivered = line.DeliveredQuantity,
                Damages = jobDetail.DamageQty,
                Shorts = jobDetail.ShortQty
            };
            editLineItemException.LineItemActions = line.LineItemActions;
            editLineItemException.Exceptions = line.LineItemActions
                  .Select(action => new EditLineItemExceptionDetail
                  {
                      Id = action.Id,
                      LineItemId = action.LineItemId,
                      Originator = action.Originator.Description(),
                      Exception = action.ExceptionType.Description(),
                      Action = EnumExtensions.GetDescription(action.DeliveryAction),
                      Quantity = action.Quantity,
                      Source = action.Source.Description(),
                      Reason = action.Reason.Description(),
                      Erdd = action.ReplanDate,
                      ActionedBy = action.ActionedBy,
                      ApprovedBy = action.ApprovedBy
                  })
                  .ToList();
            
            return editLineItemException;
        }
    }
}