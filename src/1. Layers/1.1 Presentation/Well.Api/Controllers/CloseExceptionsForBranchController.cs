using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PH.Well.Services.Contracts;

namespace PH.Well.Api.Controllers
{
    public class CloseExceptionsForBranchController : ApiController
    {
        private readonly ILineItemActionService lineItemActionService;

        public CloseExceptionsForBranchController(ILineItemActionService lineItemActionService)
        {
            this.lineItemActionService = lineItemActionService;
        }

        public IHttpActionResult Post(CloseExceptionsForBranchParameters parameters)
        {
            for (int i = 0; i < (parameters.To - parameters.From).Days ; i++)
            {
                this.lineItemActionService.CloseExceptionsForBranch(parameters.BranchId, parameters.From.AddDays(i));
            }

            return Ok();
        }
    }

    public class CloseExceptionsForBranchParameters
    {
        public int BranchId { get; set; }

        private DateTime from;
        public DateTime From
        {
            get
            {
                return this.from;
            }
            set
            {
                this.from = DateTime.SpecifyKind(value, DateTimeKind.Utc).ToLocalTime();
            }
        }

        private DateTime to;
        public DateTime To
        {
            get
            {
                return this.to;
            }
            set
            {
                this.to = DateTime.SpecifyKind(value, DateTimeKind.Utc).ToLocalTime();
            }
        }
    }
}
