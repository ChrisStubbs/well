namespace PH.Well.Services
{
    using System;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class PodService : IPodService
    {
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IDateThresholdService dateThresholdService;

        public PodService(IExceptionEventRepository exceptionEventRepository, IDateThresholdService dateThresholdService)
        {
            this.exceptionEventRepository = exceptionEventRepository;
            this.dateThresholdService = dateThresholdService;
        }


        public void CreatePodEvent(Job job, int branchId, DateTime routeDate)
        {
            var royaltyCode = job.GetRoyaltyCode();
            if (!exceptionEventRepository.IsPodEventCreatedForJob(job.Id.ToString()))
            {
                var podEvent = new PodEvent
                {
                    BranchId = branchId,
                    Id = job.Id
                };
                this.exceptionEventRepository.InsertPodEvent(podEvent, job.Id.ToString(), dateThresholdService.GracePeriodEnd(routeDate, branchId, royaltyCode));
            }
        }
    }
}
