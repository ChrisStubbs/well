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
        private readonly IJobDetailRepository jobDetailRepo;
        private readonly IJobDetailDamageRepo jobDetailDamageRepo;
        private readonly IJobRepository jobRepo;
        private readonly IAuditRepository auditRepo;
        private readonly IStopRepository stopRepo;

        public DeliveryService(IJobDetailRepository jobDetailRepo,
            IJobDetailDamageRepo jobDetailDamageRepo,
            IJobRepository jobRepo,
            IAuditRepository auditRepo,
            IStopRepository stopRepo)
        {
            this.jobDetailRepo = jobDetailRepo;
            this.jobDetailDamageRepo = jobDetailDamageRepo;
            this.jobRepo = jobRepo;
            this.auditRepo = auditRepo;
            this.stopRepo = stopRepo;
        }

        public void UpdateDeliveryLine(JobDetail jobDetailUpdates, string username)
        {
            jobDetailRepo.CurrentUser = username;
            jobDetailDamageRepo.CurrentUser = username;
            jobRepo.CurrentUser = username;
            auditRepo.CurrentUser = username;
            stopRepo.CurrentUser = username;

            IEnumerable<JobDetail> jobDetails = jobDetailRepo.GetByJobId(jobDetailUpdates.JobId);
            bool isCleanBeforeUpdate = jobDetails.All(jd => jd.IsClean());

            var jobDetail = jobDetails.Single(j => j.JobId == jobDetailUpdates.JobId && j.LineNumber == jobDetailUpdates.LineNumber);
            jobDetail.ShortQty = jobDetailUpdates.ShortQty;
            jobDetail.JobDetailDamages = jobDetailUpdates.JobDetailDamages;

            Job job = jobRepo.GetById(jobDetail.JobId);
            JobDetail originalJobDetail = jobDetailRepo.GetByJobLine(jobDetailUpdates.JobId, jobDetailUpdates.LineNumber);
            Stop stop = stopRepo.GetByJobId(jobDetailUpdates.JobId);
            Audit audit = jobDetailUpdates.CreateAuditEntry(originalJobDetail, job.JobRef3, job.JobRef1, stop.DeliveryDate);

            using (var transactionScope = new TransactionScope())
            {
                jobDetailRepo.Update(jobDetail);
                jobDetailDamageRepo.Delete(jobDetail.Id);
                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    jobDetailDamageRepo.Save(jobDetailDamage);
                }

                bool isClean = jobDetails.All(jd => jd.IsClean());
                if (isCleanBeforeUpdate && isClean == false)
                {
                    //Make dirty
                    job.PerformanceStatus = PerformanceStatus.Incom;
                    jobRepo.JobCreateOrUpdate(job);
                }

                if (isCleanBeforeUpdate == false && isClean)
                {
                    //Resolve
                    job.PerformanceStatus = PerformanceStatus.Resolved;
                    jobRepo.JobCreateOrUpdate(job);
                }

                if (audit.HasEntry)
                {
                    auditRepo.Save(audit);
                }

                transactionScope.Complete();
            }
        }
    }
}
