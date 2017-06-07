namespace PH.Well.Services.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class ActionSummaryMapper : IActionSummaryMapper
    {
        private readonly IUserThresholdService userThresholdService;
     

        public ActionSummaryMapper(IUserThresholdService userThresholdService)
        {
            this.userThresholdService = userThresholdService;
        }

        public ActionSubmitSummary Map(SubmitActionModel submitAction, bool isStopLevel)
        {
            var result = new ActionSubmitSummary();
            var unsubmittedItemsForJob = submitAction.ItemsToSubmit.ToArray();

            if (unsubmittedItemsForJob.Any())
            {
                result.JobIds.AddRange(unsubmittedItemsForJob.Select(x => x.JobId).Distinct());

                if (isStopLevel)
                {
                    foreach (var stop in unsubmittedItemsForJob.Select(x => x.Stop).Distinct())
                    {
                        result.Summary = $"The total to be credited for the selection is £{unsubmittedItemsForJob.Sum(x => x.TotalValue)}. " +
                                         $"The maximum you are allowed to credit is £{userThresholdService.GetUserCreditThresholdValue()}, " +
                                         $"any credits over your assigned threshold will be sent for approval.";
                            
                        var stopItems = unsubmittedItemsForJob.Where(x => x.Stop == stop).ToArray();

                        result.Items.Add(new ActionSubmitSummaryItem
                        {
                            Identifier = stopItems.First().Stop,
                            NoOfItems = stopItems.Select(x => x.InvoiceNumber).Distinct().Count(),
                            Qty = stopItems.Sum(x => x.Quantity),
                            Value = stopItems.Sum(x => x.TotalValue)
                        });
                    }
                }
                else
                {
                    foreach (var invoiceNumber in unsubmittedItemsForJob.Select(x => x.InvoiceNumber).Distinct())
                    {
                        var invoiceItems = unsubmittedItemsForJob.Where(x => x.InvoiceNumber == invoiceNumber).ToArray();

                        result.Summary = $"The total no of actions to be closed is {unsubmittedItemsForJob.Length}.";

                        result.Items.Add(new ActionSubmitSummaryItem
                        {
                            Identifier = invoiceItems.First().InvoiceNumber,
                            NoOfItems = invoiceItems.Length,
                            Value = invoiceItems.Sum(x => x.TotalValue),
                            Qty = invoiceItems.Sum(x => x.Quantity),
                        });
                    }

                }
            }

            return result;
        }

    }
}