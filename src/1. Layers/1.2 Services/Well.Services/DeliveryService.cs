namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class DeliveryService : IDeliveryService
    {
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;
        private readonly IJobRepository jobRepository;
        private readonly IAuditRepository auditRepository;
        private readonly IStopRepository stopRepository;
        private readonly IUserRepository userRepository;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IBranchRepository branchRepository;
        private readonly IJobStatusService jobStatusService;

        public DeliveryService(IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IJobRepository jobRepository,
            IAuditRepository auditRepository,
            IStopRepository stopRepository,
            IUserRepository userRepository,
            IExceptionEventRepository exceptionEventRepository,
            IDeliveryReadRepository deliveryReadRepository,
            IBranchRepository branchRepository,
            IJobStatusService jobStatusService)
        {
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.jobRepository = jobRepository;
            this.auditRepository = auditRepository;
            this.stopRepository = stopRepository;
            this.userRepository = userRepository;
            this.exceptionEventRepository = exceptionEventRepository;
            this.deliveryReadRepository = deliveryReadRepository;
            this.branchRepository = branchRepository;
            this.jobStatusService = jobStatusService;
        }

        public IList<Delivery> GetExceptions(string username)
        {
            var statuses = new List<JobStatus>() { JobStatus.Exception, JobStatus.CompletedOnPaper, JobStatus.Bypassed };
            var exceptionDeliveries = deliveryReadRepository.GetByStatuses(username, statuses).ToList();
            exceptionDeliveries = exceptionDeliveries.Where(e => e.IsPendingCredit == false).ToList();
            return exceptionDeliveries;
        }

        public IList<Delivery> GetApprovals(string username)
        {
            var statuses = new List<JobStatus>() { JobStatus.Exception, JobStatus.CompletedOnPaper };
            var approvals = deliveryReadRepository.GetByStatuses(username, statuses);
            approvals = approvals.Where(j => j.IsPendingCredit);

            //Populate Thresholds
            var branches = branchRepository.GetAllValidBranches();
            foreach (var approval in approvals)
            {
                var branch = branches.SingleOrDefault(b => b.Id == approval.BranchId);
                if (branch == null)
                {
                    continue;
                }
                var threshold = branch.CreditThresholds.OrderBy(t => t.Threshold)
                    .FirstOrDefault(t => t.Threshold >= approval.TotalCreditValueForThreshold);

                approval.ThresholdLevel = threshold?.ThresholdLevel;
            }

            return approvals.ToList();
        }

        // TODO refactor and try to simplify
        public void UpdateDeliveryLine(JobDetail jobDetailUpdates, string username)
        {
            IEnumerable<JobDetail> jobDetails = this.jobDetailRepository.GetByJobId(jobDetailUpdates.JobId);

            var jobDetail =
                jobDetails.Single(j => j.JobId == jobDetailUpdates.JobId && j.LineNumber == jobDetailUpdates.LineNumber);

            Stop stop = this.stopRepository.GetByJobId(jobDetailUpdates.JobId);
            Job job = this.jobRepository.GetById(jobDetail.JobId);

            Audit audit = jobDetailUpdates.CreateAuditEntry(jobDetail, job.InvoiceNumber, job.PhAccount,
                stop.DeliveryDate);

            var branchId = this.branchRepository.GetBranchIdForJob(job.Id);

            bool isCleanBeforeUpdate = job.JobStatus == JobStatus.Clean;
            
            jobDetail.ShortQty = jobDetailUpdates.ShortQty;
            jobDetail.JobDetailDamages = jobDetailUpdates.JobDetailDamages;
            jobDetail.JobDetailReasonId = jobDetailUpdates.JobDetailReasonId;
            jobDetail.JobDetailSourceId = jobDetailUpdates.JobDetailSourceId;
            jobDetail.ShortsActionId = jobDetailUpdates.ShortsActionId;

            job.JobDetails = jobDetails.ToList();

            using (var transactionScope = new TransactionScope())
            {
                jobDetailRepository.Update(jobDetail);
                jobDetailDamageRepository.Delete(jobDetail.Id);

                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    jobDetailDamageRepository.Save(jobDetailDamage);
                }

                job = jobStatusService.DetermineStatus(job, branchId);
                // was originally dirty now clean so can be resolved
                if (!isCleanBeforeUpdate && job.JobStatus == JobStatus.Clean)
                {
                    job.JobStatus = JobStatus.Resolved;
                }
                jobRepository.UpdateStatus(job.Id, job.JobStatus);

                if (audit.HasEntry)
                {
                    auditRepository.Save(audit);
                }

                transactionScope.Complete();
            }
        }

        /*public void UpdateDraftActions(JobDetail jobDetailUpdates, string username)
        {
            //////this.jobDetailActionRepository.CurrentUser = username;
            //////this.auditRepository.CurrentUser = username;

            Job job = this.jobRepository.GetById(jobDetailUpdates.JobId);
            JobDetail originalJobDetail = this.jobDetailRepository.GetByJobLine(jobDetailUpdates.JobId, jobDetailUpdates.LineNumber);
            Stop stop = this.stopRepository.GetByJobId(jobDetailUpdates.JobId);
            Audit audit = jobDetailUpdates.CreateAuditEntry(originalJobDetail, job.InvoiceNumber, job.PhAccount,
                stop.DeliveryDate);

            using (var transactionScope = new TransactionScope())
            {
                this.jobDetailActionRepository.DeleteDrafts(jobDetailUpdates.Id);

                //Save draft actions
                foreach (var action in jobDetailUpdates.Actions.Where(a => a.Status == ActionStatus.Draft))
                {
                    this.jobDetailActionRepository.Save(action);
                }

                //Audit changes
                if (audit.HasEntry)
                {
                    this.auditRepository.Save(audit);
                }

                transactionScope.Complete();
            }
        }

        public void SubmitActions(int jobId, string username)
        {
            Job job = this.jobRepository.GetById(jobId);
            Stop stop = this.stopRepository.GetByJobId(jobId);

            var jobDetailsList = this.jobDetailRepository.GetByJobId(jobId);

            using (var transactionScope = new TransactionScope())
            {
                foreach (var jobDetails in jobDetailsList)
                {
                    JobDetail originalJobDetail = this.jobDetailRepository.GetById(jobDetails.Id);

                    foreach (var draftAction in jobDetails.Actions.Where(a => a.Status == ActionStatus.Draft))
                    {
                        draftAction.Status = ActionStatus.Submitted;
                        this.jobDetailActionRepository.Update(draftAction);
                    }
                    
                    Audit audit = jobDetails.CreateAuditEntry(originalJobDetail, job.InvoiceNumber, job.PhAccount,
                        stop.DeliveryDate);
                    if (audit.HasEntry)
                    {
                        this.auditRepository.Save(audit);
                    }
                }
                transactionScope.Complete();
            }
        }*/

        public void SaveGrn(int jobId, string grn, int branchId, string username)
        {
            //////this.jobRepository.CurrentUser = username;

            using (var transactionScope = new TransactionScope())
            {
                this.jobRepository.SaveGrn(jobId, grn);

                var grnEvent = new GrnEvent();
                grnEvent.Id = jobId;
                grnEvent.BranchId = branchId;
                this.exceptionEventRepository.InsertGrnEvent(grnEvent);

                transactionScope.Complete();

            }
        }
    }
}
