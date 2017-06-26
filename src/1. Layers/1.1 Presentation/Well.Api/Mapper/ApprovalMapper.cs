namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Models;
    using Branch = Domain.Branch;

    public class ApprovalMapper : IApprovalMapper
    {
        public IEnumerable<ApprovalModel> Map(IEnumerable<Job> jobs, IEnumerable<Assignee> assignees)
        {
            var approvals = new List<ApprovalModel>();

            foreach (var job in jobs)
            {
                var submittedInfo = job.ResolutionStatusHistory.Where(x => x.Status == ResolutionStatus.PendingApproval.Description).OrderByDescending(x => x.On).First();
                approvals.Add(new ApprovalModel
                {
                    JobId = job.Id,
                    BranchId = job.JobRoute.BranchId,
                    BranchName = Branch.GetBranchName(job.JobRoute.BranchId, job.JobRoute.BranchName),
                    DeliveryDate = job.JobRoute.RouteDate,
                    AccountId = job.PhAccountId,
                    Account = job.PhAccount,
                    InvoiceNumber = job.InvoiceNumber,
                    SubmittedBy = submittedInfo.By.StripDomain(),
                    DateSubmitted = submittedInfo.On,
                    CreditQuantity = job.TotalCreditQty,
                    CreditValue = job.CreditValue,
                    AssignedTo = Assignee.GetDisplayNames(assignees.Where(x=> x.JobId == job.Id))
                });
            }
            
            return approvals.OrderBy(x=> x.DateSubmitted);
        }
    }
}