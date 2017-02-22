namespace PH.Well.Services
{
    using System.Linq;
    using System.Transactions;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class DeliveryStatusService : IDeliveryStatusService
    {
        private readonly IJobRepository jobRepository;

        public DeliveryStatusService(IJobRepository jobRepository)
        {
            this.jobRepository = jobRepository;

            //////this.jobRepository.CurrentUser = Thread.CurrentPrincipal.Identity.Name;
        }

        public void SetStatus(Job job, int branchId)
        {
            // Fetch all jobs associated with the current job's invoice and branch
            var jobs = this.jobRepository.GetJobsByBranchAndInvoiceNumber(branchId, job.InvoiceNumber).ToList();

            bool hasException = false;
            if (jobs.Any())
            {
                // If the delivered QTY > the invoiced QTY is an exception
                var products = jobs.SelectMany(x => x.JobDetails).GroupBy(x => x.PhProductCode);
                foreach (var product in products)
                {
                    // The original QTY is duplicated on all deliveries, so take the first one
                    var originalDespatchQty = product.First().OriginalDespatchQty;

                    // Delivered quantity of this product across all deliveries
                    var deliveryedQty = product.Sum(x => x.DeliveredQty);
                    if (deliveryedQty > originalDespatchQty)
                    {
                        hasException = true;
                        break;
                    }
                }
                // Set all jobs to have an exceptions
                if (hasException)
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        jobs.ForEach(x =>
                        {
                            x.HasException = true;
                            this.jobRepository.Update(x);
                        });
                        transactionScope.Complete();
                    }
                    
                    job.HasException = true;
                }
            }
            // Any damages are an exception or any shorts are an exception
            if (!hasException && (job.JobDetails.Any(x => x.JobDetailDamages.Any()) || job.JobDetails.Any(x => x.ShortQty > 0)))
            {
                job.HasException = true;
            }
        }
    }
}
