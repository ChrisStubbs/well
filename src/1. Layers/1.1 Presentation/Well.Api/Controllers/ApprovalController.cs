namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Domain;
    using Services.Contracts;

    public class ApprovalController : ApiController
    {
        private readonly IApprovalService approvalService;

        public ApprovalController(IApprovalService approvalService)
        {
            this.approvalService = approvalService;
        }

        [Route("{branchId:int}/Approval")]
        
        public IEnumerable<JobToBeApproved> Get(int branchId)
        {
            return approvalService.GetJobsToBeApproved(branchId);
        }
    }
}
