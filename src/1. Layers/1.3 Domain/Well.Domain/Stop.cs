namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Extensions;
    using PH.Well.Domain.Enums;

    [Serializable()]
    public class Stop : Entity<int>
    {
        public Stop()
        {
            this.Account = new Account();
            this.Jobs = new List<Job>();
        }

        public string PlannedStopNumber { get; set; }

        public int RouteHeaderId { get; set; }

        public string TransportOrderReference { get; set; }

        public string RouteHeaderCode { get; set; }

        public string DropId { get; set; }

        public string LocationId { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string ShellActionIndicator { get; set; }

        public bool AllowOvers { get; set; }

        public bool CustUnatt { get; set; }

        public bool PHUnatt { get; set; }

        public string StopStatusCode { get; set; }

        public string StopStatusDescription { get; set; }

        public string PerformanceStatusCode { get; set; }

        public string PerformanceStatusDescription { get; set; }

        public string StopByPassReason { get; set; }

        public Account Account { get; set; }

        public List<Job> Jobs { get; set; }

        public decimal ActualPaymentCash { get; set; }

        public decimal ActualPaymentCheque { get; set; }

        public decimal ActualPaymentCard { get; set; }

        public decimal AccountBalance { get; set; }

        public int CleanJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Clean);

        public int ExceptionJobsCount => Jobs.Count(j => j.JobStatus == JobStatus.Exception);

        public WellStatus WellStatus { get; set; }

        public string Previously { get; set; }

        public void SetPreviously(Stop original)
        {
            if (!original.DeliveryDate.ToShortDateString().Equals(DeliveryDate.ToShortDateString(), StringComparison.InvariantCultureIgnoreCase))
            {
                Previously = $"{original.DropId} - {original.DeliveryDate.ToShortDateString()}";
            }

            Previously = $"{DropId} ";
        }

    }
}