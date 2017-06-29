namespace PH.Well.Services.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;

    public class BulkEditSummaryMapper : IBulkEditSummaryMapper
    {
        public BulkEditSummary Map(IEnumerable<Job> jobs)
        {
            var summary = new BulkEditSummary();
            foreach (var job in jobs)
            {
                summary.Items.Add(Map(job, job.LineItems.ToArray()));
            }

            return summary;
        }

        public BulkEditSummary Map(IEnumerable<Job> jobs, IEnumerable<int> lineItemIds)
        {
            var summary = new BulkEditSummary();
            foreach (var job in jobs)
            {
                summary.Items.Add(Map(job, job.LineItems.Where(x => lineItemIds.Contains(x.Id)).ToArray()));
            }

            return summary;
        }

        public BulkEditItem Map(Job job, LineItem[] lineItems)
        {
            var item = new BulkEditItem
            {
                JobId = job.Id,
                Invoice = job.InvoiceNumber,
                Type = job.JobType,
                Account = job.PhAccount,
                ShortQuantity = lineItems.Sum(li => li.TotalShortQty),
                BypassQuantity = lineItems.Sum(li => li.TotalBypassQty),
                DamageQuantity = lineItems.Sum(li => li.TotalDamageQty),
                TotalValue = lineItems.Sum(li => li.TotalActionValue)
            };
            return item;
        }
    }
}