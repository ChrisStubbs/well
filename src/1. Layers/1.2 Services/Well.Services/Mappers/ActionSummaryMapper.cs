namespace PH.Well.Services.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class ActionSummaryMapper : IActionSummaryMapper
    {
        private readonly IUserThresholdService userThresholdService;
        private readonly IStopRepository stopRepository;


        public ActionSummaryMapper(IUserThresholdService userThresholdService,
            IStopRepository stopRepository)
        {
            this.userThresholdService = userThresholdService;
            this.stopRepository = stopRepository;
        }

        public ActionSubmitSummary Map(SubmitActionModel submitAction, bool isStopLevel, IList<Job> jobs)
        {

            var result = new ActionSubmitSummary();

            if (jobs.Any())
            {
                result.JobIds.AddRange(jobs.Select(x => x.Id).Distinct());

                result.Summary = GetSummary(jobs);
                if (isStopLevel)
                {
                    var stopIds = jobs.Select(x => x.StopId).Distinct();

                    foreach (var stopId in stopIds)
                    {
                        var stop = stopRepository.GetById(stopId);

                        result.Items.Add(new ActionSubmitSummaryItem
                        {
                            Identifier = stop.PlannedStopNumber,
                            TotalCreditValue = jobs.Sum(x => x.TotalCreditValue),
                            TotalActionValue = jobs.Sum(x => x.TotalActionValue),
                            TotalCreditQty = jobs.Sum(x => x.TotalCreditQty),
                            TotalQty = jobs.Sum(x => x.TotalQty),
                        });
                    }
                }
                else
                {
                    foreach (var invoiceNumber in jobs.Select(x => x.InvoiceNumber).Distinct())
                    {
                        var invoiceItems = jobs.Where(x => x.InvoiceNumber == invoiceNumber).ToArray();

                        var firstItem = invoiceItems.First();
                        result.Items.Add(new ActionSubmitSummaryItem
                        {
                            Identifier = firstItem.InvoiceNumber,
                            JobType = firstItem.JobType,
                            TotalCreditValue = invoiceItems.Sum(x => x.TotalCreditValue),
                            TotalActionValue = invoiceItems.Sum(x => x.TotalActionValue),
                            TotalCreditQty = invoiceItems.Sum(x => x.TotalCreditQty),
                            TotalQty = invoiceItems.Sum(x => x.TotalQty),
                        });
                    }

                }
            }

            return result;
        }

        private string GetSummary(IList<Job> jobs)
        {
            var totalToCredit = jobs.SelectMany(x => x.LineItems).Sum(x => x.TotalCreditValue);


            if (totalToCredit > 0)
            {
                return $"The total to be credited for the selection is £{totalToCredit}. " +
                               $"The maximum you are allowed to credit is £{userThresholdService.GetUserCreditThresholdValue()}, " +
                               $"any credits over your assigned threshold will be sent for approval.";
            }

            return $"The quantity of of items to 'submit' is {jobs.SelectMany(x => x.GetAllLineItemActions()).Sum(x => x.Quantity)}.";

        }
    }
}