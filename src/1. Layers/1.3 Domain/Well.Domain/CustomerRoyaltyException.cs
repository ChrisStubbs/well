namespace PH.Well.Domain
{
    public class CustomerRoyaltyException : Entity<int>
    {
        public int RoyaltyId { get; set; }

        public string Customer { get; set; }

        public int ExceptionDays { get; set; }
    }
}
