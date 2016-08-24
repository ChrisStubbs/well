namespace PH.Well.Domain
{
    using System;
    using System.Diagnostics.Contracts;

    public class CustomerRoyaltyException :Entity<int>
    {
        public short Royalty { get; set; }

        public string Customer { get; set; }
    }
}
