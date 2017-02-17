using PH.Well.Domain;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PH.Well.Services.Contracts;

    public class DeliveryStatusService : IDeliveryStatusService
    {
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;

        public DeliveryStatusService(IJobRepository jobRepository, IJobDetailRepository jobDetailRepository)
        {
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
        }

        public void SetStatus(Job job, int branchId)
        {
            // Fetch all jobs associated with the current job's invoice and branch
            var jobs = this.jobRepository.GetJobsByBranchAndInvoiceNumber(branchId, job.InvoiceNumber);

            if (jobs.Any())
            {
                // Any shorts are an exception

                // Any damages are an exception

                // If the delivered QTY > the invoiced QTY is an exception
                var products = jobs.SelectMany(x => x.JobDetails).GroupBy(x=>x.PhProductCode);
                foreach (var product in products)
                {
                    var originalDespatchQty = product.First().OriginalDespatchQty;
                    var deliveryedQty = product.Sum(x => x.DeliveredQty);
                    if (deliveryedQty > originalDespatchQty)
                    {
                        
                    }
                }
            }
        }
    }
}
