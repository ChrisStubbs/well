namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using System.Xml.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Repositories.Contracts;

    public class DeliveryService : IDeliveryService
    {
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepo jobDetailDamageRepo;
        private readonly IJobRepository jobRepo;

        public DeliveryService(IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepo jobDetailDamageRepo,
            IJobRepository jobRepo)
        {
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepo = jobDetailDamageRepo;
            this.jobRepo = jobRepo;
        }

        public void UpdateDeliveryLine(JobDetail jobDetailUpdates, string username)
        {
            jobDetailRepository.CurrentUser = username;
            jobDetailDamageRepo.CurrentUser = username;
            jobRepo.CurrentUser = username;

            IEnumerable<JobDetail> jobDetails = jobDetailRepository.GetByJobId(jobDetailUpdates.JobId);
            bool isCleanBeforeUpdate = jobDetails.All(jd => jd.IsClean());

            var jobDetail = jobDetails.Single(j => j.JobId == jobDetailUpdates.JobId && j.LineNumber == jobDetailUpdates.LineNumber);
            jobDetail.ShortQty = jobDetailUpdates.ShortQty;
            jobDetail.JobDetailDamages = jobDetailUpdates.JobDetailDamages;

            using (var transactionScope = new TransactionScope())
            {
                jobDetailRepository.Update(jobDetail);
                jobDetailDamageRepo.Delete(jobDetail.Id);
                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    jobDetailDamageRepo.Save(jobDetailDamage);
                }

                bool isClean = jobDetails.All(jd => jd.IsClean());
                if (isCleanBeforeUpdate && isClean == false)
                {
                    //Make dirty
                    Job job = jobRepo.GetById(jobDetail.JobId);
                    job.PerformanceStatusId = (int)PerformanceStatus.Incom;
                    jobRepo.JobCreateOrUpdate(job);
                }

                if (isCleanBeforeUpdate == false && isClean)
                {
                    //Resolve
                    Job job = jobRepo.GetById(jobDetail.JobId);
                    job.PerformanceStatusId = (int)PerformanceStatus.Resolved;
                    jobRepo.JobCreateOrUpdate(job);
                }

                transactionScope.Complete();
            }
        }
    }
}
