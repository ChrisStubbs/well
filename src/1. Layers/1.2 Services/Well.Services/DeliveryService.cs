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
        private readonly IJobDetailActionRepository jobDetailActionRepository;
        private readonly IUserRepository userRepository;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IBranchRepository branchRepository;

        public DeliveryService(IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IJobRepository jobRepository,
            IAuditRepository auditRepository,
            IStopRepository stopRepository,
            IJobDetailActionRepository jobDetailActionRepository,
            IUserRepository userRepository,
            IExceptionEventRepository exceptionEventRepository,
            IDeliveryReadRepository deliveryReadRepository,
            IBranchRepository branchRepository)
        {
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.jobRepository = jobRepository;
            this.auditRepository = auditRepository;
            this.stopRepository = stopRepository;
            this.jobDetailActionRepository = jobDetailActionRepository;
            this.userRepository = userRepository;
            this.exceptionEventRepository = exceptionEventRepository;
            this.deliveryReadRepository = deliveryReadRepository;
            this.branchRepository = branchRepository;
        }

        public IList<Delivery> GetApprovals(string username)
        {
            var approvals = deliveryReadRepository.GetByPendingCredit(username);

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
                approval.SetCanAction(username);
            }

            return approvals.ToList();
        }

        public void UpdateDeliveryLine(JobDetail jobDetailUpdates, string username)
        {
            this.jobDetailRepository.CurrentUser = username;
            this.jobDetailDamageRepository.CurrentUser = username;
            this.jobRepository.CurrentUser = username;
            this.auditRepository.CurrentUser = username;
            this.stopRepository.CurrentUser = username;

            IEnumerable<JobDetail> jobDetails = this.jobDetailRepository.GetByJobId(jobDetailUpdates.JobId);
            bool isCleanBeforeUpdate = jobDetails.All(jd => jd.IsClean());

            var jobDetail =
                jobDetails.Single(j => j.JobId == jobDetailUpdates.JobId && j.LineNumber == jobDetailUpdates.LineNumber);
            jobDetail.ShortQty = jobDetailUpdates.ShortQty;
            jobDetail.JobDetailDamages = jobDetailUpdates.JobDetailDamages;
            jobDetail.JobDetailReasonId = jobDetailUpdates.JobDetailReasonId;
            jobDetail.JobDetailSourceId = jobDetailUpdates.JobDetailSourceId;
            jobDetail.ShortsActionId = jobDetailUpdates.ShortsActionId;

            Job job = this.jobRepository.GetById(jobDetail.JobId);
            job.JobDetails = jobDetails.ToList();
            JobDetail originalJobDetail = this.jobDetailRepository.GetByJobLine(jobDetailUpdates.JobId, jobDetailUpdates.LineNumber);
            Stop stop = this.stopRepository.GetByJobId(jobDetailUpdates.JobId);
            Audit audit = jobDetailUpdates.CreateAuditEntry(originalJobDetail, job.InvoiceNumber, job.PhAccount,
                stop.DeliveryDate);

            using (var transactionScope = new TransactionScope())
            {
                this.jobDetailRepository.Update(jobDetail);
                this.jobDetailDamageRepository.Delete(jobDetail.Id);
                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    this.jobDetailDamageRepository.Save(jobDetailDamage);
                }

                bool isClean = jobDetails.All(jd => jd.IsClean());
                if (isCleanBeforeUpdate && isClean == false)
                {
                    //Make dirty
                    job.PerformanceStatus = PerformanceStatus.Incom;
                    this.jobRepository.Update(job);
                }

                if (isCleanBeforeUpdate == false && isClean)
                {
                    //Resolve
                    job.PerformanceStatus = PerformanceStatus.Resolved;
                    this.jobRepository.Update(job);
                    this.userRepository.UnAssignJobToUser(job.Id);
                }

                if (audit.HasEntry)
                {
                    this.auditRepository.Save(audit);
                }

                transactionScope.Complete();
            }
        }

        public void UpdateDraftActions(JobDetail jobDetailUpdates, string username)
        {
            this.jobDetailActionRepository.CurrentUser = username;
            this.auditRepository.CurrentUser = username;

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
            this.jobDetailActionRepository.CurrentUser = username;
            this.auditRepository.CurrentUser = username;

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
        }

        public void SaveGrn(int jobId, string grn, int branchId, string username)
        {
            this.jobRepository.CurrentUser = username;

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
