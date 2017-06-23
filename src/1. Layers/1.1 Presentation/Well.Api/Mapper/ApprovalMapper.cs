namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
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
                approvals.Add(new ApprovalModel
                {
                    JobId = job.Id,
                    BranchId = job.JobRoute.BranchId,
                    BranchName = Branch.GetBranchName(job.JobRoute.BranchId, job.JobRoute.BranchName),
                    RouteDate = job.JobRoute.RouteDate,
                    AccountId = job.PhAccountId,
                    Account = job.PhAccount,
                    InvoiceNumber = job.InvoiceNumber,
                    DateSubmitted = job.ResolutionStatusHistory.Where(x=> x.Status == ResolutionStatus.PendingApproval.Description).OrderByDescending(x=> x.On).First().On,
                    CreditQuantity = job.TotalCreditQty,
                    CreditValue = job.CreditValue,
                    AssignedTo = Assignee.GetDisplayNames(assignees.Where(x=> x.JobId == job.Id))
                });
            }
            
            return approvals;
        }
    }
}