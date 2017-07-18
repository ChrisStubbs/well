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
        private readonly IStopRepository stopRepository;
        private readonly IUserRepository userRepository;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IBranchRepository branchRepository;
        private readonly IJobService jobStatusService;
        private readonly IDateThresholdService _dateThresholdService;

        public DeliveryService(IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IJobRepository jobRepository,
            IStopRepository stopRepository,
            IUserRepository userRepository,
            IExceptionEventRepository exceptionEventRepository,
            IDeliveryReadRepository deliveryReadRepository,
            IBranchRepository branchRepository,
            IJobService jobStatusService,
            IDateThresholdService dateThresholdService)
        {
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.jobRepository = jobRepository;
            this.stopRepository = stopRepository;
            this.userRepository = userRepository;
            this.exceptionEventRepository = exceptionEventRepository;
            this.deliveryReadRepository = deliveryReadRepository;
            this.branchRepository = branchRepository;
            this.jobStatusService = jobStatusService;
            _dateThresholdService = dateThresholdService;
        }

        public IList<Delivery> GetExceptions(string username)
        {
           
            var exceptionDeliveries = deliveryReadRepository.GetByStatuses(username, ExceptionStatuses.JobStatuses).ToList();
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
                
                transactionScope.Complete();
            }
        }
        
        public void SaveGrn(int jobId, string grn, int branchId, string username)
        {
            //////this.jobRepository.CurrentUser = username;

            using (var transactionScope = new TransactionScope())
            {
                this.jobRepository.SaveGrn(jobId, grn);
                var jobRoute = jobRepository.GetJobRoute(jobId);

                var grnEvent = new GrnEvent();
                grnEvent.Id = jobId;
                grnEvent.BranchId = branchId;
                this.exceptionEventRepository.InsertGrnEvent(grnEvent,
                    _dateThresholdService.EarliestSubmitDate(jobRoute.RouteDate, branchId));

                transactionScope.Complete();

            }
        }
    }
}
