using PH.Well.Domain.Extensions;

namespace PH.Well.Services.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;

    public class PatchSummaryMapper : IPatchSummaryMapper
    {
        public PatchSummary Map(IEnumerable<Job> jobs)
        {
            var summary = new PatchSummary();
            foreach (var job in jobs)
            {
                summary.Items.Add(Map(job, job.LineItems.ToArray()));
            }

            return summary;
        }

        public PatchSummary Map(IEnumerable<Job> jobs, IEnumerable<int> lineItemIds)
        {
            var summary = new PatchSummary();
            foreach (var job in jobs)
            {
                summary.Items.Add(Map(job, job.LineItems.Where(x => lineItemIds.Contains(x.Id)).ToArray()));
            }

            return summary;
        }

        public PatchSummaryItem Map(Job job, LineItem[] lineItems)
        {
            var item = new PatchSummaryItem
            {
                JobId = job.Id,
                Invoice = job.InvoiceNumber,
                Type =job.JobTypeDisplayText,
                Account = job.PhAccount,
                ShortQuantity = lineItems.Sum(li => li.TotalShortQty),
                BypassQuantity = lineItems.Sum(li => li.TotalBypassQty),
                DamageQuantity = lineItems.Sum(li => li.TotalDamageQty),
                TotalExceptionValue = lineItems.Sum(li => li.TotalActionValue),
                TotalDispatchedQuantity = job.JobDetails.Sum(jd=> jd.OriginalDespatchQty),
                TotalDispatchedValue = job.JobDetails.Sum(jd=> jd.OriginalDespatchQty * jd.NetPrice??0)
            };
            return item;
        }
    }
}