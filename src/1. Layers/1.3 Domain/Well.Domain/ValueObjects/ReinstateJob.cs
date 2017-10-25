using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain.Enums;

namespace PH.Well.Domain.ValueObjects
{
    public class ReinstateJob
    {
        public int JobId { get; set; }

        public int StopId { get; set; }

        public string PhAccount { get; set; }

        public string PickListRef { get; set; }

        public string JobTypeCode { get; set; }

        public int WellStatusId { get; set; }

        public WellStatus WellStatus => (WellStatus)this.WellStatusId;

        public int JobStatusId { get; set; }

        public JobStatus JobStatus => (JobStatus)this.JobStatusId;

        public string GrnNumber { get; set; }

        public string RoyaltyCode { get; set; }

        public int BranchId { get; set; }

        public DateTime RouteDate { get; set; }

        public int ResolutionStatusId { get; set; }

        public ResolutionStatus ResolutionStatus => (ResolutionStatus)this.ResolutionStatusId;

        public int Sequence { get; set; }

        public string CustomerRef { get; set; }

        public int OrdOuters { get; set; }

        public int InvOuters { get; set; }

        public string Cod { get; set; }

        public string Reason { get; set; }

        public int OuterCount { get; set; }

        public int TotalOutersOver { get; set; }

        public int DetailOutersOver { get; set; }

        public bool Picked { get; set; }

        public int TotalOutersShort { get; set; }

        public int PerformanceStatusId { get; set; }

        public int? ProofOfDelivery { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
