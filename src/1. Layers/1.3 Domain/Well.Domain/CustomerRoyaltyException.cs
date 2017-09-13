namespace PH.Well.Domain
{
    public class CustomerRoyaltyExceptionWell : Entity<int>
    {
        public int RoyaltyCode { get; set; }

        public string Customer { get; set; }

        public byte ExceptionDays { get; set; }
    }
}
