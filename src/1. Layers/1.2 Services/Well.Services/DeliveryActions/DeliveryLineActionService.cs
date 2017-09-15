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
        private readonly IEnumerable<IDeliveryLinesAction> actionHandlers;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;
        private readonly IJobService jobService;

        public DeliveryLineActionService(
            IAdamRepository adamRepository,
            IJobRepository jobRepository,
            IExceptionEventRepository eventRepository,
            IEnumerable<IDeliveryLinesAction> actionHandlers,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IJobService jobService)
        {
            this.adamRepository = adamRepository;
            this.jobRepository = jobRepository;
            this.eventRepository = eventRepository;
            this.actionHandlers = actionHandlers;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.jobService = jobService;
        }

        public void CreditTransaction(CreditTransaction creditTransaction, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.Credit(creditTransaction, adamSettings);

            this.MarkAsDone(eventId, adamResponse);
        }

        public void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.Grn(grnEvent, adamSettings);

            this.MarkAsDone(eventId, adamResponse);
        }

        public ProcessDeliveryActionResult ProcessDeliveryActions(Job job)
        {
            List<ProcessDeliveryActionResult> results;
            using (var transactionScope = new TransactionScope())
            {
                results = actionHandlers.Select(p => p.Execute(job)).ToList();

                if (job.CanResolve)
                {
                    job.JobStatus = JobStatus.Resolved;
                }
                jobRepository.Update(job);
                job.JobDetails.ForEach(jd =>
                {
                    jobDetailRepository.Update(jd);
                    jd.JobDetailDamages.ForEach(jdd => jobDetailDamageRepository.Update(jdd));
                });

                transactionScope.Complete();
            }

            return new ProcessDeliveryActionResult
            {
                AdamIsDown = results.Any(p => p.AdamIsDown),
                Warnings = results.SelectMany(p => p.Warnings).ToList()
            };
        }

        public void Pod(PodEvent podEvent, int eventId, AdamSettings adamSettings)
        {
            var job = jobRepository.GetById(podEvent.Id);

            // do not attempt to create a Pod without an invoice number
            if (!string.IsNullOrEmpty(job.InvoiceNumber))
            {
                job = jobService.PopulateLineItemsAndRoute(job);
                job.GetAllLineItemActions();

                var adamResponse = this.adamRepository.Pod(podEvent, adamSettings, job);

                //Mark the podevent as processed, even if it failed - there will be a PodTransaction instead
                this.eventRepository.MarkEventAsProcessed(eventId);
                this.MarkPodAsResolved(podEvent.Id, adamResponse);
            }
        }

        public void PodTransaction(PodTransaction podTransaction, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.PodTransaction(podTransaction, adamSettings);

            this.MarkAsDone(eventId, adamResponse);
            this.MarkPodAsResolved(podTransaction.JobId, adamResponse);
        }

        public void AmendmentTransaction(AmendmentTransaction amendmentTransaction, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.AmendmentTransaction(amendmentTransaction, adamSettings);
            this.MarkAsDone(eventId, adamResponse);
        }

        private void MarkAsDone(int eventId, AdamResponse response)
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

        private void MarkGrnAsComplete(int eventId, AdamResponse response, string username)
        {
            //TODO - prevent re-submission of GRN to ADAM
            //if (response == AdamResponse.Success)
            //{
            //    this.eventRepository.CurrentUser = username;
            //    this.eventRepository.MarkEventAsProcessed(eventId);
            //}
        }
    }
}