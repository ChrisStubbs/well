using PH.Well.Common.Contracts;

namespace PH.Well.Services.DeliveryActions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class DeliveryLineActionService : IDeliveryLineActionService
    {
        private readonly IAdamRepository adamRepository;
        private readonly IJobRepository jobRepository;
        private readonly IExceptionEventRepository eventRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;
        private readonly IJobService jobService;
        private readonly ILogger logger;

        public DeliveryLineActionService(
            IAdamRepository adamRepository,
            IJobRepository jobRepository,
            IExceptionEventRepository eventRepository,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IJobService jobService,
            ILogger logger)
        {
            this.adamRepository = adamRepository;
            this.jobRepository = jobRepository;
            this.eventRepository = eventRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.jobService = jobService;
            this.logger = logger;
        }

        // PLEASE NOTE There is no transaction scope in ADAM.  If a transaction of lines plus header fails 
        // partway, some lines from the transaction may have written to ADAM.  Therefore, each event must be 
        // marked as processed and if necessary, a new event containing only the data that still needs to be sent to ADAM,
        // inserted into the ExceptionEvent table 

        public void CreditTransaction(CreditTransaction creditTransaction, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.Credit(creditTransaction, adamSettings);
            MarkSuccessfulEventAsDone(eventId, adamResponse);
        }

        public void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.Grn(grnEvent, adamSettings);
            MarkSuccessfulEventAsDone(eventId, adamResponse);
        }

        public void Pod(PodEvent podEvent, int eventId, AdamSettings adamSettings)
        {
            var job = jobRepository.GetById(podEvent.Id);

            // Do not attempt to create a PodTransaction without an invoice number
            if (!string.IsNullOrEmpty(job?.InvoiceNumber))
            {
                job = jobService.PopulateLineItemsAndRoute(job);
                var adamResponse = this.adamRepository.Pod(podEvent, adamSettings, job);
                MarkSuccessfulEventAsDone(eventId, adamResponse);
            }
        }

        public void PodTransaction(PodTransaction podTransaction, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.PodTransaction(podTransaction, adamSettings);
            if (adamResponse == AdamResponse.Success)
            {
                this.MarkPodAsResolved(podTransaction.JobId, adamResponse);
            }
            MarkSuccessfulEventAsDone(eventId, adamResponse);
        }

        public void AmendmentTransaction(AmendmentTransaction amendmentTransaction, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.AmendmentTransaction(amendmentTransaction, adamSettings);
            this.MarkSuccessfulEventAsDone(eventId, adamResponse);
        }

        private void MarkSuccessfulEventAsDone(int eventId, AdamResponse response)
        {
            if (response == AdamResponse.Success)
            {
                this.eventRepository.MarkEventAsProcessed(eventId);
            }
        }

        private void MarkPodAsResolved(int jobId, AdamResponse response)
        {
            //TODO
            if (response == AdamResponse.Success)
            {
                using (var transactionScope = new TransactionScope())
                {
                    var job = jobRepository.GetById(jobId);
                    job.JobStatus = JobStatus.Resolved;
                    jobRepository.Update(job);

                    foreach (var jobDetail in job.JobDetails)
                    {
                        jobDetail.ShortsStatus = JobDetailStatus.Res;
                        jobDetailRepository.Update(jobDetail);

                        foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                        {
                            jobDetailDamage.DamageStatus = JobDetailStatus.Res;
                            jobDetailDamageRepository.Update(jobDetailDamage);
                        }
                    }
                    transactionScope.Complete();
                }
            }
        }
    }
}