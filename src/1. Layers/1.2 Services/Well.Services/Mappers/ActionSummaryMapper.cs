namespace PH.Well.Services.Mappers
{
    using System.Linq;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;

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


                result.Summary = GetSummary(submitAction, unsubmittedItemsForJob);
                if (isStopLevel)
                {
                    foreach (var stop in unsubmittedItemsForJob.Select(x => x.Stop).Distinct())
                    {

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

        private string GetSummary(SubmitActionModel submitAction, LineItemActionSubmitModel[] unsubmittedItemsForJob)
        {
            if (submitAction.Action == DeliveryAction.Credit)
            {
                return $"The total to be credited for the selection is £{unsubmittedItemsForJob.Sum(x => x.TotalValue)}. " +
                       $"The maximum you are allowed to credit is £{userThresholdService.GetUserCreditThresholdValue()}, " +
                       $"any credits over your assigned threshold will be sent for approval.";
            }

            return $"The total no of items to '{submitAction.Action}' is {unsubmittedItemsForJob.Length}.";

        }
    }
}