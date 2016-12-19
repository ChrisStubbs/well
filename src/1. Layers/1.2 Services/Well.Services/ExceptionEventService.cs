namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Transactions;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class ExceptionEventService : IExceptionEventService
    {
        private readonly IAdamRepository adamRepository;
        private readonly IExceptionEventRepository eventRepository;
        private readonly IJobRepository jobRepository;

        public ExceptionEventService(
            IAdamRepository adamRepository,
            IExceptionEventRepository eventRepository,
            IJobRepository jobRepository)
        {
            this.adamRepository = adamRepository;
            this.eventRepository = eventRepository;
            this.jobRepository = jobRepository;
        }

        public void Credit(CreditEvent creditEvent, int eventId, AdamSettings adamSettings, string username)
        {
            var adamResponse = this.adamRepository.Credit(creditEvent, adamSettings);

            this.MarkAsDone(eventId, adamResponse, username);
        }

        public AdamResponse Credit(CreditEvent creditEvent, AdamSettings adamSettings, string username)
        {
            var response = this.adamRepository.Credit(creditEvent, adamSettings);

            if (response == AdamResponse.AdamDown)
            {
                this.eventRepository.CurrentUser = username;
                this.eventRepository.InsertCreditEvent(creditEvent);
            }
            else
            {
                this.jobRepository.ResolveJobAndJobDetails(creditEvent.Id);
                this.eventRepository.RemovedPendingCredit(creditEvent.InvoiceNumber);

                return AdamResponse.Success;
            }

            return response;
        }

        public AdamResponse BulkCredit(IEnumerable<CreditEvent> creditEvents, string username)
        {
            var adamDown = false;

            foreach (var creditEvent in creditEvents)
            {
                var settings = AdamSettingsFactory.GetAdamSettings((Branch)creditEvent.BranchId);

                using (var transactionScope = new TransactionScope())
                {
                    var response = this.adamRepository.Credit(creditEvent, settings);

                    if (response == AdamResponse.AdamDown)
                    {
                        this.eventRepository.CurrentUser = username;
                        this.eventRepository.InsertCreditEvent(creditEvent);
                        adamDown = true;
                    }
                    else
                    {
                        this.jobRepository.ResolveJobAndJobDetails(creditEvent.Id);
                        this.eventRepository.RemovedPendingCredit(creditEvent.InvoiceNumber);
                    }

                    transactionScope.Complete();
                }
            }

            return adamDown ? AdamResponse.AdamDown : AdamResponse.Success;
         }

        public void CreditReorder(CreditReorderEvent creditReorderEvent, int eventId, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse CreditReorder(CreditReorderEvent creditReorderEvent, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public void Reject(RejectEvent rejectEvent, int eventId, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse Reject(RejectEvent rejectEvent, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public void ReplanRoadnet(RoadnetEvent roadnetEvent, int eventId, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse ReplanRoadnet(RoadnetEvent roadnetEvent, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public void ReplanTranscend(TranscendEvent transcendEvent, int eventId, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse ReplanTranscend(TranscendEvent transcendEvent, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public void ReplanQueue(QueueEvent queueEvent, int eventId, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        public AdamResponse ReplanQueue(QueueEvent queueEvent, AdamSettings adamSettings, string username)
        {
            throw new System.NotImplementedException();
        }

        private void MarkAsDone(int eventId, AdamResponse response, string username)
        {
            if (response == AdamResponse.Success)
            {
                this.eventRepository.CurrentUser = username;
                this.eventRepository.MarkEventAsProcessed(eventId);
            }
        }
    }
}