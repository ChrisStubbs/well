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
    using Services.Contracts;
    using Common.Contracts;

    public class LineItemExceptionMapper : ILineItemExceptionMapper
    {
        private readonly IJobRepository jobRepository;
        private readonly IJobService jobService;
        private readonly IUserNameProvider userNameProvider;

        public LineItemExceptionMapper(IJobRepository jobRepository,
            IJobService jobService,
            IUserNameProvider userNameProvider)
        {
            this.jobRepository = jobRepository;
            this.jobService = jobService;
            this.userNameProvider = userNameProvider;
        }

        public IEnumerable<EditLineItemException> Map(IEnumerable<LineItem> lineItems)
        {
            lineItems = lineItems.ToArray();
            var result = new List<EditLineItemException>();
            var jobs = jobRepository.GetByIds(lineItems.Select(x => x.JobId).Distinct()).ToArray();
            var jobDetailLineItemTotals = jobRepository.JobDetailTotalsPerJobs(jobs.Select(x => x.Id)).ToArray();

            foreach (var line in lineItems)
            {
                var job = jobs.First(x => x.Id == line.JobId);
                var jobDetail = job.JobDetails.First(x => x.LineItemId == line.Id);
                result.Add(MapEditLineItemException(line, job, jobDetail, jobDetailLineItemTotals));
            }

            return result;
        }

        public EditLineItemException Map(LineItem lineItem)
        {
            var job = jobRepository.GetById(lineItem.JobId);
            var jobDetail = job.JobDetails.First(x => x.LineItemId == lineItem.Id);
            var jobDetailLineItemTotals = jobRepository.JobDetailTotalsPerJobs(new[] { job.Id }).ToArray();
            return MapEditLineItemException(lineItem, job, jobDetail, jobDetailLineItemTotals);
        }

        private EditLineItemException MapEditLineItemException(LineItem line, Job job, JobDetail jobDetail, JobDetailLineItemTotals[] jobDetailLineItemTotals)
        {
            var totals = jobDetailLineItemTotals.SingleOrDefault(x => x.JobDetailId == jobDetail.Id);
            
            var editLineItemException = new EditLineItemException
            {
                AccountCode = job.PhAccount,
                JobId = job.Id,
                ResolutionId = job.ResolutionStatus.Value, 
                ResolutionStatus = job.ResolutionStatus?.Description,
                Id = line.Id,
                Invoice = job.InvoiceNumber,
                Type = job.JobType,
                ProductNumber = line.ProductCode,
                Product = line.ProductDescription,
                DriverReason = line.DriverReason,
                Value = jobDetail.NetPrice ?? 0,
                Invoiced = jobDetail.OriginalDespatchQty,
                Damages = totals?.DamageTotal ?? jobDetail.DamageQty,
                Shorts = totals?.ShortTotal ?? jobDetail.ShortQty,
                CanEditActions = jobService.CanEditActions(job, this.userNameProvider.GetUserName())
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