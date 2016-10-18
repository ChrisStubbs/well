namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Transactions;
    using System.Xml.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class DeliveryService : IDeliveryService
    {
        private readonly IJobDetailRepository jobDetailRepo;
        private readonly IJobDetailDamageRepo jobDetailDamageRepo;
        private readonly IJobRepository jobRepo;
        private readonly IAuditRepository auditRepo;
        private readonly IStopRepository stopRepo;
        private readonly IJobDetailActionRepository jobDetailActionRepository;
        private readonly IUserRepository userRepository;

        public DeliveryService(IJobDetailRepository jobDetailRepo,
            IJobDetailDamageRepo jobDetailDamageRepo,
            IJobRepository jobRepo,
            IAuditRepository auditRepo,
            IStopRepository stopRepo,
            IJobDetailActionRepository jobDetailActionRepository,
            IUserRepository userRepository)
        {
            this.jobDetailRepo = jobDetailRepo;
            this.jobDetailDamageRepo = jobDetailDamageRepo;
            this.jobRepo = jobRepo;
            this.auditRepo = auditRepo;
            this.stopRepo = stopRepo;
            this.jobDetailActionRepository = jobDetailActionRepository;
            this.userRepository = userRepository;
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

            var jobDetail =
                jobDetails.Single(j => j.JobId == jobDetailUpdates.JobId && j.LineNumber == jobDetailUpdates.LineNumber);
            jobDetail.ShortQty = jobDetailUpdates.ShortQty;
            jobDetail.JobDetailDamages = jobDetailUpdates.JobDetailDamages;

            Job job = jobRepo.GetById(jobDetail.JobId);
            JobDetail originalJobDetail = jobDetailRepo.GetByJobLine(jobDetailUpdates.JobId, jobDetailUpdates.LineNumber);
            Stop stop = stopRepo.GetByJobId(jobDetailUpdates.JobId);
            Audit audit = jobDetailUpdates.CreateAuditEntry(originalJobDetail, job.InvoiceNumber, job.PhAccount,
                stop.DeliveryDate);

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

        public void UpdateDraftActions(JobDetail jobDetailUpdates, string username)
        {
            this.jobDetailActionRepository.CurrentUser = username;
            auditRepo.CurrentUser = username;

            Job job = jobRepo.GetById(jobDetailUpdates.JobId);
            JobDetail originalJobDetail = jobDetailRepo.GetByJobLine(jobDetailUpdates.JobId, jobDetailUpdates.LineNumber);
            Stop stop = stopRepo.GetByJobId(jobDetailUpdates.JobId);
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
                    auditRepo.Save(audit);
                }

                transactionScope.Complete();
            }
        }

        public void SubmitActions(int jobId, string username)
        {
            this.jobDetailActionRepository.CurrentUser = username;
            auditRepo.CurrentUser = username;

            Job job = jobRepo.GetById(jobId);
            Stop stop = stopRepo.GetByJobId(jobId);

            var jobDetailsList = jobDetailRepo.GetByJobId(jobId);

            using (var transactionScope = new TransactionScope())
            {
                foreach (var jobDetails in jobDetailsList)
                {
                    JobDetail originalJobDetail = jobDetailRepo.GetById(jobDetails.Id);

                    foreach (var draftAction in jobDetails.Actions.Where(a => a.Status == ActionStatus.Draft))
                    {
                        draftAction.Status = ActionStatus.Submitted;
                        this.jobDetailActionRepository.Update(draftAction);
                    }
                    
                    Audit audit = jobDetails.CreateAuditEntry(originalJobDetail, job.InvoiceNumber, job.PhAccount,
                        stop.DeliveryDate);
                    if (audit.HasEntry)
                    {
                        auditRepo.Save(audit);
                    }
                }
                transactionScope.Complete();
            }
        }

        public void CreditLines(IEnumerable<CreditLines> creditLines, string username)
        {
            var creditLinesTable = GetPendingCreditsTable(creditLines);

           this.jobRepo.CreditLines(creditLinesTable);
           this.jobDetailRepo.CreditLines(creditLinesTable);
           var userId = this.userRepository.GetByIdentity(username).Id;
           this.jobRepo.JobPendingCredits(creditLinesTable, userId);
        }

        private DataTable GetPendingCreditsTable(IEnumerable<CreditLines> creditLines)
        {
            var dt = new DataTable();
            dt.Columns.Add("CreditId");
            dt.Columns["CreditId"].DataType = typeof(int);
            dt.Columns.Add("IsPending");
            dt.Columns["IsPending"].DataType = typeof(bool);
            foreach (var i in creditLines)
            {
                DataRow row = dt.NewRow();
                row["CreditId"] = i.CreditId;
                row["IsPending"] = i.IsPending;
                dt.Rows.Add(row);
            }

            return dt;
        }

    }
}
