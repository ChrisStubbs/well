namespace PH.Well.Domain.ValueObjects
{
    public class CustomerRoyaltyException
    {
        public int RoyaltyId { get; set; }

        public string Customer { get; set; }

        public int ExceptionDays { get; set; }
    }
}
