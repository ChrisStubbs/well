namespace PH.Well.Domain.ValueObjects
{
    using System;

    public class AppSearchParameters
    {
        public int? BranchId { get; set; }
        public DateTime? Date { get; set; }
        public string Account { get; set; }
        public string Invoice { get; set; }
        public string Route { get; set; }
        public string Driver { get; set; }

        /// <summary>
        /// This is a subset of the JobType table
        /// </summary>
        public int? DeliveryType { get; set; }

        /// <summary>
        /// Route status - from WellStatus Descriptions (minus "Invoiced")
        /// </summary>
        public int? Status { get; set; }

        public bool HasBranch => BranchId.HasValue;
        public bool HasDate => Date.HasValue;
        public bool HasDeliveryType => DeliveryType.HasValue;
        public bool HasStatus => Status.HasValue;
        public bool HasInvoice => !string.IsNullOrEmpty(this.Invoice);
        public bool HasAccount => !string.IsNullOrEmpty(this.Account);
        public bool HasRoute=> !string.IsNullOrEmpty(this.Route);
        public bool HasDriver => !string.IsNullOrEmpty(this.Driver);
        public bool IsRouteSearch => HasBranch && !HasInvoice && !HasAccount;

        public void Format()
        {
            Driver = Driver?.Trim();
            Account = Account?.Trim();
            Invoice = Invoice?.Trim();
            Route = Route?.Trim();
        }
    }
}
